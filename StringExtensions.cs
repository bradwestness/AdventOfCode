using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace advent
{
    public static class StringExtensions
    {
        public static IEnumerable<string> ToLines(this string input)
        {
            using (var sr = new StringReader(input))
            {
                while (sr.Peek() != -1)
                {
                    yield return sr.ReadLine();
                }
            }
        }

        public static string GetMD5Hash(this string input, string prefix = null)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = string.Empty;
            }

            var bytes = _md5.Value.ComputeHash(Encoding.UTF8.GetBytes($"{prefix}{input}"));
            _sb.Value.Clear();

            for (var i = 0; i < bytes.Length; i++)
            {
                _sb.Value.Append(bytes[i].ToString("x2"));
            }

            return _sb.Value.ToString();
        }

        private static Lazy<MD5> _md5 = new Lazy<MD5>(() => MD5.Create());

        private static Lazy<StringBuilder> _sb = new Lazy<StringBuilder>(() => new());
    }
}