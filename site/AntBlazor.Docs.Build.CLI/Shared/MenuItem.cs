namespace AntBlazor.Docs.Build.CLI.Shared
{
    public class MenuItem
    {
        public int Order { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public MenuItem[] Children { get; set; }
    }
}