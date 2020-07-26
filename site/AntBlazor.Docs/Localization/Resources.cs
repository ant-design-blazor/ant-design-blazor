using System;
using System.Collections.Generic;
using System.Linq;

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
            _keyValues = languageContent.Split('\n').Select(line => line.Split(":")).ToList()
                .ToDictionary(x => x[0].ToLower(), x => x.Length == 2 ? x[1].Trim(' ', '\r') : "");
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
