using Microsoft.AspNetCore.Components;

namespace AntDesign
{
    public class ForwardRef
    {
        private ElementReference _current;
        private string _id;

        public ElementReference Current
        {
            get => _current;
            set => Set(value);
        }


        public void Set(ElementReference value)
        {
            _current = value;
        }

        public string Id => _id;

        public void SetId(string id)
        {
            _id = id;
        }
    }
}
