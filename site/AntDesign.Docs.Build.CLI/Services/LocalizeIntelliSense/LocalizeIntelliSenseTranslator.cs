// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This code is modified from the soures of https://github.com/stratosblue/IntelliSenseLocalizer

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using AntDesign.Docs.Build.CLI.Services.Translation;
using Microsoft.Extensions.Logging;

namespace IntelliSenseLocalizer.ThirdParty;

public class LocalizeIntelliSenseTranslator
{
    #region Private 字段

    //private static readonly IBaseAnyEncoder<char> s_base62Encoder = BaseAnyEncoding.CreateEncoder("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".AsSpan());
    private ITranslate _translator;

    private readonly ILogger _logger;

    #endregion Private 字段

    #region Public 构造函数

    public LocalizeIntelliSenseTranslator(ILogger<LocalizeIntelliSenseTranslator> logger, ITranslate translator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _translator = translator;
    }

    #endregion Public 构造函数

    #region Public 方法

    public virtual async Task TranslateAsync(TranslateContext context, CancellationToken cancellationToken = default)
    {
        var sourceXmlDocument = new XmlDocument();
        sourceXmlDocument.Load(context.FilePath);

        XmlDocument outputXmlDocument;

        if (context.IsPatch)
        {
            outputXmlDocument = await PatchTranslateAsync(context, sourceXmlDocument, cancellationToken);
        }
        else
        {
            outputXmlDocument = await TranslateAsync(context, sourceXmlDocument, cancellationToken);
        }

        //排序内容
        var membersNode = GetMembersNode(outputXmlDocument);
        OrderChildNodesByNameAttribute(membersNode);

        var outDir = Path.GetDirectoryName(context.OutputPath);

        if (!Directory.Exists(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        _logger.LogDebug("[{File}] processing completed. Save the file into {OutputPath}.", context.FilePath, context.OutputPath);

        outputXmlDocument.Save(context.OutputPath);
    }

    #endregion Public 方法

    #region Protected 方法

    protected virtual async Task<XmlDocument> PatchTranslateAsync(TranslateContext context, XmlDocument sourceXmlDocument, CancellationToken cancellationToken)
    {
        var patchXmlDocument = new XmlDocument();
        patchXmlDocument.Load(context.OutputPath);

        var outputXmlDocument = (XmlDocument)patchXmlDocument.Clone();

        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = context.ParallelCount > 0 ? context.ParallelCount : 1,
            CancellationToken = cancellationToken,
        };

        var shouldTranslateNodes = new List<XmlNode>();

        var membersNode = GetMembersNode(outputXmlDocument);

        var sourceMembers = SelectMembersMap(sourceXmlDocument);
        var outputMembers = SelectMembersMap(outputXmlDocument);

        foreach (var (name, outputNode) in outputMembers)
        {
            //移除已不存在的节点
            if (!sourceMembers.TryGetValue(name, out var sourceNode))
            {
                outputNode.ParentNode!.RemoveChild(outputNode);
            }
            else    //处理还存在节点的子节点
            {
                var sourceChildNodesMap = GetProcessableChildNodesMap(sourceNode);
                var outputChildNodesMap = GetProcessableChildNodesMap(outputNode);

                foreach (var (outputChildName, outputChild) in outputChildNodesMap)
                {
                    //移除已不存在的子节点
                    if (!sourceChildNodesMap.TryGetValue(outputChildName, out var sourceChild)
                        || sourceChild.Name != outputChild.Name)
                    {
                        outputChild.ParentNode!.RemoveChild(outputChild);
                    }
                    else
                    {
                        var isIgnore = outputChild.Attributes?.GetNamedItem("i")?.Value == "1";

                        if (isIgnore)
                        {
                            continue;
                        }

                        var existedVersion = outputChild.Attributes?.GetNamedItem("v")?.Value;
                        //var version = GetContentVersion(sourceChild.InnerXml);
                        //if (string.Equals(version, existedVersion))
                        //{
                        //    continue;
                        //}

                        outputChild.InnerXml = sourceChild.InnerXml;

                        shouldTranslateNodes.Add(outputChild);
                    }
                }

                //添加子节点
                foreach (var (sourceChildName, sourceChild) in sourceChildNodesMap)
                {
                    if (!outputChildNodesMap.TryGetValue(sourceChildName, out var outputChild))
                    {
                        var importedNode = outputXmlDocument.ImportNode(sourceChild, true);
                        outputNode.AppendChild(importedNode);

                        shouldTranslateNodes.Add(importedNode);
                    }
                }

                OrderChildNodesByNameAttribute(outputNode);
            }
        }

        foreach (var (name, node) in sourceMembers)
        {
            //添加缺少节点
            if (!outputMembers.TryGetValue(name, out var outputNode))
            {
                var importedNode = outputXmlDocument.ImportNode(node, true);
                membersNode.AppendChild(importedNode);

                shouldTranslateNodes.AddRange(SelectProcessableChildNodes(importedNode));
            }
        }

        //await TranslateNodesAsync( context, shouldTranslateNodes, cancellationToken);

        return outputXmlDocument;
    }

    protected virtual async Task<XmlDocument> TranslateAsync(TranslateContext context, XmlDocument sourceXmlDocument, CancellationToken cancellationToken)
    {
        var outputXmlDocument = (XmlDocument)sourceXmlDocument.Clone();

        var memberNodes = SelectMembersMap(sourceXmlDocument);
        var groupedMenberNodes = memberNodes.GroupBy(x =>
        {
            var name = x.Key.Split(':')[2];
            return string.Join(".", name.Split('.').Take(2).ToArray());
        }).ToDictionary(x => x.Key, x => x.ToList());

        foreach (var group in groupedMenberNodes)
        {
            foreach (var (name, node) in group.Value)
            {
                var memberChildNotes = SelectProcessableChildNodes(node);
                await TranslateNodesAsync(group.Key, context, memberChildNotes, cancellationToken);

                foreach (var item in memberChildNotes)
                {
                    OrderChildNodesByNameAttribute(item);
                }
            }
        }

        //var nodes = SelectNodes(outputXmlDocument, "//summary", "//typeparam", "//param", "//returns");

        //await TranslateNodesAsync(context, nodes, cancellationToken);

        ////排序
        //foreach (var item in SelectMembersMap(outputXmlDocument).SelectMany(m => SelectProcessableChildNodes(m.Value)))
        //{
        //    OrderChildNodesByNameAttribute(item);
        //}

        return outputXmlDocument;
    }

    #endregion Protected 方法

    #region Private 方法

    //private static string GetContentVersion(string content)
    //{
    //    return s_base62Encoder.EncodeToString(Crc32.Hash(Encoding.UTF8.GetBytes(content.Trim())));
    //}

    private static XmlNode GetMembersNode(XmlDocument outputXmlDocument)
    {
        return outputXmlDocument.SelectSingleNode("/doc/members")!;
    }

    private static void OrderChildNodes<TOrderKey>(XmlNode node, Func<XmlNode, TOrderKey> keySelector)
    {
        List<XmlAttribute>? attributes = null;
        if (node.Attributes is not null)
        {
            attributes = [];
            foreach (XmlAttribute item in node.Attributes)
            {
                attributes.Add(item);
            }
        }

        var childNodes = node.ChildNodes.ToList();
        node.RemoveAll();

        foreach (var childNode in childNodes.OrderBy(keySelector))
        {
            node.AppendChild(childNode);
        }

        if (attributes is not null)
        {
            foreach (XmlAttribute item in attributes)
            {
                node.Attributes!.Append(item);
            }
        }
    }

    private static void OrderChildNodesByNameAttribute(XmlNode xmlNode)
    {
        OrderChildNodes(xmlNode, m => m.Attributes?.GetNamedItem("name")?.Value ?? string.Empty);
    }

    private Dictionary<string, XmlNode> GetProcessableChildNodesMap(XmlNode xmlNode)
    {
        return SelectProcessableChildNodes(xmlNode).ToDictionary(m => $"{m.Name}:{m.Attributes!["name"]?.Value}", m => m, StringComparer.OrdinalIgnoreCase);
    }

    private Dictionary<string, XmlNode> SelectMembersMap(XmlDocument xmlDocument)
    {
        var members = SelectNodes(xmlDocument, "/doc/members/member");

        return members.ToDictionary(m => $"{m.Name}:{m.Attributes!["name"]?.Value}", m => m, StringComparer.OrdinalIgnoreCase);
    }

    private List<XmlNode> SelectNodes(XmlDocument xmlDocument, params string[] xpaths)
    {
        List<XmlNode> result = [];
        foreach (var xpath in xpaths)
        {
            if (xmlDocument.SelectNodes(xpath) is { } nodeList)
            {
                result.AddRange(nodeList.ToList());
            }
        }
        return result;
    }

    private List<XmlNode> SelectProcessableChildNodes(XmlNode xmlNode)
    {
        if (xmlNode is not XmlElement xmlElement)
        {
            return [];
        }
        List<XmlNode> result = [];

        Append(xmlElement.GetSummaryNodes());
        Append(xmlElement.GetTypeParamNodes());
        Append(xmlElement.GetParamNodes());
        Append(xmlElement.GetReturnsNodes());

        return result;

        void Append(XmlNodeList? xmlNodeList)
        {
            if (xmlNodeList is not null)
            {
                result.AddRange(xmlNodeList.ToList());
            }
        }
    }

    private async Task TranslateNodesAsync(string memberName, TranslateContext context, List<XmlNode> nodes, CancellationToken cancellationToken)
    {
        var parallelOptions = new ParallelOptions()
        {
            MaxDegreeOfParallelism = context.ParallelCount > 0 ? context.ParallelCount : 1,
            CancellationToken = cancellationToken,
        };

        await Parallel.ForEachAsync(nodes, parallelOptions, async (node, token) =>
        {
            var rawInnerXml = node.InnerXml;
            try
            {
                var translated = await _translator.TranslateText(memberName, rawInnerXml, context.TargetCultureInfo.Name);
                node.InnerXml = translated;
                var versionAttribute = node.OwnerDocument!.CreateAttribute("v");
                //var verison = GetContentVersion(rawInnerXml);
                //versionAttribute.Value = verison;
                //node.Attributes!.SetNamedItem(versionAttribute);

                var ignoreAttribute = node.OwnerDocument!.CreateAttribute("i");
                ignoreAttribute.Value = "0";
                node.Attributes!.SetNamedItem(ignoreAttribute);
                _logger.LogInformation("Translate IntelliSenseFile Content \"{Content}\" success \"{Translated}\".", rawInnerXml, translated);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Translate IntelliSenseFile Content \"{Content}\" fail.", rawInnerXml);
            }
        });
    }

    #endregion Private 方法
}
