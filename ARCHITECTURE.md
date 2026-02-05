# Arquitetura e Fluxo de Autenticação

## Visão Geral da Arquitetura

```
┌─────────────────────────────────────────────────────────────────┐
│                        Camada de Cliente                         │
├──────────────────────────┬──────────────────────────────────────┤
│   Blazor Web App         │      MAUI Mobile App                 │
│   (MinhasFinancas.Blazor)│   (MinhasFinancas.Mobile)            │
│                          │                                       │
│   - Login.razor          │   - LoginViewModel                   │
│   - Transactions.razor   │   - TransactionsViewModel            │
│   - AuthService          │   - AuthenticationService            │
│   - TransactionService   │   - TransactionService               │
└──────────────────────────┴──────────────────────────────────────┘
                              │
                              │ HTTPS + JWT Token
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                      Camada de API                               │
│                  (MinhasFinancas.API)                            │
│                                                                   │
│   ┌──────────────────────────────────────────────────────┐     │
│   │  Controllers                                          │     │
│   │  - AuthController (Login/Register)                    │     │
│   │  - TransactionsController (CRUD)                      │     │
│   └──────────────────────────────────────────────────────┘     │
│                              │                                    │
│   ┌──────────────────────────────────────────────────────┐     │
│   │  Services                                             │     │
│   │  - TokenService (Gera JWT)                            │     │
│   │  - AuthService (Valida usuário)                       │     │
│   └──────────────────────────────────────────────────────┘     │
│                              │                                    │
│   ┌──────────────────────────────────────────────────────┐     │
│   │  Models                                               │     │
│   │  - User (Dados do usuário)                            │     │
│   │  - Transaction (Transações)                           │     │
│   └──────────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Camada de Dados                               │
│                  (Em memória - Demo)                             │
│                                                                   │
│   - Lista de usuários em memória                                 │
│   - Lista de transações em memória                               │
│                                                                   │
│   ⚠️ Para produção: substituir por banco de dados real           │
└─────────────────────────────────────────────────────────────────┘
```

## Modelos Compartilhados

```
┌─────────────────────────────────────────────────────────────────┐
│                 MinhasFinancas.Shared                            │
│                                                                   │
│   Models usados por todos os projetos:                           │
│   - LoginRequest                                                 │
│   - LoginResponse                                                │
│   - RegisterRequest                                              │
│   - UserInfo                                                     │
│   - TransactionDto                                               │
└─────────────────────────────────────────────────────────────────┘
```

## Fluxo de Autenticação Detalhado

### 1. Processo de Login

```
┌──────────┐           ┌─────────────┐           ┌────────────┐
│  Cliente │           │AuthController│          │ AuthService│
└────┬─────┘           └──────┬──────┘           └─────┬──────┘
     │                        │                        │
     │ 1. POST /api/auth/login│                        │
     │    {email, password}   │                        │
     │───────────────────────>│                        │
     │                        │                        │
     │                        │ 2. AuthenticateAsync() │
     │                        │───────────────────────>│
     │                        │                        │
     │                        │  3. Busca usuário      │
     │                        │     e valida senha     │
     │                        │<───────────────────────│
     │                        │  (User object)         │
     │                        │                        │
     │                        │ 4. GenerateToken()     │
     │                        │──────────────┐         │
     │                        │              │         │
     │                        │  JWT Token   │         │
     │                        │<─────────────┘         │
     │                        │                        │
     │ 5. LoginResponse       │                        │
     │    {token, user}       │                        │
     │<───────────────────────│                        │
     │                        │                        │
     │ 6. Armazena token      │                        │
     │    localmente          │                        │
     │─────────────┐          │                        │
     │             │          │                        │
     │<────────────┘          │                        │
```

### 2. Requisições Autenticadas

