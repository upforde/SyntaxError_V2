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
    public class CrewMembersController : ControllerBase
    {
        private readonly SyntaxErrorContext _context;

        public CrewMembersController(SyntaxErrorContext context)
        {
            _context = context;
        }

        // GET: api/CrewMembers
        [HttpGet]
        public IEnumerable<CrewMember> GetCrewMembers()
        {
            return _context.CrewMembers;
        }

        // GET: api/CrewMembers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCrewMember([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var crewMember = await _context.CrewMembers.FindAsync(id);

            if (crewMember == null)
            {
                return NotFound();
            }

            return Ok(crewMember);
        }

        // PUT: api/CrewMembers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCrewMember([FromRoute] int id, [FromBody] CrewMember crewMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != crewMember.CrewMemberID)
            {
                return BadRequest();
            }

            _context.Entry(crewMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CrewMemberExists(id))
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

        // POST: api/CrewMembers
        [HttpPost]
        public async Task<IActionResult> PostCrewMember([FromBody] CrewMember crewMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CrewMembers.Add(crewMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCrewMember", new { id = crewMember.CrewMemberID }, crewMember);
        }

        // DELETE: api/CrewMembers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCrewMember([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var crewMember = await _context.CrewMembers.FindAsync(id);
            if (crewMember == null)
            {
                return NotFound();
            }

            var challenges = _context.Challenges.OfType<CrewChallenge>().Where(c => c.CrewMemberID == id).ToList();
            _context.Challenges.RemoveRange(challenges);
            _context.CrewMembers.Remove(crewMember);
            await _context.SaveChangesAsync();

            return Ok(crewMember);
        }

        /// <summary>Checks if the crew member exists in the database.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private bool CrewMemberExists(int id)
        {
            return _context.CrewMembers.Any(e => e.CrewMemberID == id);
        }
    }
}
