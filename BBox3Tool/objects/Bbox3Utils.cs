using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace BBox3Tool
{
    public class Bbox3Utils
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

        /// <summary>
        /// Send http request to BBox
        /// </summary>
        /// <param name="url">Url to send request to</param>
        /// <param name="cookies">cookies to send with request (optional)</param>
        /// <param name="data">Data to send to Bbox (post or get data)</param>
        /// <param name="mode">Request mode: post or get</param>
        /// <returns>Reply as string</returns>
        public static string sendRequest(Uri url, CookieCollection cookies = null, Dictionary<string, string> data = null, WebRequestMode mode = WebRequestMode.Get)
        {
            try
            {
                HttpWebRequest request = null;

                //set data
                if (data != null)
                
                {
                    string dataStr = String.Join("&", data.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value)));        
                    switch (mode)
                    {
                        case WebRequestMode.Get:
                            url = new Uri(url.ToString() + "?" + dataStr);
                            request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = "GET";
                            request.Host = url.Host;
                            break;
                        case WebRequestMode.Post:
                            request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = "POST";
                            request.Host = url.Host;

                            //request.Referer = "http://192.168.1.1/2.5.7/gui/";

                            //thank you stackoverflow!
                            //http://stackoverflow.com/questions/566437/http-post-returns-the-error-417-expectation-failed-c
                            ServicePointManager.Expect100Continue = false;
                            request.ServicePoint.Expect100Continue = false;

                            // add post data to request
                            byte[] postBytes = Encoding.UTF8.GetBytes(dataStr);
                            request.ContentLength = postBytes.Length;
                            Stream postStream = request.GetRequestStream();
                            postStream.Write(postBytes, 0, postBytes.Length);
                            postStream.Close();

                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                    request = WebRequest.Create(url) as HttpWebRequest;

                //set headers, fake real browser the best we can
                request.KeepAlive = true;
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.Headers.Add("Accept-Language", "nl,en-US;q=0.7,en;q=0.3");
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
                request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Cache-control", "no-cache");
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko"; //fake IE11

                //set cookies
                if (cookies != null)
                {
                    request.CookieContainer = new CookieContainer();
                    request.CookieContainer.Add(cookies);
                }

                //make request and get response
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();
                response.Close();
                return responseString;
            }
            catch (Exception ex)
            {
                Debugger.Log(0, "error", ex.ToString());
                return "";
            }
        }
    }
}
