using Newtonsoft.Json;
using SyntaxError.V2.Modell.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.DataAccess
{
    public class GameProfiles
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri gameProfilesBaseUri = new Uri("http://localhost:51749/api/GameProfiles");

        public async Task<GameProfile> CreateNewGame(GameProfile param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));
            
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(gameProfilesBaseUri, content);

            param.ID = int.Parse(result.Headers.Location.Segments.Last());

            return param;
        }

        public async Task<GameProfile[]> GetGameProfilesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(gameProfilesBaseUri);
            string json = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<GameProfile[]>(json);
            return new GameProfile[0];
        }

        public async Task<bool> EditGameProfileAsync(GameProfile param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));
            
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(gameProfilesBaseUri, "GameProfiles/" + param.ID.ToString()), content);

            return result.IsSuccessStatusCode;
        }

        internal async Task<bool> DeleteGameProfileAsync(GameProfile param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(gameProfilesBaseUri, "GameProfiles/" + param.ID.ToString()));
            return result.IsSuccessStatusCode;
        }

        
    }
}
