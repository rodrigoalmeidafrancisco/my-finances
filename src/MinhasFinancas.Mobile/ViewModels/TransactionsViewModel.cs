using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MinhasFinancas.Mobile.Services;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Mobile.ViewModels;

/// <summary>
/// ViewModel for the Transactions page.
/// Manages the list of transactions and provides summary calculations.
/// </summary>
public class TransactionsViewModel : INotifyPropertyChanged
{
    private readonly ITransactionService _transactionService;
    private readonly IAuthenticationService _authService;
    private bool _isLoading;
    private string _errorMessage = string.Empty;
    private string _successMessage = string.Empty;

    public TransactionsViewModel(
        ITransactionService transactionService,
        IAuthenticationService authService)
    {
        _transactionService = transactionService;
        _authService = authService;
        Transactions = new ObservableCollection<TransactionDto>();
    }

    public ObservableCollection<TransactionDto> Transactions { get; }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public string SuccessMessage
    {
        get => _successMessage;
        set
        {
            _successMessage = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalReceitas
    {
        get
        {
            return Transactions
                .Where(t => t.Type == TransactionType.Receita)
                .Sum(t => t.Amount);
        }
    }

    public decimal TotalDespesas
    {
        get
        {
            return Transactions
                .Where(t => t.Type == TransactionType.Despesa)
                .Sum(t => t.Amount);
        }
    }

    public decimal Saldo => TotalReceitas - TotalDespesas;

    public async Task LoadTransactionsAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var transactions = await _transactionService.GetAllAsync();
            Transactions.Clear();
            foreach (var transaction in transactions.OrderByDescending(t => t.Date))
            {
                Transactions.Add(transaction);
            }

            UpdateCalculations();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao carregar transações: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<bool> CreateTransactionAsync(TransactionDto transaction)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var result = await _transactionService.CreateAsync(transaction);
            if (result != null)
            {
                Transactions.Insert(0, result);
                UpdateCalculations();
                SuccessMessage = "Transação adicionada com sucesso!";
                return true;
            }
            else
            {
                ErrorMessage = "Erro ao adicionar transação";
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task<bool> DeleteTransactionAsync(int id)
    {
        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var success = await _transactionService.DeleteAsync(id);
            if (success)
            {
                var transaction = Transactions.FirstOrDefault(t => t.Id == id);
                if (transaction != null)
                {
                    Transactions.Remove(transaction);
                    UpdateCalculations();
                }
                SuccessMessage = "Transação excluída com sucesso!";
                return true;
            }
            else
            {
                ErrorMessage = "Erro ao excluir transação";
                return false;
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro: {ex.Message}";
            return false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
    }

    private void UpdateCalculations()
    {
        OnPropertyChanged(nameof(TotalReceitas));
        OnPropertyChanged(nameof(TotalDespesas));
        OnPropertyChanged(nameof(Saldo));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
