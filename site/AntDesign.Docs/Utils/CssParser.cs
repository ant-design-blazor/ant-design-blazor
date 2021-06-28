using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AntDesign.Docs.Utils
{
    public class CSSParser : List<KeyValuePair<string, List<KeyValuePair<string, string>>>>
    {
        private const string SelectorKey = "selector";
        private const string NameKey = "name";
        private const string ValueKey = "value";
        public const string CssGroups = @"(?<selector>(?:(?:[^,{]+),?)*?)\{(?:(?<name>[^}:]+):?(?<value>[^};]+);?)*?\}";
        public const string CssComments = @"(?<!"")\/\*.+?\*\/(?!"")";

        [NonSerialized]
        private readonly Regex rStyles = new Regex(CssGroups, RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private string stylesheet = string.Empty;
        private Dictionary<string, Dictionary<string, string>> classes;
        private Dictionary<string, Dictionary<string, string>> elements;

        public string StyleSheet
        {
            get => this.stylesheet;
            set
            {
                this.stylesheet = value;
                this.Clear();
            }
        }

        public CSSParser()
        {
            this.StyleSheet = string.Empty;
        }

        public CSSParser(string cascadingStyleSheet)
        {
            this.Read(cascadingStyleSheet);
        }

        public void ReadCSSFile(string path)
        {
            this.StyleSheet = File.ReadAllText(path);
            this.Read(StyleSheet);
        }

        public void Read(string cascadingStyleSheet)
        {
            this.StyleSheet = cascadingStyleSheet;

            if (string.IsNullOrEmpty(cascadingStyleSheet))
                return;

            MatchCollection matchList = rStyles.Matches(Regex.Replace(cascadingStyleSheet, CssComments, string.Empty));
            foreach (Match item in matchList)
            {
                if (item?.Groups[SelectorKey]?.Captures?[0] != null && !string.IsNullOrEmpty(item.Groups[SelectorKey].Value))
                {
                    string strSelector = item.Groups[SelectorKey].Captures[0].Value.Trim();
                    var style = new List<KeyValuePair<string, string>>();

                    for (int i = 0; i < item.Groups[NameKey].Captures.Count; i++)
                    {
                        string className = item.Groups[NameKey].Captures[i].Value;
                        string value = item.Groups[ValueKey].Captures[i].Value;

                        if (string.IsNullOrEmpty(className) || string.IsNullOrEmpty(value))
                            continue;

                        className = className.TrimWhiteSpace();
                        value = value.TrimWhiteSpace();

                        if (!string.IsNullOrEmpty(className) && !string.IsNullOrEmpty(value))
                        {
                            style.Add(new KeyValuePair<string, string>(className, value));
                        }
                    }

                    this.Add(new KeyValuePair<string, List<KeyValuePair<string, string>>>(strSelector, style));
                }
            }
        }

        public Dictionary<string, Dictionary<string, string>> Classes
        {
            get
            {
                if (classes == null || classes.Count == 0)
                {
                    this.classes = this.Where(cl => cl.Key.StartsWith(".")).ToDictionary(cl => cl.Key.Trim(new Char[] { '.' }), cl => cl.Value.ToDictionary(p => p.Key, p => p.Value));
                }

                return classes;
            }
        }

        public Dictionary<string, Dictionary<string, string>> Elements
        {
            get
            {
                if (elements == null || elements.Count == 0)
                {
                    elements = this.Where(el => !el.Key.StartsWith(".")).ToDictionary(el => el.Key, el => el.Value.ToDictionary(p => p.Key, p => p.Value));
                }
                return elements;
            }
        }

        public IEnumerable<KeyValuePair<string, List<KeyValuePair<string, string>>>> Styles => this.ToArray();

        public new void Clear()
        {
            base.Clear();
            this.classes = null;
            this.elements = null;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.StyleSheet.Length);
            foreach (var item in this)
            {
                sb.Append(item.Key).Append("{");
                foreach (var property in item.Value)
                {
                    sb.Append(property.Key).Append(":").Append(property.Value).Append(";");
                }
                sb.Append("}");
            }

            return sb.ToString();
        }

        public string AddScopeIdToString(string scopeId)
        {
            if (string.IsNullOrEmpty(scopeId))
                return this.ToString();

            StringBuilder sb = new StringBuilder(this.StyleSheet.Length);
            foreach (var item in this)
            {
                if (item.Key.IndexOf($"#{scopeId}") >= 0)
                {
                    sb.Append($"{item.Key} {{");
                }
                else
                {
                    sb.Append($"#{scopeId} {item.Key} {{");
                }
                foreach (var property in item.Value)
                {
                    sb.Append($"{property.Key}:{property.Value};");
                }
                sb.Append("}");
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StyleSheet);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            CSSParser o = obj as CSSParser;
            return this.StyleSheet.Equals(o.StyleSheet);
        }
    }
}
