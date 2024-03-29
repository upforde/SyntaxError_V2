﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.Challenges;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyntaxError.V2.DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChallengeBasesController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public ChallengeBasesController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/ChallengeBases/?type=Type
        [HttpGet]
        public async Task<IActionResult> GetChallenges([FromQuery]string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challenges = (type != null) ? await _context.Challenges.Where(o => o.GetType().Name == type).ToListAsync() : _context.Challenges.ToList();

            if (challenges == null)
            {
                return NotFound();
            }

            return Ok(challenges);
        }

        // GET: api/ChallengeBases/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChallengeBase([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challengeBase = await _context.Challenges.FindAsync(id);

            if (challengeBase == null)
            {
                return NotFound();
            }

            return Ok(challengeBase);
        }
        
        // PUT: api/ChallengeBases/AudienceChallenge/5/
        [HttpPut("AudienceChallenge/{id}")]
        public async Task<IActionResult> PutAudienceChallenge([FromRoute] int id, [FromBody] AudienceChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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

        // PUT: api/ChallengeBases/CrewChallenge/5/
        [HttpPut("CrewChallenge/{id}")]
        public async Task<IActionResult> PutCrewChallenge([FromRoute] int id, [FromBody] CrewChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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

        // PUT: api/ChallengeBases/MultipleChoiceChallenge/5/
        [HttpPut("MultipleChoiceChallenge/{id}")]
        public async Task<IActionResult> PutMultipleChoiceChallenge([FromRoute] int id, [FromBody] MultipleChoiceChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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
        
        // PUT: api/ChallengeBases/MusicChallenge/5/
        [HttpPut("MusicChallenge/{id}")]
        public async Task<IActionResult> PutMusicChallenge([FromRoute] int id, [FromBody] MusicChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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
        
        // PUT: api/ChallengeBases/QuizChallenge/5/
        [HttpPut("QuizChallenge/{id}")]
        public async Task<IActionResult> PutQuizChallenge([FromRoute] int id, [FromBody] QuizChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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
        
        // PUT: api/ChallengeBases/ScreenshotChallenge/5/
        [HttpPut("ScreenshotChallenge/{id}")]
        public async Task<IActionResult> PutScreenshotChallenge([FromRoute] int id, [FromBody] ScreenshotChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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
        
        // PUT: api/ChallengeBases/SilhouetteChallenge/5/
        [HttpPut("SilhouetteChallenge/{id}")]
        public async Task<IActionResult> PutSilhouetteChallenge([FromRoute] int id, [FromBody] SilhouetteChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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

        // PUT: api/ChallengeBases/SologameChallenge/5/
        [HttpPut("SologameChallenge/{id}")]
        public async Task<IActionResult> PutSologameChallenge([FromRoute] int id, [FromBody] SologameChallenge challenge)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (id != challenge.ChallengeID)
            {
                return BadRequest();
            }
            
            _context.Entry(challenge).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChallengeBaseExists(id))
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
        
        // POST: api/ChallengeBases
        [HttpPost]
        public async Task<IActionResult> PostChallengeBase([FromQuery] string type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            ChallengeBase challengeWithType;
            switch (type)
            {
                case "AudienceChallenge":
                    challengeWithType = new AudienceChallenge();
                    break;
                case "CrewChallenge":
                    challengeWithType = new CrewChallenge();
                    break;
                case "MultipleChoiceChallenge":
                    challengeWithType = new MultipleChoiceChallenge();
                    break;
                case "MusicChallenge":
                    challengeWithType = new MusicChallenge();
                    break;
                case "QuizChallenge":
                    challengeWithType = new QuizChallenge();
                    break;
                case "ScreenshotChallenge":
                    challengeWithType = new ScreenshotChallenge();
                    break;
                case "SilhouetteChallenge":
                    challengeWithType = new SilhouetteChallenge();
                    break;
                case "SologameChallenge":
                    challengeWithType = new SologameChallenge();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            challengeWithType.ChallengeTask = "New Challenge";
            _context.Challenges.Add(challengeWithType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChallengeBase", new { id = challengeWithType.ChallengeID }, challengeWithType);
        }

        // DELETE: api/ChallengeBases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChallengeBase([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var challengeBase = await _context.Challenges.FindAsync(id);
            if (challengeBase == null)
            {
                return NotFound();
            }

            switch (challengeBase.GetDiscriminator())
            {
                case "MultipleChoiceChallenge":
                    var multipleChoiceChallenge = (QuestionChallenge) challengeBase;
                    var answers = _context.Answers.Find(multipleChoiceChallenge.AnswersID);
                    if (answers != null)
                        _context.Answers.Remove(answers);
                    break;
                case "MusicChallenge":
                    var musicChallenge = (MusicChallenge) challengeBase;
                    var song = _context.Objects.Find(musicChallenge.SongID);
                    if (song != null)
                        _context.Objects.Remove(song);
                    break;
                case "QuizChallenge":
                    var quizChallenge = (QuestionChallenge) challengeBase;
                    var answer = _context.Answers.Find(quizChallenge.AnswersID);
                    if (answer != null)
                        _context.Answers.Remove(answer);
                    break;
                case "ScreenshotChallenge":
                    var screenshotChallenge = (ImageChallenge) challengeBase;
                    var screenshot = _context.Objects.Find(screenshotChallenge.ImageID);
                    if (screenshot != null)
                        _context.Objects.Remove(screenshot);
                    break;
                case "SilhouetteChallenge":
                    var silhouetteChallenge = (ImageChallenge) challengeBase;
                    var silhouette = _context.Objects.Find(silhouetteChallenge.ImageID);
                    if (silhouette != null)
                        _context.Objects.Remove(silhouette);
                    break;
                default:
                    break;
            }

            _context.Challenges.Remove(challengeBase);
            await _context.SaveChangesAsync();

            return Ok(challengeBase);
        }

        /// <summary>  Checks if the challenge exists in the database.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool ChallengeBaseExists(int id)
        {
            return _context.Challenges.Any(e => e.ChallengeID == id);
        }
    }
}
