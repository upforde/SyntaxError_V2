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
            challenges.AddRange(await GetAudienceChallengesAsync());
            challenges.AddRange(await GetCrewChallengesAsync());
            challenges.AddRange(await GetMultipleChoiceChallengesAsync());
            challenges.AddRange(await GetMusicChallengesAsync());
            challenges.AddRange(await GetQuizChallengesAsync());
            challenges.AddRange(await GetScreenshotChallengesAsync());
            challenges.AddRange(await GetSilhouetteChallengesAsync());
            challenges.AddRange(await GetSologameChallengesAsync());
            return challenges;
        }

        public async Task<List<AudienceChallenge>> GetAudienceChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=AudienceChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AudienceChallenge[]>(json).ToList();
        }

        public async Task<CrewChallenge[]> GetCrewChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=CrewChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CrewChallenge[]>(json);
        }

        public async Task<MultipleChoiceChallenge[]> GetMultipleChoiceChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=MultipleChoiceChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MultipleChoiceChallenge[]>(json);
        }
        public async Task<MusicChallenge[]> GetMusicChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=MusicChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MusicChallenge[]>(json);
        }
        public async Task<QuizChallenge[]> GetQuizChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=QuizChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<QuizChallenge[]>(json);
        }
        public async Task<ScreenshotChallenge[]> GetScreenshotChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=ScreenshotChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ScreenshotChallenge[]>(json);
        }
        public async Task<SilhouetteChallenge[]> GetSilhouetteChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=SilhouetteChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SilhouetteChallenge[]>(json);
        }
        public async Task<SologameChallenge[]> GetSologameChallengesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(challengesBaseUri, "ChallengeBases/?type=SologameChallenge"));
            string json = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SologameChallenge[]>(json);
        }

        /// <summary>Edits the media object asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> EditChallengeAsync(ChallengeBase param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));
            
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            content.Headers.Add("Type", param.GetDiscriminator());
            HttpResponseMessage result = await _httpClient.PutAsync(
                new Uri(challengesBaseUri, "ChallengeBases/" + param.GetDiscriminator() + "/" + param.ChallengeID.ToString()), content
                );

            return result.IsSuccessStatusCode;
        }

        internal async Task<bool> DeleteChallengeAsync(ChallengeBase param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(challengesBaseUri, "ChallengeBases/" + param.ChallengeID.ToString()));
            return result.IsSuccessStatusCode;
        }
    }
}
