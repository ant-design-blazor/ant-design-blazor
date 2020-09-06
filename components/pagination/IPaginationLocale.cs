namespace AntDesign
{
    public interface IPaginationLocale
    {
        public string ItemsPerPage { get; }

        public string JumpTo { get; }

        public string JumpToConfirm { get; }

        public string Page { get; }

        public string PrevPage { get; }

        public string NextPage { get; }

        public string Prev5 { get; }

        public string Next5 { get; }

        public string Prev3 { get; }

        public string Next3 { get; }
    }
}
