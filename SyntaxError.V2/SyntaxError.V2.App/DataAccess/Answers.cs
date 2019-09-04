using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.DataAccess
{
    public class Answers
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri answersBaseUri = new Uri("http://localhost:51749/api/Answers/");

        public async Task<Modell.ChallengeObjects.Answers> GetAnswersAsync(int? id)
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(answersBaseUri, "" + id));
            string json = await result.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Modell.ChallengeObjects.Answers>(json);
        }
    }
}
