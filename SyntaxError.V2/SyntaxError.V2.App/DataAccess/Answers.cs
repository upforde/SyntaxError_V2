using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.DataAccess
{
    public class Answers
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri answersBaseUri = new Uri("http://localhost:51749/api/Answers/");

        public async Task<Modell.ChallengeObjects.Answers> CreateAnswersAsync(Modell.ChallengeObjects.Answers param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(answersBaseUri, content);

            param.AnswersID = int.Parse(result.Headers.Location.Segments.Last());

            return param;
        }

        public async Task<Modell.ChallengeObjects.Answers> GetAnswersAsync(int? id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(answersBaseUri, "" + id));
            string json = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Modell.ChallengeObjects.Answers>(json);
        }

        internal async Task<bool> EditAnswersAsync(Modell.ChallengeObjects.Answers param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));
            
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(answersBaseUri, param.AnswersID.ToString()), content);

            return result.IsSuccessStatusCode;
        }
    }
}
