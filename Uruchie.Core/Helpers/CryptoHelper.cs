using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Uruchie.Core.Helpers
{
    public static class CryptoHelper
    {
        public static string CalculateMd5(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)       
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }
    }
}
