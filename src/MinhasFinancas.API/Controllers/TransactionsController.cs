using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ILogger<TransactionsController> _logger;
    // In-memory storage for demonstration
    private static readonly List<TransactionDto> _transactions = new();
    private static int _nextId = 1;

    public TransactionsController(ILogger<TransactionsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TransactionDto>> GetAll()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var userTransactions = _transactions.Where(t => t.UserId == userId).ToList();
            return Ok(userTransactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transações");
            return StatusCode(500);
        }
    }

    [HttpGet("{id}")]
    public ActionResult<TransactionDto> GetById(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var transaction = _transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar transação");
            return StatusCode(500);
        }
    }

    [HttpPost]
    public ActionResult<TransactionDto> Create([FromBody] TransactionDto transaction)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            transaction.Id = _nextId++;
            transaction.UserId = userId;
            _transactions.Add(transaction);

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar transação");
            return StatusCode(500);
        }
    }

    [HttpPut("{id}")]
    public ActionResult<TransactionDto> Update(int id, [FromBody] TransactionDto transaction)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var existingTransaction = _transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            existingTransaction.Description = transaction.Description;
            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Type = transaction.Type;

            return Ok(existingTransaction);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar transação");
            return StatusCode(500);
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var transaction = _transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (transaction == null)
            {
                return NotFound();
            }

            _transactions.Remove(transaction);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar transação");
            return StatusCode(500);
        }
    }
}
