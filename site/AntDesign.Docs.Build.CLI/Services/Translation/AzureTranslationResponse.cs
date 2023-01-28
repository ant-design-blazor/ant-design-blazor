// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class AzureTranslationResponse
    {
        public IEnumerable<Translation> Translations { get; set; }

        public class Translation
        {
            public string To { get; set; }

            public string Text { get; set; }
        }
    }
}
