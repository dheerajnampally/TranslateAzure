using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace TranslateAzure
{
    class Program
    {
        //This key is no longer valid, generate a new key and replace it.
        private const string key = "e4861c05fa98476fa74d88a8da0612ef";

        public static async Task Main()
        {
            var text = Console.ReadLine();
            var lang = Console.ReadLine();
            await Translate(text, lang);
        }

        /*
         * Translate method which uses Azure Cognitive services 
         */
        public static async Task<string> Translate(string text, string language)
        {
            //_ = WebUtility.UrlEncode(text);
            string texttotranslate = text;
            string uri = $"https://api.cognitive.microsofttranslator.com//translate?api-version=3.0&from=en&to={language}";

            System.Object[] body = new System.Object[] { new { Text = texttotranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using var client = new HttpClient();
            using var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.RequestUri = new Uri(uri);
            request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            request.Headers.Add("Ocp-Apim-Subscription-Key", key);
            request.Headers.Add("Ocp-Apim-Subscription-Region", "francecentral");
            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<IList<Dictionary<string, IList<IDictionary<string, string>>>>>(responseBody);
            var translation = result[0]["translations"][0]["text"];

            return translation;
        }
    }
}
