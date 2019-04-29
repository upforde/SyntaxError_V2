using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.Challenges;

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

        // PUT: api/ChallengeBases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChallengeBase([FromRoute] int id, [FromBody] ChallengeBase challengeBase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != challengeBase.ChallengeID)
            {
                return BadRequest();
            }

            _context.Entry(challengeBase).State = EntityState.Modified;

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
        public async Task<IActionResult> PostChallengeBase([FromBody] ChallengeBase challengeBase)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Challenges.Add(challengeBase);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChallengeBase", new { id = challengeBase.ChallengeID }, challengeBase);
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
                    _context.Answers.Remove(answers);
                    break;
                case "MusicChallenge":
                    var musicChallenge = (MusicChallenge) challengeBase;
                    var song = _context.Objects.Find(musicChallenge.SongID);
                    _context.Objects.Remove(song);
                    break;
                case "QuizChallenge":
                    var quizChallenge = (QuestionChallenge) challengeBase;
                    var answer = _context.Answers.Find(quizChallenge.AnswersID);
                    _context.Answers.Remove(answer);
                    break;
                case "ScreenshotChallenge":
                    var screenshotChallenge = (ImageChallenge) challengeBase;
                    var screenshot = _context.Objects.Find(screenshotChallenge.ImageID);
                    _context.Objects.Remove(screenshot);
                    break;
                case "SilhouetteChallenge":
                    var silhouetteChallenge = (ImageChallenge) challengeBase;
                    var silhouette = _context.Objects.Find(silhouetteChallenge.ImageID);
                    _context.Objects.Remove(silhouette);
                    break;
                default:
                    break;
            }

            _context.Challenges.Remove(challengeBase);
            await _context.SaveChangesAsync();

            return Ok(challengeBase);
        }

        private bool ChallengeBaseExists(int id)
        {
            return _context.Challenges.Any(e => e.ChallengeID == id);
        }
    }
}
