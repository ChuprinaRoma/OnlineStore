using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public class ConnectorShope
    {
        private static HttpClient httpClient = null;
        private readonly string url          = null;
        private readonly string pref         = null;
        private readonly string typeReqvest  = null;


        public ConnectorShope(string url, string pref, string typeReqvest)
        {
            this.typeReqvest = typeReqvest;
            this.url         = url;
            this.pref        = pref;
            httpClient       = new HttpClient();
        }

        public async Task<string> GetContent(string idPage)
        {
            string sourse       = null;
            byte[] sourseInByte = null;
            string uri          = null;
            
            uri                 = $"{url}/{pref}{idPage}";
           
            var response        = await httpClient.GetAsync(uri);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                sourseInByte = await response.Content.ReadAsByteArrayAsync();
                sourse       = Encoding.Default.GetString(sourseInByte, 0, sourseInByte.Length - 1);
            }

            return sourse;
        }

        public static async Task<string> GetContentSimplePage(string url)
        {
            string sourse       = null;
            byte[] sourseInByte = null;

            var response        = await httpClient.GetAsync($"{url}");

            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                sourseInByte = await response.Content.ReadAsByteArrayAsync();
                sourse       = Encoding.Default.GetString(sourseInByte, 0, sourseInByte.Length - 1);
            }

            return sourse;
        }
    }
}
