using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.Utility;

namespace SyntaxError.V2.DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameProfilesController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public GameProfilesController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/GameProfiles
        [HttpGet]
        public IEnumerable<GameProfile> GetGameProfiles()
        {
            return _context.GameProfiles
                .Include(gp => gp.SaveGame)
                .ThenInclude(sg => sg.Challenges)
                .Include(gp => gp.Profile)
                .ThenInclude(p => p.Challenges);
        }

        // GET: api/GameProfiles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameProfile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameProfile = await _context.GameProfiles.FindAsync(id);

            if (gameProfile == null)
            {
                return NotFound();
            }
            else
            {
                var profileID = (int) gameProfile.ProfileID;
                var saveGameID = (int) gameProfile.SaveGameID;
                gameProfile.Profile = (Profile) await _context.UsingPanes.FindAsync(profileID);
                gameProfile.Profile.Challenges = await _context.UsingChallenges.Where(c => c.UsingID == profileID).ToListAsync();
                gameProfile.SaveGame = (SaveGame) await _context.UsingPanes.FindAsync(saveGameID);
                gameProfile.SaveGame.Challenges = await _context.UsingChallenges.Where(c => c.UsingID == saveGameID).ToListAsync();
            }

            return Ok(gameProfile);
        }

        // PUT: api/GameProfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameProfile([FromRoute] int id, [FromBody] GameProfile gameProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gameProfile.ID)
            {
                return BadRequest();
            }

            _context.Entry(gameProfile).State = EntityState.Modified;

            _context.UsingChallenges.RemoveRange(await _context.UsingChallenges.Where(x => x.UsingID == gameProfile.ProfileID).ToListAsync());

            foreach (var challenge in gameProfile.Profile.Challenges)
                _context.UsingChallenges.Add(new UsingChallenge { ChallengeID=challenge.ChallengeID, UsingID=gameProfile.ProfileID });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameProfileExists(id))
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

        // PUT: api/GameProfiles/5/Refresh
        [HttpPut("{id}/Refresh")]
        public async Task<IActionResult> RefreshGameProfileSaveGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var profile = _context.GameProfiles.Where(x => x.ID == id).FirstOrDefault();

            if (profile != null)
            {
                _context.UsingChallenges.RemoveRange(await _context.UsingChallenges.Where(x => x.UsingID == profile.SaveGameID).ToListAsync());

                _context.Entry(profile).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameProfileExists(id))
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

        // POST: api/GameProfiles
        [HttpPost]
        public async Task<IActionResult> PostGameProfile([FromBody] GameProfile gameProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GameProfiles.Add(gameProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameProfile", new { id = gameProfile.ID }, gameProfile);
        }

        // POST: api/GameProfiles/SaveGameEntry
        [HttpPost("SaveGameEntry")]
        public async Task<IActionResult> PostSaveGameEntry([FromBody] UsingChallenge usingChallenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.UsingChallenges.Add(new UsingChallenge { ChallengeID = usingChallenge.ChallengeID, UsingID = usingChallenge.UsingID });

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/GameProfiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameProfile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameProfile = await _context.GameProfiles.FindAsync(id);
            if (gameProfile == null)
            {
                return NotFound();
            }
            else
            {
                var profileID = (int) gameProfile.ProfileID;
                var saveGameID = (int) gameProfile.SaveGameID;
                gameProfile.Profile = (Profile) await _context.UsingPanes.FindAsync(profileID);
                gameProfile.Profile.Challenges = await _context.UsingChallenges.Where(c => c.UsingID == profileID).ToListAsync();
                gameProfile.SaveGame = (SaveGame) await _context.UsingPanes.FindAsync(saveGameID);
                gameProfile.SaveGame.Challenges = await _context.UsingChallenges.Where(c => c.UsingID == saveGameID).ToListAsync();
            }

            _context.UsingChallenges.RemoveRange(gameProfile.Profile.Challenges);
            _context.UsingChallenges.RemoveRange(gameProfile.SaveGame.Challenges);
            _context.UsingPanes.Remove(gameProfile.Profile);
            _context.UsingPanes.Remove(gameProfile.SaveGame);
            _context.GameProfiles.Remove(gameProfile);
            await _context.SaveChangesAsync();

            return Ok(gameProfile);
        }

        /// <summary>  Checks if the game profile exists in the database.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool GameProfileExists(int id)
        {
            return _context.GameProfiles.Any(e => e.ID == id);
        }
    }
}
