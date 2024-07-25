using MasrafDeneme.Data;
using MasrafDeneme.Helpers;
using MasrafDeneme.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MasrafDeneme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PersonController> _logger;

        public PersonController(AppDbContext context, ILogger<PersonController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Person
        [HttpGet]
        public ActionResult<IEnumerable<Person>> GetPeople()
        {
            try
            {
                new Aggregate(_context).DailyAggregate();
                var people = _context.People.ToList();
                return Ok(new { status = "success", message = "People retrieved successfully", data = people });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetPeople)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<Person> GetPerson(int id)
        {
            try
            {
                var person = _context.People.Find(id);

                if (person == null)
                {
                    return NotFound(new { status = "error", message = "Person not found" });
                }

                return Ok(new { status = "success", message = "Person retrieved successfully", data = person });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetPerson)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // POST: api/Person
        [HttpPost]
        public ActionResult<Person> PostPerson(Person person)
        {
            try
            {
                _context.People.Add(person);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, new { status = "success", message = "Person created successfully", data = person });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PostPerson)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public IActionResult PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest(new { status = "error", message = "Invalid person ID" });
            }

            try
            {
                _context.Entry(person).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok(new { status = "success", message = "Person updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(PutPerson)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public IActionResult DeletePerson(int id)
        {
            try
            {
                var person = _context.People.Find(id);
                if (person == null)
                {
                    return NotFound(new { status = "error", message = "Person not found" });
                }

                _context.People.Remove(person);
                _context.SaveChanges();

                return Ok(new { status = "success", message = "Person deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(DeletePerson)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // GET: api/Person/5/total-spent
        [HttpGet("{id}/total-spent")]
        public ActionResult<decimal> GetTotalSpent(int id)
        {
            try
            {
                var totalSpent = _context.Transactions
                    .Where(t => t.PersonId == id)
                    .Sum(t => t.Amount);

                return Ok(new { status = "success", message = "Total spent retrieved successfully", data = totalSpent });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTotalSpent)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // POST: api/Person/CronJobTest
        [HttpPost("CronJobTest")]
        public ActionResult<string> CronJobTest()
        {
            try
            {
                new Aggregate(_context).DailyAggregate();
                return Ok(new { status = "success", message = "Cron job executed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(CronJobTest)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }
    }
}
