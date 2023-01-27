// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public class GoogleTranslationResponse
    {
        public IEnumerable<SentencesModel> Sentences { get; set; }

        public class SentencesModel
        {
            public string Trans { get; set; }
        }
    }
}
