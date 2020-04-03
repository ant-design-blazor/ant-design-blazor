namespace AntBlazor.Docs.Build.Utils
{
    public class ShellProcessFactory
    {
        public ShellProcess Create(string name, string argument = null)
        {
            return new ShellProcess(name, argument).Start();
        }

        public int Release(ShellProcess process)
        {
            process.Close();
            return process.ExitCode;
        }
    }
}