using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;

namespace AntBlazor.Docs.Localization
{
    public class Resources
    {
        private Dictionary<string, string> keyValues = null;

        public Resources(string languageContent)
        {
            Initialize(languageContent);
        }

        private void Initialize(string languageContent)
        {
            keyValues = new Deserializer().Deserialize<Dictionary<string, string>>(languageContent).Select(k => new { Key = k.Key.ToLower(), Value = k.Value }).ToDictionary(t => t.Key, t => t.Value);
        }

        public string this[string key]
        {
            get
            {
                try
                {
                    return keyValues[key.ToLower()];
                }
                catch
                {
                    return key;
                }
            }
        }
    }
}