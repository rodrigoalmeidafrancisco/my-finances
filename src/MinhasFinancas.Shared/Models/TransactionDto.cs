namespace MinhasFinancas.Shared.Models;

public class TransactionDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
}

public enum TransactionType
{
    Receita = 1,
    Despesa = 2
}
