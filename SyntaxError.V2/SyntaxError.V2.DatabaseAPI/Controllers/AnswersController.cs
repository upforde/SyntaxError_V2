using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SyntaxError.V2.DataAccess;
using SyntaxError.V2.Modell.ChallengeObjects;

namespace SyntaxError.V2.DatabaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public AnswersController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/Answers
        [HttpGet]
        public IEnumerable<Answers> GetAnswers()
        {
            return _context.Answers;
        }

        // GET: api/Answers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnswers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var answers = await _context.Answers.FindAsync(id);

            if (answers == null)
            {
                return NotFound();
            }

            return Ok(answers);
        }
        

        private bool AnswersExists(int id)
        {
            return _context.Answers.Any(e => e.AnswersID == id);
        }
    }
}
