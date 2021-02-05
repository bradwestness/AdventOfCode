using System;
using System.Security.Cryptography;
using System.Text;

namespace advent._2015
{
    public class _04 : IAnswer
    {
        public void Run()
        {
            const string startsWith = "000000";
            long i = 0;
            string hash = GetMD5Hash(i);

            while (!hash.StartsWith(startsWith))
            {
                // keep going
                hash = GetMD5Hash(++i);
            }

            Console.WriteLine($"First integer with a hash that starts with {startsWith}: {i}");
        }

        private static Lazy<MD5> _md5 = new Lazy<MD5>(() => MD5.Create());

        private static Lazy<StringBuilder> _sb = new Lazy<StringBuilder>(() => new());

        private static string GetMD5Hash(long number)
        {
            var bytes = _md5.Value.ComputeHash(Encoding.UTF8.GetBytes($"{INPUT}{number}"));
            _sb.Value.Clear();

            for (var i = 0; i < bytes.Length; i++)
            {
                _sb.Value.Append(bytes[i].ToString("x2"));
            }

            return _sb.Value.ToString();
        }

        private const string INPUT = @"iwrupvqb";
    }
}
