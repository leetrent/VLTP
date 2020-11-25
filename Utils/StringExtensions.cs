using System;
using System.Text;
using System.Web;
using System.IO;
using System.IO.Compression;

namespace VLTP.Utils
{
    public static class StringExtensions
    {
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        public static string UpperCaseUrlEncode(this string value)
        {
            var result = new StringBuilder(value);
            for (var i = 0; i < result.Length; i++)
                if (result[i] == '%')
                {
                    result[++i] = char.ToUpper(result[i]);
                    result[++i] = char.ToUpper(result[i]);
                }
            return result.ToString();
        }

        public static string DeflateEncode(this string value)
        {
            var memoryStream = new MemoryStream();
            using (var writer = new StreamWriter(new DeflateStream(memoryStream, CompressionMode.Compress, true),
                new UTF8Encoding(false)))
            {
                writer.Write(value);
                writer.Close();
                return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length,
                    Base64FormattingOptions.None);
            }
        }
    }
}
