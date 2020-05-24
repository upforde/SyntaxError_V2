using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SyntaxError.V2.ImageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IHostingEnvironment environment;

        public ImagesController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        /// <summary>
        /// Gets the specified image.
        /// </summary>
        /// <param name="name">The name of the image.</param>
        /// <returns>Returns the image, otherwise NotFound</returns>
        /// <remarks>
        /// YOU need to add error handling!
        /// </remarks>
        [Route("{name}", Name = "GetImageByName")]
        public ActionResult Get(string name)
        {
            string imagePath = GetImagePath();
            string fileName = $"{imagePath}\\{name}";

            if (!System.IO.File.Exists(fileName))
                return NotFound();

            var image = System.IO.File.ReadAllBytes(fileName);

            string extension = new System.IO.FileInfo(fileName).Extension.Substring(1);
            return File(image, $"image/{extension}");
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <remarks>Inspired by 
        /// http://www.c-sharpcorner.com/article/uploading-image-to-server-using-web-api-2-0/
        /// Handles ONLY upload of ONE file
        /// 
        /// YOU need to add error handling!
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var httpContext = HttpContext.Request.Form;
            var file = HttpContext.Request.Form.Files.Count == 1 ? HttpContext.Request.Form.Files[0] : null;

            if (file != null && file.Length > 0)
            {
                string fileName = System.IO.Path.GetFileName(file.FileName);
                string imagePath = GetImagePath();

                using (var outStream = System.IO.File.Create($"{imagePath}\\{fileName}"))
                    await file.CopyToAsync(outStream);

                return CreatedAtRoute("GetImageByName", new { name = fileName }, null);
            }
            else
                return BadRequest();
        }

        /// <summary>Deletes the specified file.</summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(string name)
        {
            if (!System.IO.File.Exists(name))
                return NotFound();

            System.IO.File.Delete(name);

            return Ok();
        }

        /// <summary>
        /// Gets the image path.
        /// </summary>
        /// <returns></returns>
        private string GetImagePath()
        {
            var path = $"{environment.WebRootPath}\\uploads";

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            return path;
        }
    }
}