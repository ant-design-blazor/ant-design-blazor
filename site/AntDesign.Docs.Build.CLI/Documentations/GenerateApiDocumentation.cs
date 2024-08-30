// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntDesign.Docs.Build.CLI.Services.Translation;

namespace AntDesign.Docs.Build.CLI.Documentations
{
    public class GenerateApiDocumentation
    {
        internal static DemoComponent ForComponent(
            string language,
            DocumentationAttribute docAttribute,
            string title,
            IDictionary<string, string> componentSummary,
            IDictionary<string, string> pageUrl,
            IDictionary<string, IEnumerable<IApiDocumentation>> apiDocs,
            IDictionary<string, IDictionary<string, IEnumerable<IApiDocumentation>>> seeAlso,
            Dictionary<string, string> faqs,
            IEnumerable<DemoInformation> demosInformation)
        {
            var allApiDocumentation = new StringBuilder();

            allApiDocumentation.AppendLine(ApiHeader(mainApi: true, title, pageUrl[language]));
            allApiDocumentation.Append(GenerateApiNonMethodDocumentation(apiDocs[language], language));
            allApiDocumentation.Append(GenerateApiMethodDocumentation(apiDocs[language], language));

            foreach (var seeAlsoMember in seeAlso)
            {
                allApiDocumentation.AppendLine(ApiHeader(mainApi: false, seeAlsoMember.Key, pageUrl[language]));

                var first = seeAlsoMember.Value[language].First();
                if (first is EnumDocumentation)
                {
                    allApiDocumentation.AppendLine($"<p>{StaticTextTranslation.Translated(StaticTextTranslation.UnderlyingType, language)}: {((EnumDocumentation)first).UnderlyingType}</p>");
                    allApiDocumentation.Append(GenerateEnumDocumentation(seeAlsoMember.Value[language].Select(x => (EnumDocumentation)x), language));
                }
                else
                {
                    allApiDocumentation.Append(GenerateApiNonMethodDocumentation(seeAlsoMember.Value[language], language));
                    allApiDocumentation.Append(GenerateApiMethodDocumentation(seeAlsoMember.Value[language], language));
                }
            }

            if (faqs[language] is not null)
            {
                allApiDocumentation.AppendLine($"<h2>{StaticTextTranslation.Translated(StaticTextTranslation.Faq, language)}</h2>");
                allApiDocumentation.AppendLine(faqs[language]);
            }

            var demos = demosInformation.Select(x => new DemoItem
            {
                Title = x.DescriptionContent.Meta.Title[language],
                Order = x.DescriptionContent.Meta.Order,
                Iframe = x.DescriptionContent.Meta.Iframe,
                Code = x.Code,
                Description = x.DescriptionContent.Descriptions[language] ?? default,
                Name = x.Type.Split('.').Last().Replace(".razor", ""),
                Style = x.DescriptionContent.Style,
                Debug = x.DescriptionContent.Meta.Debug,
                Docs = x.DescriptionContent.Meta.Docs,
                Type = x.Type
            }).ToList();

            return new DemoComponent()
            {
                Category = docAttribute.Category.ToString(),
                Title = title,
                SubTitle = docAttribute.SubTitle,
                Type = docAttribute.Type.ToString(),
                Desc = componentSummary[language],
                ApiDoc = allApiDocumentation.ToString(),
                Cols = docAttribute.Columns,
                Cover = docAttribute.CoverImageUrl,
                DemoList = demos
            };
        }

        private static string ApiHeader(bool mainApi, string title, string pageUrl)
        {
            var anchorLink = mainApi ? "API" : $"API-{title}";

            return $"<h2 id=\"{anchorLink}\"><span>{title} API</span><a href=\"{pageUrl}#{anchorLink}\" class=\"anchor\">#</a></h2>";
        }

