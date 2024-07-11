// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    public record TokenHash(string TokenKey, string HashId);

    public interface IToken
    {
        Dictionary<string, object> GetTokens();
        void Merge(IToken token);
        void Merge(params IToken[] tokens);
        TokenHash GetTokenHash(string version = "5.11.4", bool hashed = true);
    }

    public partial class GlobalToken : IToken
    {
        protected readonly Dictionary<string, object> _tokens = new();
        private TokenHash _tokenHash;
#if DEBUG
        private const string HashPrefix = "css-dev-only-do-not-override";
#else
        private const string HashPrefix = "css";
#endif

        public object this[string key]
        {
            get => _tokens[key];
            set => _tokens[key] = value;
        }

        public GlobalToken() { }

        public Dictionary<string, object> GetTokens()
        {
            return _tokens;
        }

        public void Merge(IToken source)
        {
            var tokens = source.GetTokens();
            foreach (var token in tokens)
            {
                _tokens[token.Key] = token.Value;
            }
        }

        public void Merge(params IToken[] tokens)
        {
            foreach (var token in tokens)
            {
                Merge(token);
            }
        }

        public TokenHash GetTokenHash(string version = "5.11.4", bool hashed = true)
        {
            if (_tokenHash == null)
            {
                var hashFlag = hashed ? "true" : "";
                var salt = $"{version}-{hashFlag}";
                var tokenKey = TokenToKey(_tokens, salt);
                // var hashId = $"{HashPrefix}-{Hash(tokenKey)}";
                var hashId = $"{HashPrefix}-3nv711";
                _tokenHash = new TokenHash(tokenKey, hashId);
            }

            return _tokenHash;
        }

        public string GetHashId()
        {
            return GetTokenHash().HashId;
        }

        public string TokenToKey(Dictionary<string, object> token, string salt)
        {
            return Hash($"{salt}_{FlattenToken(token)}");
        }

        private string FlattenToken(Dictionary<string, object> token)
        {
            var sb = new StringBuilder();
            foreach (var item in token)
            {
                sb.Append(item.Key);
                if (item.Value is bool)
                {
                    sb.Append(item.Value.ToString().ToLower());
                }
                else
                {
                    sb.Append(item.Value);
                }
            }
            return sb.ToString();
        }

        private string Hash(string str)
        {
            long h = 0;
            int k = 0;
            var i = 0;
            var len = str.Length;
            for (; len >= 4; ++i, len -= 4)
            {
                k = (str.CharCodeAt(i) & 0xff) |
                    ((str.CharCodeAt(++i) & 0xff) << 8) |
                    ((str.CharCodeAt(++i) & 0xff) << 16) |
                    ((str.CharCodeAt(++i) & 0xff) << 24);

                k = (k & 0xffff) * 0x5bd1e995 + (((k >>> 16) * 0xe995) << 16);
                k ^= k >>> 24;
                h = ((k & 0xffff) * 0x5bd1e995 + (((k >>> 16) * 0xe995) << 16)) ^
                    ((h & 0xffff) * 0x5bd1e995 + (((h >>> 16) * 0xe995) << 16));
            }
            switch (len)
            {
                case 3:
                    h ^= (str.CharCodeAt(i + 2) & 0xff) << 16;
                    goto case 2;
                case 2:
                    h ^= (str.CharCodeAt(i + 1) & 0xff) << 8;
                    goto case 1;
                case 1:
                    h ^= str.CharCodeAt(i) & 0xff;
                    h = ((h & 0xffff) * 0x5bd1e995) + (((int)(h >>> 16) * 0xe995) << 16);
                    break;
            }
            h ^= (int)h >>> 13;
            h = ((h & 0xffff) * 0x5bd1e995) + (((int)(h >>> 16) * 0xe995) << 16);
            var val = ((int)(h ^ ((int)h >>> 15)));
            return Convert.ToInt64(Convert.ToString((val >>> 0), toBase: 2), 2).ToString(36);
        }
    }

    public static class StringEntensions
    {
        public static int CharCodeAt(this string str, int index)
        {
            return str[index];
        }
    }

    public static class IntEntensions
    {
        private const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";

        public static string ToString(this long value, int radix = 36)
        {
            var clistarr = CharList.ToCharArray();
            var result = new Stack<char>();
            while (value != 0)
            {
                result.Push(clistarr[value % radix]);
                value /= radix;
            }
            return new string(result.ToArray());
        }
    }
}
