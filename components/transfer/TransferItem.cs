using System.Collections.Generic;

namespace AntDesign
{
    public record struct TransferItem
    {
        public string Key { get; set; } = "";

        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool Disabled { get; set; } = false;

        private readonly IDictionary<string, object> _properties = new Dictionary<string, object>();

        public TransferItem()
        {
        }

        public object this[string index]
        {
            get
            {
                return _properties[index];
            }
            set
            {
                _properties[index] = value;
            }
        }
    }
}
