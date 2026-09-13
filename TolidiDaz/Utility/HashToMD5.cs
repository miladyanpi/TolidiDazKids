using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Utility
{
    public class HashToMD5
    {
        /// <summary>
        /// هش کردن توسط MD5
        /// </summary>
        /// <param name="RawText">متن خام</param>
        /// <returns>رشته Base64</returns>
        public static string HashMD5(string RawText)
        {
            var HashProvider = System.Security.Cryptography.MD5.Create();
            byte[] HashArray = HashProvider.ComputeHash(Encoding.ASCII.GetBytes(RawText));
            HashProvider.Clear();
            StringBuilder SB = new StringBuilder();
            for (int i = 0; i < HashArray.Length; i++)
            {
                SB.Append(HashArray[i].ToString("X2"));
            }
            return SB.ToString();
        }
    }
}