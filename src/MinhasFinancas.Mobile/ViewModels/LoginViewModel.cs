using System.ComponentModel;
using System.Runtime.CompilerServices;
using MinhasFinancas.Mobile.Services;
using MinhasFinancas.Shared.Models;

namespace MinhasFinancas.Mobile.ViewModels;

/// <summary>
/// ViewModel for the Login page.
/// Handles user authentication (login and registration).
/// </summary>
public class LoginViewModel : INotifyPropertyChanged
{
    private readonly IAuthenticationService _authService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _name = string.Empty;
    private string _errorMessage = string.Empty;
    private string _successMessage = string.Empty;
    private bool _isLoading;
    private bool _isRegisterMode;

    public LoginViewModel(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
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

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public bool IsRegisterMode
    {
        get => _isRegisterMode;
        set
        {
            _isRegisterMode = value;
            OnPropertyChanged();
        }
    }

    public async Task<bool> LoginAsync()
    {
        IsLoading = true;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        try
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Email e senha são obrigatórios";
                return false;
            }

            LoginResponse result;

            if (IsRegisterMode)
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    ErrorMessage = "Nome é obrigatório";
                    return false;
                }

                result = await _authService.RegisterAsync(new RegisterRequest
                {
                    Name = Name,
                    Email = Email,
                    Password = Password
                });
            }
            else
            {
                result = await _authService.LoginAsync(new LoginRequest
                {
                    Email = Email,
                    Password = Password
                });
            }

            if (result.Success)
            {
                SuccessMessage = result.Message;
                return true;
            }
            else
            {
                ErrorMessage = result.Message;
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

    public void ToggleMode()
    {
        IsRegisterMode = !IsRegisterMode;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        Name = string.Empty;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
