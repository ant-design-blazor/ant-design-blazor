using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace AntDesign.Docs.Localization
{
    public class Resources
    {
        private Dictionary<string, string> _keyValues = null;

        public Resources(string languageContent)
        {
            Initialize(languageContent);
        }

        private void Initialize(string languageContent)
        {
            _keyValues = new Deserializer().Deserialize<Dictionary<string, string>>(languageContent).Select(k => new { Key = k.Key.ToLower(), Value = k.Value }).ToDictionary(t => t.Key, t => t.Value);
        }

        public string this[string key]
        {
            get
            {
                try
                {
                    return _keyValues[key.ToLower()];
                }
                catch
                {
                    return key;
                }
            }
        }
    }
}
