using API.Context;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Route("v1/accounting")]
    public class AccountingController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountingController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ObjectResult> GetAccountingUsers()
        {
            List<Accounting> accountingUsers = new List<Accounting>();
            accountingUsers = _context.Accountants.ToList();

            if (accountingUsers != null)
            {
                return Ok(accountingUsers);
            }

            return NotFound("Accounting users not found");
        }

        [HttpGet("Accountant/{name}")]
        public async Task<ObjectResult> FindAccountByName(string name)
        {
            Accounting foundedUser = await _context.Accountants.FirstOrDefaultAsync<Accounting>(a => a.Name == name);
            if (foundedUser != null)
            {
                return Ok(foundedUser);
            }
            return NotFound("Not found");
        }

        [HttpGet("{id}")]
        public async Task<ObjectResult> FindAccountById(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId");
            }

            Accounting foundedUser = await _context.Accountants.FindAsync(objectId);
            if (foundedUser != null)
            {
                return Ok(foundedUser);
            }
            return NotFound("Not found");
        }

        [HttpPost]
        public async Task<ObjectResult> CreateAccountingUser(AccountingRequestData requestData)
        {
            Accounting accountingUser = new Accounting
            {
                Name = requestData.Name,
                BirthDate = requestData.BirthDate
            };

            try
            {
                _context.Accountants.Add(accountingUser);
                await _context.SaveChangesAsync();
                return Ok(accountingUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ObjectResult> UpdateAccountingUser(string id, AccountingRequestData requestData)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId");
            }

            if (id == null)
            {
                return BadRequest("Invalid id");
            }

            Accounting accounting = await _context.Accountants.FindAsync(objectId);

            if (accounting == null)
            {
                return BadRequest("User doesnt exist");
            }

            if (accounting.Name != requestData.Name)
            {
                accounting.Name = requestData.Name;
            }

            if (accounting.BirthDate != requestData.BirthDate)
            {
                accounting.BirthDate = requestData.BirthDate;
            }

            _context.SaveChanges();
            return Ok(accounting);
        }

        [HttpDelete("{id}")]
        public async Task<ObjectResult> DeleteAccountingUser(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId objectId))
            {
                return BadRequest("Invalid ObjectId");
            }

            if (id == null)
            {
                return BadRequest("Invalid id");
            }

            Accounting accounting = await _context.Accountants.FindAsync(objectId);

            if (accounting == null)
            {
                return BadRequest("User doesnt exist");
            }

            _context.Remove(accounting);
            _context.SaveChanges();

            return Ok("Deleted");
        }

        public record AccountingRequestData
        {
            [Required]
            public string Name { get; init; }
            [Required]
            public DateTime BirthDate { get; init; }
        }
    }
}