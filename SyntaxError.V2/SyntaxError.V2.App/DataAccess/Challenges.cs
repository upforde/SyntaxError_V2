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
                        var AC = JsonConvert.DeserializeObject<AudienceChallenge[]>(json);
                        challenges.AddRange(AC);
                        break;
                    case "CrewChallenge":
                        var CC = JsonConvert.DeserializeObject<CrewChallenge[]>(json);
                        challenges.AddRange(CC);
                        break;
                    case "MultipleChoiceChallenge":
                        var MCC = JsonConvert.DeserializeObject<MultipleChoiceChallenge[]>(json);
                        challenges.AddRange(MCC);
                        break;
                    case "MusicChallenge":
                        var MC = JsonConvert.DeserializeObject<MusicChallenge[]>(json);
                        challenges.AddRange(MC);
                        break;
                    case "QuizChallenge":
                        var QC = JsonConvert.DeserializeObject<QuizChallenge[]>(json);
                        challenges.AddRange(QC);
                        break;
                    case "ScreenshotChallenge":
                        var SC = JsonConvert.DeserializeObject<ScreenshotChallenge[]>(json);
                        challenges.AddRange(SC);
                        break;
                    case "SilhouetteChallenge":
                        var SLC = JsonConvert.DeserializeObject<SilhouetteChallenge[]>(json);
                        challenges.AddRange(SLC);
                        break;
                    case "SologameChallenge":
                        var SGC = JsonConvert.DeserializeObject<SologameChallenge[]>(json);
                        challenges.AddRange(SGC);
                        break;
                }
            }
            return challenges;
        }

        internal async Task<bool> DeleteChallengeAsync(ChallengeBase param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(challengesBaseUri, "ChallengeBases/" + param.ChallengeID.ToString()));
            return result.IsSuccessStatusCode;
        }
    }
}
