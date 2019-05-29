using Newtonsoft.Json;
using SyntaxError.V2.Modell.ChallengeObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxError.V2.App.DataAccess
{
    public class CrewMembers
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri crewBaseUri = new Uri("http://localhost:51749/api/CrewMembers/");

        /// <summary>Creates a new CrewMember asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public async Task<CrewMember> CreateCrewMemberAsync(CrewMember param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));
            
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(crewBaseUri, content);

            param.CrewMemberID = int.Parse(result.Headers.Location.Segments.Last());

            return param;
        }

        /// <summary>Gets the crew members asynchronous.</summary>
        /// <returns></returns>
        public async Task<CrewMember[]> GetCrewMembersAsync()
        {
            List<CrewMember> crewMembers = new List<CrewMember>();
            
            HttpResponseMessage result = await _httpClient.GetAsync(crewBaseUri);
            string json = await result.Content.ReadAsStringAsync();
                
            return JsonConvert.DeserializeObject<CrewMember[]>(json);
        }

        /// <summary>Edits the CrewMember asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> EditCrewMemberAsync(CrewMember param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(crewBaseUri, param.CrewMemberID.ToString()), content);

            return result.IsSuccessStatusCode;
        }

        /// <summary>Deletes the CrewMember asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> DeleteCrewMemberAsync(CrewMember param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(crewBaseUri, param.CrewMemberID.ToString()));
            return result.IsSuccessStatusCode;
        }

    }
}
