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
    public class MediaObjects
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri MediaObjectsBaseUri = new Uri("http://localhost:51749/api/MediaObjects");

        public async Task<MediaObject[]> GetMediaObjectsOfTypeAsync(string type)
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/?type=" + type));
            string json = await result.Content.ReadAsStringAsync();
            switch (type)
            {
                case "Game":
                    return JsonConvert.DeserializeObject<Game[]>(json);
                case "Image":
                     return JsonConvert.DeserializeObject<Image[]>(json);
                case "Music":
                    return JsonConvert.DeserializeObject<Music[]>(json);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
