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
    public class OuterSourceObjectsController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public OuterSourceObjectsController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/OuterSourceObjects/?type=Type
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

        // GET: api/OuterSourceObjects/5
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

        // PUT: api/OuterSourceObjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOuterSourceObject([FromRoute] int id, [FromBody] OuterSourceObject outerSourceObject)
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

        // POST: api/OuterSourceObjects
        [HttpPost]
        public async Task<IActionResult> PostOuterSourceObject([FromBody] OuterSourceObject outerSourceObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Objects.Add(outerSourceObject);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOuterSourceObject", new { id = outerSourceObject.ID }, outerSourceObject);
        }

        // DELETE: api/OuterSourceObjects/5
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

        private bool OuterSourceObjectExists(int id)
        {
            return _context.Objects.Any(e => e.ID == id);
        }
    }
}