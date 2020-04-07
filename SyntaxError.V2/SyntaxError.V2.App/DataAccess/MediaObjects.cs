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
            if (!param.Name.Equals(""))
            {
                string json = await Task.Run(() => JsonConvert.SerializeObject(param));

                var type = param.GetType().Name;

                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage result = await _httpClient.PostAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/?type=" + type), content);

                param.ID = int.Parse(result.Headers.Location.Segments.Last());
            }
            return param;
        }

        public async Task<MediaObject> GetMediaObjectAsync(int? id, string type)
        {
            HttpResponseMessage result = await _httpClient.GetAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/" + id));

            string json = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                switch (type)
                {
                    case "Game":
                        return JsonConvert.DeserializeObject<Game>(json);
                    case "Image":
                        return JsonConvert.DeserializeObject<Image>(json);
                    case "Music":
                        return JsonConvert.DeserializeObject<Music>(json);
                    default:
                        throw new ArgumentException();
                }
            } return null;
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
                    if (result.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<Game[]>(json);
                    return new Game[0];
                case "Image":
                    if (result.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<Image[]>(json);
                    return new Image[0];
                case "Music":
                    if (result.IsSuccessStatusCode)
                        return JsonConvert.DeserializeObject<Music[]>(json);
                    return new Music[0];
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

        internal async Task<Game[]> UpdateGameListAsync(List<Game> param)
        {
            var type = param.First().GetType().Name;
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/update/?type=" + type), content);

            string resultJson = await result.Content.ReadAsStringAsync();
            
            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Game[]>(resultJson);
            return new Game[0];
        }

        internal async Task<Image[]> UpdateImageListAsync(List<Image> param)
        {
            var type = param.First().GetType().Name;
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/update/?type=" + type), content);

            string resultJson = await result.Content.ReadAsStringAsync();
            
            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Image[]>(resultJson);
            return new Image[0];
        }

        internal async Task<Music[]> UpdateMusicListAsync(List<Music> param)
        {
            var type = param.First().GetType().Name;
            string json = await Task.Run(() => JsonConvert.SerializeObject(param));

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage result = await _httpClient.PutAsync(new Uri(MediaObjectsBaseUri, "MediaObjects/update/?type=" + type), content);

            string resultJson = await result.Content.ReadAsStringAsync();
            
            if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<Music[]>(resultJson);
            return new Music[0];
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
