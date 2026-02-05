# MinhasFinancas.Mobile - MAUI Project

Este projeto contém a estrutura completa para um aplicativo mobile MAUI com autenticação JWT.

## Estrutura do Projeto

```
MinhasFinancas.Mobile/
├── Services/
│   ├── AuthenticationService.cs       # Serviço de autenticação com JWT
│   ├── SecureStorageService.cs        # Armazenamento seguro de tokens
│   └── TransactionService.cs          # Serviço para gerenciar transações
├── ViewModels/
│   ├── LoginViewModel.cs              # ViewModel para tela de login
│   └── TransactionsViewModel.cs       # ViewModel para tela de transações
└── MauiProgram.Example.cs            # Exemplo de configuração do MAUI

```

## Como Usar em um Projeto MAUI Real

### 1. Criar um Novo Projeto MAUI

```bash
dotnet new maui -n MinhasFinancas.MauiApp -o src/MinhasFinancas.MauiApp
```

### 2. Adicionar Referência ao Projeto Mobile

```bash
cd src/MinhasFinancas.MauiApp
dotnet add reference ../MinhasFinancas.Mobile/MinhasFinancas.Mobile.csproj
```

### 3. Configurar MauiProgram.cs

Copie o conteúdo de `MauiProgram.Example.cs` para o seu `MauiProgram.cs` e ajuste conforme necessário.

### 4. Implementar Armazenamento Seguro Real

Substitua `InMemorySecureStorageService` por uma implementação que usa `SecureStorage` do MAUI.Essentials:

```csharp
public class SecureStorageService : ISecureStorageService
{
    public async Task<string?> GetAsync(string key)
    {
        return await SecureStorage.Default.GetAsync(key);
    }

    public async Task SetAsync(string key, string value)
    {
        await SecureStorage.Default.SetAsync(key, value);
    }

    public Task RemoveAsync(string key)
    {
        SecureStorage.Default.Remove(key);
        return Task.CompletedTask;
    }

    public Task<bool> ContainsAsync(string key)
    {
        var value = SecureStorage.Default.GetAsync(key).Result;
        return Task.FromResult(value != null);
    }
}
```

### 5. Criar Páginas XAML

#### LoginPage.xaml (Exemplo)

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MinhasFinancas.Mobile.ViewModels;assembly=MinhasFinancas.Mobile"
             x:Class="MinhasFinancas.MauiApp.Pages.LoginPage"
             Title="Login">
    
    <ContentPage.BindingContext>
        <vm:LoginViewModel />
    </ContentPage.BindingContext>

    <ScrollView>
        <VerticalStackLayout Padding="30" Spacing="20">
            <Label Text="Minhas Finanças"
                   FontSize="32"
                   FontAttributes="Bold"
                   HorizontalOptions="Center" />

            <Entry Placeholder="Email"
                   Text="{Binding Email}"
                   Keyboard="Email" />

            <Entry Placeholder="Senha"
                   Text="{Binding Password}"
                   IsPassword="True" />

            <Entry Placeholder="Nome (apenas para registro)"
                   Text="{Binding Name}"
                   IsVisible="{Binding IsRegisterMode}" />

            <Label Text="{Binding ErrorMessage}"
                   TextColor="Red"
                   IsVisible="{Binding ErrorMessage, Converter={StaticResource StringToBoolConverter}}" />

            <Label Text="{Binding SuccessMessage}"
                   TextColor="Green"
                   IsVisible="{Binding SuccessMessage, Converter={StaticResource StringToBoolConverter}}" />

            <Button Text="{Binding IsRegisterMode, Converter={StaticResource LoginRegisterTextConverter}}"
                    Command="{Binding LoginCommand}"
                    IsEnabled="{Binding IsLoading, Converter={StaticResource InverseBoolConverter}}" />

            <Button Text="{Binding IsRegisterMode, Converter={StaticResource ToggleModeTextConverter}}"
                    Command="{Binding ToggleModeCommand}"
                    BackgroundColor="Transparent"
                    TextColor="{StaticResource Primary}" />

            <ActivityIndicator IsRunning="{Binding IsLoading}"
                             IsVisible="{Binding IsLoading}" />
        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

