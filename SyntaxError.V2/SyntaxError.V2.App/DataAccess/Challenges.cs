using Newtonsoft.Json;
using SyntaxError.V2.Modell.Challenges;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.DataAccess
{
    public class Challenges
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri challengesBaseUri = new Uri("http://localhost:51749/api/ChallengeBases");
        static readonly string[] challengeTypes =
            {
                "AudienceChallenge",
                "CrewChallenge",
                "MultipleChoiceChallenge",
                "MusicChallenge",
                "QuizChallenge",
                "ScreenshotChallenge",
                "SilhouetteChallenge",
                "SologameChallenge"
            };

        /// <summary>Creates the Challenge asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public async Task<ChallengeBase> CreateChallengeAsync(ChallengeBase param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            var type = param.GetType().Name;

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(new Uri(challengesBaseUri, "/api/ChallengeBases/?type=" + type), content);

            param.ChallengeID = int.Parse(result.Headers.Location.Segments.Last());

            return param;
        }

        /// <summary>Gets the challenges asynchronous.</summary>
        /// <returns></returns>
        public async Task<List<ChallengeBase>> GetChallengesAsync()
        {
            List<ChallengeBase> challenges = new List<ChallengeBase>();
            foreach (var type in challengeTypes)
            {
                HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=" + type));
                string json = await result.Content.ReadAsStringAsync();
                switch (type)
                {
                    case "AudienceChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<AudienceChallenge[]>(json));
                        break;
                    case "CrewChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<CrewChallenge[]>(json));
                        break;
                    case "MultipleChoiceChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<MultipleChoiceChallenge[]>(json));
                        break;
                    case "MusicChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<MusicChallenge[]>(json));
                        break;
                    case "QuizChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<QuizChallenge[]>(json));
                        break;
                    case "ScreenshotChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<ScreenshotChallenge[]>(json));
                        break;
                    case "SilhouetteChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<SilhouetteChallenge[]>(json));
                        break;
                    case "SologameChallenge":
                        if (result.IsSuccessStatusCode)
                            challenges.AddRange(JsonConvert.DeserializeObject<SologameChallenge[]>(json));
                        break;
                }
            }
            return challenges;
        }

        /// <summary>Edits the media object asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> EditChallengeAsync(ChallengeBase param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("Type", param.GetDiscriminator());
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(challengesBaseUri, "ChallengeBases/" + param.ChallengeID.ToString()), content);
            

            return result.IsSuccessStatusCode;
        }

        internal async Task<bool> DeleteChallengeAsync(ChallengeBase param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(challengesBaseUri, "ChallengeBases/" + param.ChallengeID.ToString()));
            return result.IsSuccessStatusCode;
        }
    }
}
