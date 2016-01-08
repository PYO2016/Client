using System.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PYO2016_Client.Sources.HttpGetter
{
    class HttpGetter
    {
        public static string HttpDelete(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.Method = "DELETE";
            string result = null;
            try
            {

                using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(resp.GetResponseStream());
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
                return "BAD";
            }
            return result;
        }

        public static string HttpGet(string url)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return result;
        }

        public static string HttpPost(string url, string[] paramName, string[] paramVal)
        {
            HttpWebRequest req = WebRequest.Create(new Uri(url)) as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "application/json";

            StringBuilder paramz = new StringBuilder();
            paramz.Append("{");
            for (int i = 0; i < paramName.Length; i++)
            {
                paramz.Append(@"""");
                paramz.Append(paramName[i]);
                paramz.Append(@""":""");
                paramz.Append(paramVal[i]);
                if(i != paramName.Length - 1)
                    paramz.Append(@""",");
                else
                    paramz.Append(@"""");
            }
            paramz.Append("}");

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(paramz.ToString());
            req.ContentLength = formData.Length;

            // Send the request:
            try
            {
                using (Stream post = req.GetRequestStream())
                {
                    post.Write(formData, 0, formData.Length);
                }
            } catch (System.Net.WebException e)
            {
                throw e;
            }

            // Pick up the response:
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader =
                    new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string HttpFileUpload(string url, string fileName, string contentType, int contentLength, Stream inputStream)
        {
            HttpWebRequest req = WebRequest.Create(new Uri(url))
                                 as HttpWebRequest;
            req.Method = "POST";
            req.ContentType = "multipart/form-data";
            

            StringBuilder paramz = new StringBuilder();
            paramz.Append("{");
            paramz.Append(@"""FileName"":""");
            paramz.Append(fileName);
            paramz.Append(@""",""ContentType"":""");
            paramz.Append(contentType);
            paramz.Append(@""",""ContentLength"":");
            paramz.Append(contentLength);
            paramz.Append(@",""InputStream"":""");
            paramz.Append(inputStream);
            paramz.Append(@"""}");

            // Encode the parameters as form data:
            byte[] formData = UTF8Encoding.UTF8.GetBytes(paramz.ToString());
            req.ContentLength = formData.Length;

            // Send the request:
            using (Stream post = req.GetRequestStream())
            {
                post.Write(formData, 0, formData.Length);
            }

            // Pick up the response:
            string result = null;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
