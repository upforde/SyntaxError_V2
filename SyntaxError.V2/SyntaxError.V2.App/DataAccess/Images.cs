using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SyntaxError.V2.App.DataAccess
{
    public class Images
    {
        readonly HttpClient _httpClient = new HttpClient();
        static readonly Uri imageBaseUri = new Uri("http://localhost:56806/api/Images");

        /// <summary>Uploads the Image asynchronous.</summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public async Task<string> PostImageAsync(StorageFile file)
        {
            var randomAccessStream = await file.OpenReadAsync();
            Stream stream = randomAccessStream.AsStreamForRead();

            using (var content = new MultipartFormDataContent())
            {
                var streamContent = new StreamContent(stream);
                streamContent.Headers.Add("Content-Type", "application/octet-stream");
                streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + file.Path + "\"");
                content.Add(streamContent, "file");
                
                HttpResponseMessage result = await _httpClient.PostAsync(imageBaseUri, content);
                return result.Headers.Location.ToString();
            }
        }
    }
}
