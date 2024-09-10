// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using AntDesign.Docs.Build.CLI.Extensions;
using IntelliSenseLocalizer.ThirdParty;
using Microsoft.Extensions.CommandLineUtils;

namespace AntDesign.Docs.Build.CLI.Command
{
    public class GenerateIntelliSenseCommand : IAppCommand
    {
        public string Name => "intellisense";

        private LocalizeIntelliSenseTranslator _localizeIntelliSenseTranslator;

        public GenerateIntelliSenseCommand(LocalizeIntelliSenseTranslator localizeIntelliSenseTranslator)
        {
            _localizeIntelliSenseTranslator = localizeIntelliSenseTranslator;
        }

        public void Execute(CommandLineApplication command)
        {
            command.Description = "Generate intellisense xml files with multiple languages.";
            command.HelpOption();

            CommandArgument directoryArgument = command.Argument(
                "source", "[Required] The directory of docs files.");

            CommandArgument outputArgument = command.Argument(
                "output", "[Required] The directory where the json file to output");

            CommandArgument languageArgument = command.Argument(
                "language", "[Required] The language of intellisense xml files.");

            command.OnExecute(async () =>
            {
                string source = directoryArgument.Value;
                string output = outputArgument.Value;
                var languages = languageArgument.Value.Split(';');

                if (string.IsNullOrEmpty(source) || !File.Exists(source))
                {
                    Console.WriteLine("Invalid source.");
                    return 1;
                }

                if (string.IsNullOrEmpty(output))
                {
                    output = "./";
                }

                foreach (var language in languages)
                {
                    var context = new TranslateContext(source, output, CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo(language), false);
                    await _localizeIntelliSenseTranslator.TranslateAsync(context);
                }

                return 0;
            });
        }
    }
}
