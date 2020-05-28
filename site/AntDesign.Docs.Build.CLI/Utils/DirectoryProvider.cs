using System;
using System.IO;

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class DirectoryProvider
    {
        private readonly PlatformInformationArbiter _platformInformation;

        public string TmpDirectory => _platformInformation.GetValue(
            () => Path.Combine(Environment.GetEnvironmentVariable("USERPROFILE"), "AppData\\Local\\Temp"),
            () => "/tmp",
            () => "/tmp",
            () => "/tmp");

        public string DotnetDirectory => _platformInformation.GetValue(
            () => Path.Combine("C:\\Progra~1", "dotnet"),
            () => Path.Combine("/usr/local/share", "dotnet"),
            () => Path.Combine("/usr/local/share", "dotnet"),
            () => Path.Combine("/usr/local/share", "dotnet"));

        public string AgentPath => "skyapm.agent.aspnetcore";

        public string AdditonalDepsRootDirectory => Path.Combine(DotnetDirectory, "x64", "additionalDeps");

        public string StoreDirectory => Path.Combine(DotnetDirectory, "store");

        public DirectoryProvider(PlatformInformationArbiter platformInformation)
        {
            _platformInformation = platformInformation;
        }

        public string GetAdditonalDepsPath(string additonalName, string frameworkVersion)
        {
            return Path.Combine(GetAdditonalDepsDirectory(additonalName), "shared", "Microsoft.NETCore.App", frameworkVersion);
        }

        public string GetAdditonalDepsDirectory(string additonalName)
        {
            return Path.Combine(AdditonalDepsRootDirectory, additonalName);
        }
    }
}