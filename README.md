# Minhas Finanças

Sistema completo para controle financeiro pessoal com aplicativo MAUI Mobile, aplicativo Blazor Web e API em .NET Core com autenticação JWT.

## 🏗️ Arquitetura

O projeto está dividido em 4 componentes principais:

1. **MinhasFinancas.API** - API RESTful com autenticação JWT
2. **MinhasFinancas.Blazor** - Aplicativo Web Blazor
3. **MinhasFinancas.Mobile** - Biblioteca para aplicativo MAUI (Mobile)
4. **MinhasFinancas.Shared** - Modelos compartilhados entre projetos

## 🚀 Funcionalidades

### Autenticação
- ✅ Registro de novos usuários
- ✅ Login com email e senha
- ✅ Autenticação via JWT (JSON Web Token)
- ✅ Token seguro armazenado no cliente
- ✅ Validação de token em todas as requisições à API

### Gerenciamento de Transações
- ✅ Criar transações (Receitas e Despesas)
- ✅ Listar todas as transações do usuário
- ✅ Excluir transações
- ✅ Visualizar totais e saldo

## 📋 Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) ou superior
- IDE recomendada: Visual Studio 2022 ou VS Code

Para o projeto MAUI (opcional):
- Workload do .NET MAUI instalado
- Para desenvolvimento Android: Android SDK
- Para desenvolvimento iOS: macOS com Xcode

## 🔧 Instalação e Configuração

### 1. Clonar o Repositório

```bash
git clone https://github.com/rodrigoalmeidafrancisco/minhas-financas.git
cd minhas-financas
```

### 2. Restaurar Dependências

```bash
dotnet restore
```

### 3. Configurar a API

A API já vem configurada com valores padrão. Para produção, atualize o arquivo `src/MinhasFinancas.API/appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "SUA-CHAVE-SECRETA-AQUI-MINIMO-32-CARACTERES",
    "Issuer": "MinhasFinancasAPI",
    "Audience": "MinhasFinancasClients",
    "ExpirationMinutes": "60"
  }
}
```

### 4. Executar a API

```bash
cd src/MinhasFinancas.API
dotnet run
```

A API estará disponível em:
- HTTPS: https://localhost:7000
- HTTP: http://localhost:5000

### 5. Executar o Blazor Web App

Em um novo terminal:

```bash
cd src/MinhasFinancas.Blazor
dotnet run
```

O aplicativo Blazor estará disponível em:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001

### 6. Configurar o Projeto Mobile (MAUI)

O projeto Mobile contém toda a estrutura necessária. Para criar um app MAUI completo:

1. Instale o workload do MAUI:
```bash
dotnet workload install maui
```

2. Crie um novo projeto MAUI:
```bash
dotnet new maui -n MinhasFinancas.MauiApp -o src/MinhasFinancas.MauiApp
```

3. Adicione referência ao projeto Mobile:
```bash
cd src/MinhasFinancas.MauiApp
dotnet add reference ../MinhasFinancas.Mobile/MinhasFinancas.Mobile.csproj
```

4. Siga as instruções em `src/MinhasFinancas.Mobile/README.md` para configuração completa.

## 🔑 Credenciais de Teste

O sistema já vem com um usuário de demonstração pré-cadastrado:

- **Email:** demo@minhasfinancas.com
- **Senha:** demo123

## 📚 Estrutura de Pastas

```
minhas-financas/
├── src/
│   ├── MinhasFinancas.API/          # API RESTful
│   │   ├── Controllers/             # Controladores da API
│   │   ├── Models/                  # Modelos da API
│   │   ├── Services/                # Serviços (Auth, Token)
│   │   └── Program.cs               # Configuração da API
│   │
│   ├── MinhasFinancas.Blazor/       # Aplicativo Web Blazor
│   │   ├── Components/              # Componentes Blazor
│   │   │   ├── Pages/               # Páginas (Login, Transactions)
│   │   │   └── Layout/              # Layout do aplicativo
│   │   ├── Services/                # Serviços do Blazor
│   │   └── Program.cs               # Configuração do Blazor
│   │
│   ├── MinhasFinancas.Mobile/       # Biblioteca Mobile (MAUI)
│   │   ├── Services/                # Serviços (Auth, Transactions, Storage)
│   │   ├── ViewModels/              # ViewModels (MVVM)
│   │   └── README.md                # Guia detalhado do Mobile
│   │
│   └── MinhasFinancas.Shared/       # Modelos compartilhados
│       └── Models/                  # DTOs e modelos
│
├── MinhasFinancas.sln               # Solução do Visual Studio
└── README.md                        # Este arquivo
```

