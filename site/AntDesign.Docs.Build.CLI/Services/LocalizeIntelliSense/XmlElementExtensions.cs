// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using IntelliSenseLocalizer;

namespace System.Xml;

// This code is modified from the soures of https://github.com/stratosblue/IntelliSenseLocalizer

internal static class XmlElementExtensions
{
    public static XmlElement CreateParaNode(this XmlElement element, string? value = null)
    {
        var result = element.OwnerDocument.CreateElement("para");
        if (!string.IsNullOrEmpty(value))
        {
            result.Value = value;
        }
        return result;
    }

    public static Dictionary<string, XmlNode> CreateRefDictionary(this XmlElement element)
    {
        var result = new Dictionary<string, XmlNode>();

        AppendRefs(element, result, "see");
        AppendRefs(element, result, "paramref");
        AppendRefs(element, result, "typeparamref");
        AppendRefs(element, result, "c");

        return result;

        static void AppendRefs(XmlElement rootElement, Dictionary<string, XmlNode> result, string tagName)
        {
            if (rootElement.GetElementsByTagName(tagName) is XmlNodeList xmlNodeList)
            {
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    var item = (XmlElement)xmlNodeList[i]!;

                    var key = IntelliSenseNameUtil.TrimMemberPrefix(item.GetAttribute("cref"));
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        key = item.GetAttribute("langword");
                    }
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        key = item.GetAttribute("name");
                    }
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        key = item.InnerText.Trim();
                    }
                    key = IntelliSenseNameUtil.NormalizeOriginNameToUniqueKey(key);
                    result.TryAdd(key, item);
                }
            }
        }
    }

    public static XmlNodeList GetParamNodes(this XmlElement element)
    {
        return element.GetElementsByTagName("param");
    }

    public static XmlNodeList GetReturnsNodes(this XmlElement element)
    {
        return element.GetElementsByTagName("returns");
    }

    public static XmlNodeList GetSummaryNodes(this XmlElement element)
    {
        return element.GetElementsByTagName("summary");
    }

    public static XmlNodeList GetTypeParamNodes(this XmlElement element)
    {
        return element.GetElementsByTagName("typeparam");
    }

    public static XmlElement ImportAppendChild(this XmlElement element, XmlNode? node)
    {
        if (node is null)
        {
            return element;
        }
        var newNode = element.OwnerDocument.ImportNode(node, true);
        element.AppendChild(newNode);
        return element;
    }

    public static List<XmlNode> ToList(this XmlNodeList xmlNodeList)
    {
        var result = new List<XmlNode>();
        foreach (XmlNode item in xmlNodeList)
        {
            result.Add(item);
        }
        return result;
    }
}
