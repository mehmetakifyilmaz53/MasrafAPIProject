using MasrafDeneme.Data;
using MasrafDeneme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MasrafDeneme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PersonController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Person
        [HttpGet]

        public ActionResult<IEnumerable<Person>> GetPeople()
        {
            return _context.People.ToList();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Person> GetPerson(int id)
        {
            var person = _context.People.Find(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // POST: api/Person
        [HttpPost]
        public ActionResult<Person> PostPerson(Person person)
        {
            _context.People.Add(person);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, person);
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            var person = _context.People.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.People.Remove(person);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpGet("{id}/total-spent")]
        public ActionResult<decimal> GetTotalSpent(int id)
        {
            var totalSpent = _context.Transactions
                .Where(t => t.PersonId == id)
                .Sum(t => t.Amount);

            return totalSpent;
        }
    }
}