#### TransactionsPage.xaml (Exemplo)

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MinhasFinancas.Mobile.ViewModels;assembly=MinhasFinancas.Mobile"
             x:Class="MinhasFinancas.MauiApp.Pages.TransactionsPage"
             Title="Transações">
    
    <ContentPage.BindingContext>
        <vm:TransactionsViewModel />
    </ContentPage.BindingContext>

    <Grid RowDefinitions="Auto,*">
        <!-- Summary Cards -->
        <Grid Grid.Row="0" ColumnDefinitions="*,*,*" Padding="10">
            <Frame Grid.Column="0" BackgroundColor="Green" CornerRadius="10">
                <VerticalStackLayout>
                    <Label Text="Receitas" FontSize="14" TextColor="White" />
                    <Label Text="{Binding TotalReceitas, StringFormat='R$ {0:N2}'}" 
                           FontSize="20" FontAttributes="Bold" TextColor="White" />
                </VerticalStackLayout>
            </Frame>
            
            <Frame Grid.Column="1" BackgroundColor="Red" CornerRadius="10">
                <VerticalStackLayout>
                    <Label Text="Despesas" FontSize="14" TextColor="White" />
                    <Label Text="{Binding TotalDespesas, StringFormat='R$ {0:N2}'}" 
                           FontSize="20" FontAttributes="Bold" TextColor="White" />
                </VerticalStackLayout>
            </Frame>
            
            <Frame Grid.Column="2" BackgroundColor="Blue" CornerRadius="10">
                <VerticalStackLayout>
                    <Label Text="Saldo" FontSize="14" TextColor="White" />
                    <Label Text="{Binding Saldo, StringFormat='R$ {0:N2}'}" 
                           FontSize="20" FontAttributes="Bold" TextColor="White" />
                </VerticalStackLayout>
            </Frame>
        </Grid>

        <!-- Transactions List -->
        <CollectionView Grid.Row="1" ItemsSource="{Binding Transactions}">
            <CollectionView.ItemTemplate>
                <DataTemplate>
                    <SwipeView>
                        <SwipeView.RightItems>
                            <SwipeItems>
                                <SwipeItem Text="Deletar"
                                          BackgroundColor="Red"
                                          Command="{Binding Source={RelativeSource AncestorType={x:Type vm:TransactionsViewModel}}, Path=DeleteTransactionCommand}"
                                          CommandParameter="{Binding Id}" />
                            </SwipeItems>
                        </SwipeView.RightItems>
                        
                        <Frame Margin="10,5" Padding="10" CornerRadius="10">
                            <Grid ColumnDefinitions="*,Auto">
                                <VerticalStackLayout Grid.Column="0">
                                    <Label Text="{Binding Description}" FontAttributes="Bold" />
                                    <Label Text="{Binding Date, StringFormat='{0:dd/MM/yyyy}'}" FontSize="12" />
                                </VerticalStackLayout>
                                <VerticalStackLayout Grid.Column="1" HorizontalOptions="End">
                                    <Label Text="{Binding Amount, StringFormat='R$ {0:N2}'}" FontAttributes="Bold" />
                                    <Label Text="{Binding Type}" FontSize="12" />
                                </VerticalStackLayout>
                            </Grid>
                        </Frame>
                    </SwipeView>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
</ContentPage>
```

## Funcionalidades Implementadas

### Autenticação
- ✅ Login com email e senha
- ✅ Registro de novos usuários
- ✅ Armazenamento seguro de tokens JWT
- ✅ Logout com limpeza de tokens
- ✅ Verificação de autenticação

### Transações
- ✅ Listagem de transações do usuário
- ✅ Criação de novas transações
- ✅ Exclusão de transações
- ✅ Cálculo de totais (Receitas, Despesas, Saldo)
- ✅ Autenticação automática nas requisições

### Segurança
- ✅ Token JWT em todas as requisições à API
- ✅ Armazenamento seguro de credenciais
- ✅ Renovação automática de headers de autorização

## Configuração da API

Atualize a URL da API no `MauiProgram.cs`:

```csharp
var apiBaseUrl = "https://your-api-url.com"; // Substitua pela URL da sua API
```

Para desenvolvimento local no Android:
```csharp
var apiBaseUrl = "http://10.0.2.2:7000"; // Emulador Android
```

Para desenvolvimento local no iOS:
```csharp
var apiBaseUrl = "https://localhost:7000"; // iOS Simulator
```

## Testando a Aplicação

1. Inicie a API primeiro
2. Configure a URL da API no projeto mobile
3. Execute o aplicativo MAUI
4. Use as credenciais padrão para testar:
   - Email: demo@minhasfinancas.com
   - Senha: demo123

## Próximos Passos

1. Implementar as páginas XAML reais
2. Adicionar comandos nos ViewModels (usando CommunityToolkit.Mvvm)
3. Implementar navegação entre páginas
4. Adicionar validação de formulários
5. Implementar refresh de tokens
6. Adicionar suporte offline
7. Implementar sincronização de dados
