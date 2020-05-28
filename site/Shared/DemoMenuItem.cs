namespace AntDesign.Docs
{
    public class DemoMenuItem
    {
        public int Order { get; set; }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public DemoMenuItem[] Children { get; set; }
    }
}