## 🔒 Segurança

### JWT (JSON Web Token)
- Tokens são gerados na API após login bem-sucedido
- Tokens são enviados no header `Authorization: Bearer {token}`
- Tokens expiram após 60 minutos (configurável)
- Senhas são armazenadas com hash SHA256

### Armazenamento
- **API**: Armazenamento em memória (para demonstração)
- **Blazor**: Token armazenado em memória da sessão
- **Mobile**: Token armazenado em SecureStorage do dispositivo

> ⚠️ **IMPORTANTE**: Para produção, implemente um banco de dados real (SQL Server, PostgreSQL, etc.)

## 🌐 API Endpoints

### Autenticação

#### POST /api/auth/login
Login de usuário existente.

**Request:**
```json
{
  "email": "usuario@email.com",
  "password": "senha123"
}
```

**Response:**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "message": "Login realizado com sucesso",
  "user": {
    "id": 1,
    "email": "usuario@email.com",
    "name": "Nome do Usuário"
  }
}
```

#### POST /api/auth/register
Registro de novo usuário.

**Request:**
```json
{
  "name": "Nome do Usuário",
  "email": "usuario@email.com",
  "password": "senha123"
}
```

#### GET /api/auth/me
Obter informações do usuário autenticado (requer token).

### Transações

Todos os endpoints de transações requerem autenticação (header `Authorization: Bearer {token}`).

#### GET /api/transactions
Lista todas as transações do usuário.

#### GET /api/transactions/{id}
Obter uma transação específica.

#### POST /api/transactions
Criar nova transação.

**Request:**
```json
{
  "description": "Salário",
  "amount": 5000.00,
  "date": "2024-01-15",
  "type": 1
}
```
> Type: 1 = Receita, 2 = Despesa

#### PUT /api/transactions/{id}
Atualizar transação existente.

#### DELETE /api/transactions/{id}
Excluir transação.

## 🧪 Testando a API

Você pode testar a API usando:

### cURL

```bash
# Login
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"demo@minhasfinancas.com","password":"demo123"}'

# Listar transações (substitua {TOKEN} pelo token recebido no login)
curl -X GET https://localhost:7000/api/transactions \
  -H "Authorization: Bearer {TOKEN}"
```

### Postman ou Insomnia

1. Faça login em `/api/auth/login` para obter o token
2. Copie o token da resposta
3. Nas próximas requisições, adicione o header:
   - Key: `Authorization`
   - Value: `Bearer {SEU_TOKEN}`

## 🎨 Interface do Usuário

### Blazor Web App

O aplicativo Blazor oferece:
- Página de login/registro responsiva
- Dashboard de transações com cartões de resumo
- Tabela de transações com opção de exclusão
- Modal para adicionar novas transações
- Design Bootstrap responsivo

### Mobile App (MAUI)

O projeto Mobile inclui:
- ViewModels completos para Login e Transações
- Serviços de autenticação e API
- Armazenamento seguro de tokens
- Arquitetura MVVM
- Exemplos de páginas XAML

## 🛠️ Tecnologias Utilizadas

- **.NET 10** - Framework principal
- **ASP.NET Core** - API e Blazor
- **JWT Bearer Authentication** - Autenticação
- **Blazor Server** - Interface Web
- **MAUI** - Aplicativo Mobile (estrutura)
- **HttpClient** - Comunicação HTTP
- **Bootstrap** - Estilização

## 📝 Próximos Passos

### Melhorias Recomendadas

1. **Banco de Dados**
   - Implementar Entity Framework Core
   - Usar SQL Server, PostgreSQL ou SQLite
   - Migrations para gerenciar schema

2. **Autenticação Avançada**
   - Refresh tokens
   - Esqueci minha senha
   - Confirmação de email
   - Autenticação de dois fatores

3. **Funcionalidades**
   - Categorias de transações
   - Filtros e busca
   - Relatórios e gráficos
   - Exportação de dados (PDF, Excel)
   - Metas financeiras
   - Lembretes e notificações

4. **Mobile**
   - Implementar páginas XAML completas
   - Suporte offline
   - Sincronização automática
   - Notificações push
   - Biometria para login

5. **Infraestrutura**
   - Containerização com Docker
   - CI/CD
   - Deploy em cloud (Azure, AWS)
   - Logs estruturados
   - Monitoramento

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto é de código aberto e está disponível para uso educacional e comercial.

## 👥 Autor

Rodrigo Almeida Francisco

## 📞 Suporte

Para questões ou suporte, abra uma issue no GitHub.