```
┌──────────┐           ┌──────────────────┐           ┌────────────┐
│  Cliente │           │TransactionsController│        │   [Authorize]
└────┬─────┘           └─────────┬────────────┘       │   Middleware
     │                           │                     │              │
     │ 1. GET /api/transactions  │                     │              │
     │    Authorization: Bearer {token}                │              │
     │───────────────────────────────────────────────────────────────>│
     │                           │                     │              │
     │                           │                     │  2. Valida   │
     │                           │                     │     token    │
     │                           │                     │<─────────────│
     │                           │                     │              │
     │                           │   3. Request autorizado            │
     │                           │     User.Claims preenchido         │
     │                           │<──────────────────────────────────│
     │                           │                     │              │
     │  4. Lista transações      │                     │              │
     │     do usuário            │                     │              │
     │<──────────────────────────│                     │              │
```

## Estrutura do Token JWT

### Claims Incluídas

```json
{
  "nameid": "1",                          // ID do usuário
  "email": "demo@minhasfinancas.com",     // Email do usuário
  "name": "Usuario Demo",                 // Nome do usuário
  "jti": "unique-token-id",               // ID único do token
  "exp": 1234567890,                      // Data de expiração
  "iss": "MinhasFinancasAPI",             // Emissor
  "aud": "MinhasFinancasClients"          // Audiência
}
```

### Exemplo de Token

```
Header:
{
  "alg": "HS256",
  "typ": "JWT"
}

Payload:
{
  "nameid": "1",
  "email": "demo@minhasfinancas.com",
  "name": "Usuario Demo",
  "jti": "abc123...",
  "exp": 1234567890,
  "iss": "MinhasFinancasAPI",
  "aud": "MinhasFinancasClients"
}

Signature:
HMACSHA256(
  base64UrlEncode(header) + "." +
  base64UrlEncode(payload),
  secret-key
)
```

## Implementação por Projeto

### API (MinhasFinancas.API)

#### Program.cs - Configuração JWT

```csharp
// Adiciona serviços de autenticação
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Middleware de autenticação (ordem importa!)
app.UseAuthentication();  // Primeiro autentica
app.UseAuthorization();   // Depois autoriza
```

#### AuthController - Endpoints

```csharp
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        // 1. Valida usuário
        var user = await _authService.AuthenticateAsync(email, password);
        
        // 2. Gera token
        var token = _tokenService.GenerateToken(user);
        
        // 3. Retorna resposta
        return Ok(new LoginResponse { Token = token, User = ... });
    }
}
```

#### Proteção de Endpoints

```csharp
[Authorize]  // Requer token JWT válido
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<TransactionDto>> GetAll()
    {
        // Obtém ID do usuário do token
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Retorna apenas transações deste usuário
        return _transactions.Where(t => t.UserId == userId);
    }
}
```

### Blazor (MinhasFinancas.Blazor)

#### AuthenticationService - Cliente

```csharp
public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    // 1. Chama API
    var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
    
    // 2. Armazena token
    _token = result.Token;
    
    // 3. Configura HttpClient para incluir token
    _httpClient.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", _token);
    
    return result;
}
```

#### Login.razor - Componente

```razor
@code {
    private async Task HandleLogin()
    {
        // 1. Chama serviço de autenticação
        var result = await AuthService.LoginAsync(loginRequest);
        
        // 2. Se sucesso, redireciona
        if (result.Success)
        {
            Navigation.NavigateTo("/transactions");
        }
    }
}
```

### Mobile (MinhasFinancas.Mobile)

#### SecureStorage - Armazenamento

```csharp
// MAUI SecureStorage para tokens
public async Task SetAsync(string key, string value)
{
    await SecureStorage.Default.SetAsync(key, value);
}

public async Task<string?> GetAsync(string key)
{
    return await SecureStorage.Default.GetAsync(key);
}
```

#### AuthenticationService - Cliente Mobile

```csharp
public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    // 1. Chama API
    var result = await _httpClient.PostAsJsonAsync("api/auth/login", request);
    
    // 2. Armazena token de forma segura
    await _secureStorage.SetAsync("auth_token", result.Token);
    
    // 3. Configura header para próximas requisições
    _httpClient.DefaultRequestHeaders.Authorization = 
        new AuthenticationHeaderValue("Bearer", result.Token);
    
    return result;
}
```

