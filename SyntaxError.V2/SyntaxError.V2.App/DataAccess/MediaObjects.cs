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


        /// <summary>Creates the media object asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public async Task<MediaObject> CreateMediaObjectAsync(MediaObject param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            var type = param.GetType().Name;

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PostAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/?type=" + type), content);

            param.ID = int.Parse(result.Headers.Location.Segments.Last());

            return param;
        }

        /// <summary>Gets the media objects of a specific type asynchronous.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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

        /// <summary>Edits the media object asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> EditMediaObjectAsync(MediaObject param)
        {
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/" + param.ID.ToString()), content);

            return result.IsSuccessStatusCode;
        }

        /// <summary>Deletes the media object asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        internal async Task<bool> DeleteMediaObjectAsync(MediaObject param)
        {
            HttpResponseMessage result = await _httpClient.DeleteAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/" + param.ID.ToString()));
            return result.IsSuccessStatusCode;
        }
    }
}
