# API Testing Guide

## Testing Authentication

Este documento mostra como testar os endpoints de autenticação da API.

### Credenciais de Teste

```
Email: demo@minhasfinancas.com
Senha: demo123
```

### 1. Testar Login

#### Usando curl (Linux/Mac/WSL)

```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"demo@minhasfinancas.com","password":"demo123"}' \
  -k
```

#### Usando PowerShell (Windows)

```powershell
$body = @{
    email = "demo@minhasfinancas.com"
    password = "demo123"
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7000/api/auth/login" `
    -Method POST `
    -Body $body `
    -ContentType "application/json" `
    -SkipCertificateCheck
```

#### Resposta Esperada

```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login realizado com sucesso",
  "user": {
    "id": 1,
    "email": "demo@minhasfinancas.com",
    "name": "Usuario Demo"
  }
}
```

### 2. Testar Registro

```bash
curl -X POST https://localhost:7000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"Novo Usuario","email":"novo@email.com","password":"senha123"}' \
  -k
```

### 3. Obter Usuário Atual

Substitua `{TOKEN}` pelo token recebido no login:

```bash
curl -X GET https://localhost:7000/api/auth/me \
  -H "Authorization: Bearer {TOKEN}" \
  -k
```

### 4. Listar Transações

```bash
curl -X GET https://localhost:7000/api/transactions \
  -H "Authorization: Bearer {TOKEN}" \
  -k
```

### 5. Criar Transação

```bash
curl -X POST https://localhost:7000/api/transactions \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {TOKEN}" \
  -d '{
    "description": "Salário",
    "amount": 5000.00,
    "date": "2024-01-15T00:00:00",
    "type": 1
  }' \
  -k
```

Tipos de transação:
- `1` = Receita
- `2` = Despesa

### 6. Excluir Transação

```bash
curl -X DELETE https://localhost:7000/api/transactions/{id} \
  -H "Authorization: Bearer {TOKEN}" \
  -k
```

## Testando com Postman ou Insomnia

### Passo 1: Login

1. Crie uma nova requisição POST
2. URL: `https://localhost:7000/api/auth/login`
3. Headers:
   - `Content-Type: application/json`
4. Body (raw JSON):
   ```json
   {
     "email": "demo@minhasfinancas.com",
     "password": "demo123"
   }
   ```
5. Envie a requisição
6. Copie o valor do campo `token` da resposta

### Passo 2: Usar o Token

Para qualquer endpoint protegido:

1. Adicione um header `Authorization`
2. Valor: `Bearer {SEU_TOKEN_AQUI}`

Exemplo:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Testando com o Blazor App

1. Inicie a API:
   ```bash
   cd src/MinhasFinancas.API
   dotnet run
   ```

2. Em outro terminal, inicie o Blazor:
   ```bash
   cd src/MinhasFinancas.Blazor
   dotnet run
   ```

3. Abra o navegador em: https://localhost:7001
4. Clique em "Começar Agora"
5. Use as credenciais de teste para fazer login
6. Experimente criar e gerenciar transações

## Fluxo de Autenticação

```
┌──────────┐                ┌─────────┐                ┌──────────┐
│  Cliente │                │   API   │                │  Storage │
└────┬─────┘                └────┬────┘                └────┬─────┘
     │                           │                          │
     │  POST /api/auth/login     │                          │
     │ ─────────────────────────>│                          │
     │                           │                          │
     │                           │ Valida credenciais       │
     │                           │ ─────────────────────────│
     │                           │                          │
     │  200 OK + JWT Token       │                          │
     │ <─────────────────────────│                          │
     │                           │                          │
     │ Armazena token            │                          │
     │ ─────────────────────────────────────────────────────>│
     │                           │                          │
     │  GET /api/transactions    │                          │
     │  + Authorization: Bearer  │                          │
     │ ─────────────────────────>│                          │
     │                           │                          │
     │                           │ Valida token JWT         │
     │                           │ ─────────────────────────│
     │                           │                          │
     │  200 OK + dados           │                          │
     │ <─────────────────────────│                          │
     │                           │                          │
```

## Códigos de Status HTTP

- `200 OK` - Requisição bem-sucedida
- `201 Created` - Recurso criado com sucesso
- `400 Bad Request` - Dados inválidos
- `401 Unauthorized` - Token ausente ou inválido
- `404 Not Found` - Recurso não encontrado
- `500 Internal Server Error` - Erro no servidor

## Segurança

### Token JWT

O token JWT contém:
- **NameIdentifier**: ID do usuário
- **Email**: Email do usuário
- **Name**: Nome do usuário
- **Jti**: ID único do token

### Expiração

Tokens expiram após **60 minutos** (configurável em appsettings.json).

### Armazenamento

- **API**: Não armazena tokens (stateless)
- **Blazor**: Token em memória durante a sessão
- **Mobile**: Token em SecureStorage do dispositivo

## Troubleshooting

### Erro: "Failed to connect"

Verifique se a API está rodando:
```bash
netstat -an | grep 7000
```

### Erro: "401 Unauthorized"

- Verifique se o token está sendo enviado no header
- Verifique se o token não expirou (60 minutos)
- Faça login novamente para obter um novo token

### Erro: "Email ou senha inválidos"

- Verifique as credenciais
- Use as credenciais de teste: demo@minhasfinancas.com / demo123

### Certificado SSL em desenvolvimento

Para ignorar erros de certificado SSL em desenvolvimento:
- curl: use a flag `-k` ou `--insecure`
- PowerShell: use `-SkipCertificateCheck`
- Postman: desative "SSL certificate verification" nas configurações
