using System;
using System.Security.Cryptography;
using System.Text;

namespace advent
{
    public static class StringExtensions
    {
        public static string GetMD5Hash(this string str, string prefix = null, string suffix = null)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                prefix = string.Empty;
            }

            if (string.IsNullOrEmpty(suffix))
            {
                suffix = string.Empty;
            }

            var bytes = _md5.Value.ComputeHash(Encoding.UTF8.GetBytes($"{prefix}{str}{suffix}"));
            _sb.Value.Clear();

            for (var i = 0; i < bytes.Length; i++)
            {
                _sb.Value.Append(bytes[i].ToString("x2"));
            }

            return _sb.Value.ToString();
        }

        public static string ToHexString(this string str, Encoding encoding = null) =>
            BitConverter.ToString((encoding ?? Encoding.Default).GetBytes(str ?? string.Empty)).Replace("-", "");

        private static Lazy<MD5> _md5 = new Lazy<MD5>(() => MD5.Create());

        private static Lazy<StringBuilder> _sb = new Lazy<StringBuilder>(() => new());
    }
}