using System;
using System.Security.Cryptography;
using System.Text;

namespace advent._2015
{
    public class _04 : IAnswer
    {
        public void Run()
        {
            var i = 0;
            var hash = GetIntegerMD5Hash(i);

            while (!hash.StartsWith("000000"))
            {
                // keep going
                hash = GetIntegerMD5Hash(++i);
            }

            Console.WriteLine($"First integer with a hash that starts with five zeros: {i}");
        }

        private static Lazy<MD5> _md5 = new Lazy<MD5>(() => MD5.Create());

        private static Lazy<StringBuilder> _sb = new Lazy<StringBuilder>(() => new());

        private static string GetIntegerMD5Hash(int integer)
        {
            var bytes = _md5.Value.ComputeHash(Encoding.UTF8.GetBytes($"{INPUT}{integer}"));
            _sb.Value.Clear();

            for (int i = 0; i < bytes.Length; i++)
            {
                _sb.Value.Append(bytes[i].ToString("x2"));
            }

            return _sb.Value.ToString();
        }

        private const string INPUT = @"iwrupvqb";
    }
}