## Segurança

### Melhores Práticas Implementadas

✅ **Token JWT com assinatura HMAC-SHA256**
- Tokens são assinados e verificados
- Não podem ser adulterados

✅ **Senhas com hash**
- Senhas nunca são armazenadas em texto puro
- Hash SHA256 aplicado

✅ **HTTPS obrigatório**
- Comunicação criptografada
- Tokens protegidos em trânsito

✅ **Claims de usuário**
- Informações do usuário no token
- Sem necessidade de consultar BD a cada requisição

✅ **Expiração de tokens**
- Tokens expiram após 60 minutos
- Força re-autenticação periódica

✅ **Validação de token em cada requisição**
- Middleware [Authorize] verifica automaticamente
- Rejeita tokens inválidos ou expirados

### Para Produção

⚠️ **Adicione estas melhorias de segurança:**

1. **Refresh Tokens**
   - Tokens de curta duração + refresh tokens
   - Renovação automática sem re-login

2. **Banco de Dados Real**
   - Entity Framework Core
   - SQL Server / PostgreSQL
   - Hashing com bcrypt ou Argon2

3. **Revogação de Tokens**
   - Lista negra de tokens
   - Logout efetivo

4. **Rate Limiting**
   - Limite de tentativas de login
   - Proteção contra força bruta

5. **CORS Configurado**
   - Apenas origens confiáveis
   - Não usar AllowAll em produção

6. **Variáveis de Ambiente**
   - Secret key em variável de ambiente
   - Não commit de chaves em código

7. **HTTPS Forçado**
   - Redirecionamento automático
   - HSTS headers

8. **Validação de Input**
   - FluentValidation
   - Data Annotations

## Fluxo de Desenvolvimento

### Para Adicionar Nova Funcionalidade

1. **Criar modelo em Shared**
   ```csharp
   // MinhasFinancas.Shared/Models/NovoModel.cs
   public class NovoModel { ... }
   ```

2. **Adicionar endpoint na API**
   ```csharp
   // MinhasFinancas.API/Controllers/NovoController.cs
   [Authorize]
   [ApiController]
   [Route("api/[controller]")]
   public class NovoController : ControllerBase { ... }
   ```

3. **Criar serviço no Blazor**
   ```csharp
   // MinhasFinancas.Blazor/Services/NovoService.cs
   public class NovoService : INovoService { ... }
   ```

4. **Criar página Blazor**
   ```razor
   // MinhasFinancas.Blazor/Components/Pages/Novo.razor
   @page "/novo"
   @inject INovoService NovoService
   ```

5. **Criar serviço no Mobile**
   ```csharp
   // MinhasFinancas.Mobile/Services/NovoService.cs
   public class NovoService : INovoService { ... }
   ```

6. **Criar ViewModel no Mobile**
   ```csharp
   // MinhasFinancas.Mobile/ViewModels/NovoViewModel.cs
   public class NovoViewModel : INotifyPropertyChanged { ... }
   ```

## Testando a Integração

### 1. Testar API diretamente
```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"demo@minhasfinancas.com","password":"demo123"}'
```

### 2. Testar Blazor
1. Iniciar API
2. Iniciar Blazor
3. Navegar para https://localhost:7001
4. Fazer login e testar funcionalidades

### 3. Testar Mobile
1. Iniciar API
2. Configurar URL da API no Mobile
3. Executar app MAUI
4. Fazer login e testar funcionalidades

## Recursos Adicionais

### Documentação Oficial
- [ASP.NET Core Security](https://docs.microsoft.com/aspnet/core/security/)
- [JWT.io](https://jwt.io/) - Decodificador de tokens
- [Blazor](https://docs.microsoft.com/aspnet/core/blazor/)
- [.NET MAUI](https://docs.microsoft.com/dotnet/maui/)

### Ferramentas Úteis
- [Postman](https://www.postman.com/) - Testar API
- [JWT Debugger](https://jwt.io/#debugger) - Verificar tokens
- [Swagger/OpenAPI](https://swagger.io/) - Documentação de API
