namespace MinhasFinancas.API.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
}

public enum TransactionType
{
    Receita = 1,
    Despesa = 2
}
