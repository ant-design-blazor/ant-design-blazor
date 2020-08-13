using System;
using System.Collections.Generic;

namespace AntDesign.Docs.Localization
{
    public class Resources
    {
        private IDictionary<string, string> _keyValues = null;

        public Resources(string languageContent)
        {
            Initialize(languageContent);
        }

        private void Initialize(string languageContent)
        {
            _keyValues = System.Text.Json.JsonSerializer.Deserialize<IDictionary<string, string>>(languageContent);
        }

        public string this[string key]
        {
            get
            {
                try
                {
                    return _keyValues[key.ToLower()];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return key;
                }
            }
        }
    }
}
