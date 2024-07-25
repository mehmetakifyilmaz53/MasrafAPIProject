using MasrafDeneme.Data;
using MasrafDeneme.Helpers;
using MasrafDeneme.Models;
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
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(AppDbContext context, ILogger<TransactionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Transaction
        [HttpGet]
        public ActionResult<IEnumerable<Transaction>> GetTransactions()
        {
            try
            {
                var transactions = _context.Transactions.Include(t => t.Person).ToList();
                return Ok(new { status = "success", message = "Transactions retrieved successfully", data = transactions });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTransactions)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public ActionResult<Transaction> GetTransaction(int id)
        {
            try
            {
                var transaction = _context.Transactions.Include(t => t.Person).FirstOrDefault(t => t.Id == id);

                if (transaction == null)
                {
                    return NotFound(new { status = "error", message = "Transaction not found" });
                }

                return Ok(new { status = "success", message = "Transaction retrieved successfully", data = transaction });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTransaction)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        // POST: api/Transaction
        [HttpPost]
        public ActionResult<Transaction> PostTransaction(Transaction transaction)
        {
            using (var transactionScope = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                    transactionScope.Commit();

                    return CreatedAtAction(nameof(GetTransaction), new { id = transaction.Id }, new { status = "success", message = "Transaction created successfully", data = transaction });
                }
                catch (Exception ex)
                {
                    transactionScope.Rollback();
                    _logger.LogError($"Error in {nameof(PostTransaction)}: {ex.Message}");
                    return StatusCode(500, new { status = "error", message = "Internal server error" });
                }
            }
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public IActionResult PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest(new { status = "error", message = "Invalid transaction ID" });
            }

            using (var transactionScope = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Entry(transaction).State = EntityState.Modified;
                    _context.SaveChanges();
                    transactionScope.Commit();

                    return Ok(new { status = "success", message = "Transaction updated successfully" });
                }
                catch (Exception ex)
                {
                    transactionScope.Rollback();
                    _logger.LogError($"Error in {nameof(PutTransaction)}: {ex.Message}");
                    return StatusCode(500, new { status = "error", message = "Internal server error" });
                }
            }
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            using (var transactionScope = _context.Database.BeginTransaction())
            {
                try
                {
                    var transaction = _context.Transactions.Find(id);
                    if (transaction == null)
                    {
                        return NotFound(new { status = "error", message = "Transaction not found" });
                    }

                    _context.Transactions.Remove(transaction);
                    _context.SaveChanges();
                    transactionScope.Commit();

                    return Ok(new { status = "success", message = "Transaction deleted successfully" });
                }
                catch (Exception ex)
                {
                    transactionScope.Rollback();
                    _logger.LogError($"Error in {nameof(DeleteTransaction)}: {ex.Message}");
                    return StatusCode(500, new { status = "error", message = "Internal server error" });
                }
            }
        }

        [HttpGet("date-range")]
        public ActionResult<IEnumerable<Transaction>> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var transactions = _context.Transactions
                    .Include(t => t.Person)
                    .Where(t => t.Date >= startDate && t.Date <= endDate)
                    .ToList();

                return Ok(new { status = "success", message = "Transactions retrieved successfully for the specified date range", data = transactions });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTransactionsByDateRange)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        [HttpGet("amount-greater-than")]
        public ActionResult<IEnumerable<Transaction>> GetTransactionsByAmount(decimal amount)
        {
            try
            {
                var transactions = _context.Transactions
                    .Include(t => t.Person)
                    .Where(t => t.Amount > amount)
                    .ToList();

                return Ok(new { status = "success", message = "Transactions retrieved successfully for the specified amount", data = transactions });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTransactionsByAmount)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }

        [HttpGet("person/{personId}/total-spent")]
        public ActionResult GetTotalSpentByPerson(int personId)
        {
            try
            {
                var totalSpent = _context.Transactions
                    .Where(t => t.PersonId == personId)
                    .Sum(t => t.Amount);

                return Ok(new { status = "success", message = "Total amount spent by person retrieved successfully", data = totalSpent });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in {nameof(GetTotalSpentByPerson)}: {ex.Message}");
                return StatusCode(500, new { status = "error", message = "Internal server error" });
            }
        }
    }
}
