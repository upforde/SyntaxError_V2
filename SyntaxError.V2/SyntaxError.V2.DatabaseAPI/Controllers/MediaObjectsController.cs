﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.ChallengeObjects;
using SyntaxError.V2.Modell.Challenges;

namespace SyntaxError.V2.DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaObjectsController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public MediaObjectsController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/MediaObjects/?type=Type
        [HttpGet]
        public async Task<IActionResult> GetObjectsOfType([FromQuery]string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outerSourceObjects = (type != null) ? await _context.Objects.Where(o => o.GetType().Name == type).ToListAsync() : _context.Objects.ToList();

            if (outerSourceObjects == null)
            {
                return NotFound();
            }

            return Ok(outerSourceObjects);
        }
        
        // GET: api/MediaObjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOuterSourceObject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outerSourceObject = await _context.Objects.FindAsync(id);

            if (outerSourceObject == null)
            {
                return NotFound();
            }

            return Ok(outerSourceObject);
        }

        // PUT: api/MediaObjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOuterSourceObject([FromRoute] int id, [FromBody] MediaObject outerSourceObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != outerSourceObject.ID)
            {
                return BadRequest();
            }

            _context.Entry(outerSourceObject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OuterSourceObjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // POST: api/MediaObjects/?type=Type
        [HttpPost]
        public async Task<IActionResult> PostOuterSourceObject([FromQuery] string type, [FromBody] MediaObject outerSourceObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MediaObject mediaObjectWithType;
            switch (type)
            {
                case "Game":
                    mediaObjectWithType = new Game
                    {
                        ID=outerSourceObject.ID,
                        Name=outerSourceObject.Name,
                        URI=outerSourceObject.URI
                    };
                    break;
                case "Image":
                    mediaObjectWithType = new Image
                    {
                        ID=outerSourceObject.ID,
                        Name=outerSourceObject.Name,
                        URI=outerSourceObject.URI
                    };
                    break;
                case "Music":
                    mediaObjectWithType = new Music
                    {
                        ID=outerSourceObject.ID,
                        Name=outerSourceObject.Name,
                        URI=outerSourceObject.URI
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _context.Objects.Add(mediaObjectWithType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOuterSourceObject", new { id = mediaObjectWithType.ID }, outerSourceObject);
        }

        // PUT: api/MediaObjects/update
        [HttpPut("update")]
        public async Task<IActionResult> GetUpdatedGameListOfType([FromBody]List<MediaObject> objectList, [FromQuery]string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outerSourceObjects = await _context.Objects.Where(o => !objectList.Contains(o) && o.GetType().Name == type).ToListAsync();

            if (outerSourceObjects == null)
            {
                return NotFound();
            }

            return Ok(outerSourceObjects);
        }

        // DELETE: api/MediaObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOuterSourceObject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var outerSourceObject = await _context.Objects.FindAsync(id);
            if (outerSourceObject == null)
            {
                return NotFound();
            }
            
            List<ChallengeBase> challenges = new List<ChallengeBase>();

            switch (outerSourceObject.GetType().Name)
            {
                case "Game":
                    challenges.AddRange(_context.Challenges.OfType<AudienceChallenge>().Where(c => c.GameID == id).ToList());
                    challenges.AddRange(_context.Challenges.OfType<CrewChallenge>().Where(c => c.GameID == id).ToList());
                    challenges.AddRange(_context.Challenges.OfType<SologameChallenge>().Where(c => c.GameID == id).ToList());
                    break;
                case "Image":
                    challenges.AddRange(_context.Challenges.OfType<ScreenshotChallenge>().Where(c => c.ImageID == id).ToList());
                    challenges.AddRange(_context.Challenges.OfType<SilhouetteChallenge>().Where(c => c.ImageID == id).ToList());
                    break;
                case "Music":
                    challenges.AddRange(_context.Challenges.OfType<MusicChallenge>().Where(c => c.SongID == id).ToList());
                    break;
                default:
                    challenges = new List<ChallengeBase>();
                    break;
            }

            _context.Challenges.RemoveRange(challenges);
            _context.Objects.Remove(outerSourceObject);
            await _context.SaveChangesAsync();

            return Ok(outerSourceObject);
        }

        /// <summary>  Checks if the media object exists in the database.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool OuterSourceObjectExists(int id)
        {
            return _context.Objects.Any(e => e.ID == id);
        }
    }
}
