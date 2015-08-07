using System;
using System.Security.Cryptography;
using System.Text;

namespace BBox3Tool.utils
{
    public class SagemUtils
    {
        /// <summary>
        /// Calculate md5 hash of a string
        /// </summary>
        /// <param name="input">string to be hashed</param>
        /// <returns>MD5 hash as 32-length string</returns>
        public static string Md5(string input)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        /// <summary>
        /// Generate random number for request 'cnonce'
        /// </summary>
        /// <returns>random unsigned int</returns>
        public static string getLocalNonce()
        {
            return ((uint) new Random(DateTime.Now.Millisecond).Next(Int32.MaxValue)).ToString();
        }

        /// <summary>
        /// Calc Ha1 hash for json request
        /// </summary>
        /// <param name="user">Current user's username</param>
        /// <param name="password">Current user's password</param>
        /// <param name="nonce">random value for each request</param>
        /// <returns>Calculated Ha1 request value</returns>
        public static string calcHa1(string user, string password, string nonce)
        {
           return Md5(user + ":" + nonce.ToString() + ":" + Md5(password));
        }

        /// <summary>
        /// Calc Ha1 hash for cookie
        /// </summary>
        /// <param name="user">Current user's username</param>
        /// <param name="password">Current user's password</param>
        /// <param name="nonce">random value for each request</param>
        /// <returns>Calculated Ha1 cookie value</returns>
        public static string calcHa1Cookie(string user, string password, string nonce)
        {
            string ha1 = calcHa1(user, password, nonce);
            return ha1.Substring(0,10) + Md5(password) + ha1.Substring(10);
        }

        /// <summary>
        /// Calculate Authkey for json request
        /// </summary>
        /// <param name="user">Current user's usernam</param>
        /// <param name="password">Current user's password</param>
        /// <param name="requestId">Session's request ID</param>
        /// <param name="nonce">Session's nonce</param>
        /// <param name="cNonce">Session's cnonce</param>
        /// <returns>Calculated auth key</returns>
        public static string calcAuthKey(string user, string password, int requestId, string nonce, string cNonce)
        {
            string ha1 = calcHa1(user, password, nonce);
            return Md5(ha1 + ":" + requestId.ToString() + ":" + cNonce + ":JSON:/cgi/json-req");
        }

    }
}
