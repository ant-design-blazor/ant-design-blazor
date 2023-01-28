// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Threading.Tasks;

namespace AntDesign.Docs.Build.CLI.Services.Translation
{
    public interface ITranslate
    {
        Task<string?> TranslateText(string text, string to, string from = "auto");

        Task BackupTranslations(bool onlyKeepUsed = true);
    }
}
