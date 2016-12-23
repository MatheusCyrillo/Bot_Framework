using ChatBot.Serialization;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services
{
    public class Luis
    {
        public static JsonObject GetResponse(string message)
        {
            using (var client = new HttpClient())
            {
                const string authKey = "03f03812f59c4a6aa5f57c8077abaaa3";

                var url = $"https://api.projectoxford.ai/luis/v2.0/apps/ae6f9d6f-63d5-4bf2-9b3f-d691f2ff13b8?subscription-key={authKey}&q={message}&verbose=true";

                var jsonString = new System.Net.WebClient().DownloadString(url);
                JsonObject informacaoJSON = JsonConvert.DeserializeObject<JsonObject>(jsonString);

                return informacaoJSON;
            }
        }
    }
}