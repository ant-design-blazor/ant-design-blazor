using System;
using System.Diagnostics;

namespace AntDesign.Docs.Build.CLI.Utils
{
    public class ShellProcess
    {
        private readonly object _lock = new object();
        private readonly Process _process;
        private bool _isError;
        private int _exitCode;

        public int ExitCode
        {
            get
            {
                lock (_lock)
                {
                    return _isError ? 1 : _exitCode;
                }
            }
        }

        public ShellProcess(string name, string argument)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = name,
                CreateNoWindow = true,
                ErrorDialog = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            if (!string.IsNullOrEmpty(argument))
            {
                processStartInfo.Arguments = argument;
            }

            _process = new Process { StartInfo = processStartInfo };
        }

        public void Exec(string command)
        {
            if (!_isError)
            {
                _process.StandardInput.WriteLine(command);
            }
        }

        public ShellProcess Start()
        {
            _process.Start();
            _process.OutputDataReceived += ProcessOnOutputDataReceived;
            _process.ErrorDataReceived += ProcessOnErrorDataReceived;
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            return this;
        }

        public void Close()
        {
            _process.StandardInput.WriteLine("exit");
            _process.WaitForExit();
            _exitCode = _process.ExitCode;
            _process.OutputDataReceived -= ProcessOnOutputDataReceived;
            _process.ErrorDataReceived -= ProcessOnErrorDataReceived;
            _process.Dispose();
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            if (e.Data.StartsWith("Cloning into"))
            {
                Console.WriteLine(e.Data);
                return;
            }

            ConsoleUtils.WriteLine(e.Data, ConsoleColor.Yellow);

            lock (_lock)
            {
                _isError = true;
            }
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
                Console.WriteLine(e.Data);
        }
    }
}