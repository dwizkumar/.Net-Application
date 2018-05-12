using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RestUtility
{
    public class REST
    {
        private string baseUri;
        public REST(string bU)
        {
            this.baseUri = bU;
        }

        #region GetRest
        public string getRestData(string uri)
        {
            string baseUri = "http://ist.rit.edu/api";
            //connect to the api
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(baseUri + uri);
            try
            {
                WebResponse resp = req.GetResponse();
                using (Stream respStream = resp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                WebResponse err = ex.Response;
                using (Stream respStream = err.GetResponseStream())
                {
                    StreamReader r = new StreamReader(respStream, Encoding.UTF8);
                    string errorText = r.ReadToEnd();
                    //log it
                }
                throw;
            }
        }
        #endregion
    }
}
