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

        public async Task<GameProfile[]> GetGameProfilesAsync()
        {
            HttpResponseMessage result = await _httpClient.GetAsync(gameProfilesBaseUri);
            string json = await result.Content.ReadAsStringAsync();
            GameProfile[] gameProfiles = JsonConvert.DeserializeObject<GameProfile[]>(json);

            return gameProfiles;
        }

        internal async Task<bool> DeleteGameProfileAsync(GameProfile param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(gameProfilesBaseUri, "GameProfiles/" + param.ID.ToString()));
            return result.IsSuccessStatusCode;
        }

        
    }
}
