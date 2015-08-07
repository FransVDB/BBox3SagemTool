using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace BBox3Tool.utils
{
    public class NetworkUtils
    {
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

        /// <summary>
        /// Auto detect type of modem
        /// </summary>
        /// <param name="ipAddress">IP address of the modem</param>
        /// <returns>Type of device if recognised, or unknown</returns>
        public static Device detectDevice(string ipAddress)
        {
            try
            {
                Uri host = new Uri("http://" + ipAddress);
                Uri uriBbox3S = new Uri(host, Path.Combine("cgi", "json-req"));
                Uri uriBbox3T = new Uri(host, "login.lua");
                Uri uriBbox3T2 = new Uri(host, "login.lp");
                Uri uriBbox2 = new Uri(host, "index.cgi");

                if (detectDeviceGetStatusCode(uriBbox3S) == 200)
                    return Device.BBOX3S;
                else if (detectDeviceGetStatusCode(uriBbox3T) == 200 || detectDeviceGetStatusCode(uriBbox3T2) == 200)
                    return Device.BBOX3T;
                else if (detectDeviceGetStatusCode(uriBbox2) == 200)
                    return Device.BBOX2;
                else
                    return Device.unknown;
            }
            catch(Exception ex)
            {
                Debugger.Log(0, "error", ex.ToString());
                return Device.unknown;
            }
        }

        /// <summary>
        /// Get http status code for an url
        /// </summary>
        /// <param name="url">Url to get http status code form</param>
        /// <returns>Http status code</returns>
        private static int detectDeviceGetStatusCode(Uri url)
        {
            int status = 0;
            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.AllowAutoRedirect = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                status = (int)response.StatusCode;
                response.Close();
            }
            catch (WebException ex)
            {
                status = (int)((HttpWebResponse)ex.Response).StatusCode;
                if (status == 400 && url.ToString().EndsWith("json-req")) //bad request bbox3/s
                    status = 200;
            }
            catch (Exception)
            {
                status = 0;
            }
            return status;
        }

    }
}