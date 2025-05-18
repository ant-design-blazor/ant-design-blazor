// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public static class StaticTextTranslation
    {
        public const string MethodSignatureHeader = "Method Signature";

        public const string ReturnTypeHeader = "Return Type";

        public const string Description = "Description";

        public const string ObsoleteWillBeRemoved = "Will be removed in a future version.";

        public const string Property = "Property";

        public const string Type = "Type";

        public const string DefaultValue = "Default Value";

        public const string Name = "Name";

        public const string Parameter = "Parameter";

        public const string UnderlyingType = "Underlying Type";

        public const string Faq = "FAQs";

        public const string Version = "Version";

        private static readonly Dictionary<string, Dictionary<string, string>> _translations = new()
        {
            {
                Constants.ChineseLanguage,
                new Dictionary<string, string>
                {
                    { MethodSignatureHeader, "方法签名" },
                    { ReturnTypeHeader, "返回类型" },
                    { Description, "描述" },
                    { ObsoleteWillBeRemoved, "将在未来的版本中删除。" },
                    { Property, "属性" },
                    { Type, "类型" },
                    { DefaultValue, "默认值" },
                    { Name, "名称" },
                    { Parameter, "属性" },
                    { UnderlyingType, "基础类型" },
                    { Faq, "常问问题" },
                    { Version , "版本" }
                }
            }
        };

        public static string Translated(string text, string language)
        {
            if (language == Constants.EnglishLanguage)
            {
                return text;
            }

            return _translations[language][text];
        }
    }
}