        private static StringBuilder GenerateApiMethodDocumentation(IEnumerable<IApiDocumentation> apiDocumentation, string language)
        {
            var docs = new StringBuilder();
            var methodMembers = apiDocumentation.Where(x => x is MethodDocumentation);
            if (!methodMembers.Any())
            {
                return docs;
            }

            docs.AppendLine($@"<table class=""api methods-api"">
                <thead>
                    <tr>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.MethodSignatureHeader, language)}</th>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.ReturnTypeHeader, language)}</th>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.Description, language)}</th>
                    </tr>
                </thead>
                <tbody>");

            foreach (var member in methodMembers)
            {
                var castedMember = (MethodDocumentation)member;
                docs.AppendLine(@$"<tr>
                    <td class=""api-signature"">{castedMember.Signature}</td>
                    <td class=""api-data-type"">{castedMember.ReturnType}</td>
                    <td class=""api-description"">{castedMember.Summary}</td>
                </tr>");
            }

            docs.AppendLine("</tbody></table>");

            return docs;
        }

        private static StringBuilder GenerateApiNonMethodDocumentation(IEnumerable<IApiDocumentation> apiDocumentation, string language)
        {
            var docs = new StringBuilder();
            var nonMethodMembers = apiDocumentation.Where(x => x is PropertyDocumentation);
            if (!nonMethodMembers.Any())
            {
                return docs;
            }

            docs.AppendLine(@"<table class=""api properties-api"">");
            docs.AppendLine($@"<thead>
                    <tr>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.Property, language)}</th>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.Description, language)}</th>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.Type, language)}</th>
                        <th>{StaticTextTranslation.Translated(StaticTextTranslation.DefaultValue, language)}</th>
                    </tr>
                </thead>
                <tbody>");

            foreach (var member in nonMethodMembers)
            {
                var castedMember = (PropertyDocumentation)member;

                var obsoleteMessage = GetObsoleteMessage(castedMember.ObsoleteMessage, language);
                var parameterTag = castedMember.IsParameter
                    ? $@"<span class=""ant-tag"">{StaticTextTranslation.Translated(StaticTextTranslation.Parameter, language)}</span>"
                    : string.Empty;

                var rowClass = $"{(obsoleteMessage is null ? string.Empty : "obsolete")}";

                docs.AppendLine(@$"<tr class=""{rowClass}"">
                    <td class=""api-code-identifier"">{castedMember.Name}</td>
                    <td class=""api-description"">{parameterTag}{castedMember.Summary}{obsoleteMessage}</td>
                    <td class=""api-data-type"">{castedMember.Type}</td>
                    <td class=""api-default"">{GetDefault(castedMember.Default)}</td>
                </tr>");
            }

            docs.AppendLine("</tbody></table>");

            return docs;
        }

        private static StringBuilder GenerateEnumDocumentation(IEnumerable<EnumDocumentation> enumerable, string language)
        {
            var docs = new StringBuilder();
            docs.AppendLine(@"<table class=""api enum-api"">");
            docs.AppendLine($@"
                    <thead>
                        <tr>
                            <th>{StaticTextTranslation.Translated(StaticTextTranslation.Name, language)}</th>
                            <th>{StaticTextTranslation.Translated(StaticTextTranslation.Description, language)}</th>
                        </tr>
                    </thead>
                    <tbody>");

            foreach (var member in enumerable)
            {
                var obsoleteMessage = GetObsoleteMessage(member.ObsoleteMessage, language);
                var rowClass = $"{(obsoleteMessage is null ? string.Empty : "obsolete")}";

                docs.AppendLine(@$"<tr class=""{rowClass}"">
                    <td class=""api-code-identifier"">{member.Name}</td>
                    <td class=""api-description"">{member.Summary}{obsoleteMessage}</td>
                </tr>");
            }

            docs.AppendLine("</tbody></table>");

            return docs;
        }

        private static string GetDefault(string defaultValue)
        {
            return string.IsNullOrWhiteSpace(defaultValue)
                ? "--"
                : defaultValue;
        }

        private static string GetObsoleteMessage(string obsoleteMessage, string language)
        {
            return string.IsNullOrWhiteSpace(obsoleteMessage)
                ? null
                : $"{obsoleteMessage}<br /><br /><span class=\"ant-tag\">Obsolete</span>"
                    + $" {StaticTextTranslation.Translated(StaticTextTranslation.ObsoleteWillBeRemoved, language)}";
        }
    }
}
