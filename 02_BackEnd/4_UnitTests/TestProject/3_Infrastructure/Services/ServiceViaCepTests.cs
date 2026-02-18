using Data.Servicies;

namespace TestProject
{
    public class ServiceViaCepTests
    {
        private ServiceViaCep _serviceViaCep;

        [SetUp]
        public void Setup()
        {
            SetupTests.InitializeSettingApp();
            _serviceViaCep = new ServiceViaCep();
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByCep retorna um endereço válido quando um CEP válido é fornecido. 
        /// Ele valida se o resultado não é nulo, se o CEP, logradouro, UF e localidade estão corretos para o CEP testado (Avenida Paulista, São Paulo).
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByCep_WhenCepIsValid_ShouldReturnAddress()
        {
            // Arrange
            var cep = "01310100"; // CEP da Avenida Paulista, São Paulo

            // Act
            var result = await _serviceViaCep.GetAddressByCep(cep);

            // Assert
            Assert.That(result, Is.Not.Null, "O resultado não deveria ser nulo para um CEP válido");
            Assert.That(result.CEP, Is.Not.Null.And.Not.Empty, "O CEP não deveria estar vazio");
            Assert.That(result.Logradouro, Is.Not.Null.And.Not.Empty, "O logradouro não deveria estar vazio");
            Assert.That(result.UF, Is.EqualTo("SP"), "O UF deveria ser SP");
            Assert.That(result.Localidade, Is.EqualTo("São Paulo"), "A localidade deveria ser São Paulo");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByCep retorna nulo quando um CEP inválido é fornecido. 
        /// Ele valida se o resultado é nulo para um CEP que não existe ou tem formato incorreto.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByCep_WhenCepIsInvalid_ShouldReturnNull()
        {
            // Arrange
            var cep = "00000000"; // CEP inválido

            // Act
            var result = await _serviceViaCep.GetAddressByCep(cep);

            // Assert
            Assert.That(result, Is.Null, "O resultado deveria ser nulo para um CEP inválido");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByCep retorna nulo quando um CEP com formato inválido é fornecido.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByCep_WhenCepHasInvalidFormat_ShouldReturnNull()
        {
            // Arrange
            var cep = "12345"; // CEP com formato inválido

            // Act
            var result = await _serviceViaCep.GetAddressByCep(cep);

            // Assert
            Assert.That(result, Is.Null, "O resultado deveria ser nulo para um CEP com formato inválido");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByCep retorna nulo quando um CEP que não existe é fornecido.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByCep_WhenCepNotFound_ShouldReturnNull()
        {
            // Arrange
            var cep = "99999999"; // CEP que não existe

            // Act
            var result = await _serviceViaCep.GetAddressByCep(cep);

            // Assert
            Assert.That(result, Is.Null, "O resultado deveria ser nulo para um CEP não encontrado");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByLogradouro retorna uma lista de endereços válidos quando parâmetros válidos são fornecidos.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByLogradouro_WhenParametersAreValid_ShouldReturnAddress()
        {
            // Arrange
            var uf = "SP";
            var cidade = "São Paulo";
            var logradouro = "Paulista";

            // Act
            var result = await _serviceViaCep.GetAddressByLogradouro(uf, cidade, logradouro);

            // Assert
            Assert.That(result, Is.Not.Null, "O resultado não deveria ser nulo para parâmetros válidos");
            Assert.That(result, Is.Not.Empty, "O resultado não deveria estar vazio para parâmetros válidos");
            Assert.That(result[0].UF, Is.EqualTo("SP"), "O UF deveria ser SP");
            Assert.That(result[0].Localidade, Is.EqualTo("São Paulo"), "A localidade deveria ser São Paulo");
            Assert.That(result[0].Logradouro, Does.Contain("Paulista").IgnoreCase, "O logradouro deveria conter 'Paulista'");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByLogradouro retorna nulo ou vazio quando um logradouro que não existe é fornecido.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByLogradouro_WhenLogradouroNotFound_ShouldReturnNull()
        {
            // Arrange
            var uf = "SP";
            var cidade = "São Paulo";
            var logradouro = "XYZ123LogradouroInexistente";

            // Act
            var result = await _serviceViaCep.GetAddressByLogradouro(uf, cidade, logradouro);

            // Assert
            Assert.That(result, Is.Null.Or.Empty, "O resultado deveria ser nulo ou vazio para logradouro não encontrado");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByLogradouro retorna nulo ou vazio quando um UF inválido é fornecido.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByLogradouro_WhenUFIsInvalid_ShouldReturnNull()
        {
            // Arrange
            var uf = "XX"; // UF inválido
            var cidade = "Cidade";
            var logradouro = "Rua";

            // Act
            var result = await _serviceViaCep.GetAddressByLogradouro(uf, cidade, logradouro);

            // Assert
            Assert.That(result, Is.Null.Or.Empty, "O resultado deveria ser nulo ou vazio para UF inválido");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByLogradouro retorna nulo ou vazio quando uma cidade que não existe é fornecida.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByLogradouro_WhenCityNotFound_ShouldReturnNull()
        {
            // Arrange
            var uf = "SP";
            var cidade = "CidadeInexistente123XYZ";
            var logradouro = "Rua";

            // Act
            var result = await _serviceViaCep.GetAddressByLogradouro(uf, cidade, logradouro);

            // Assert
            Assert.That(result, Is.Null.Or.Empty, "O resultado deveria ser nulo ou vazio para cidade não encontrada");
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByCep retorna um endereço válido quando um CEP formatado com hífen é fornecido.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByCep_WhenCepIsFormatted_ShouldReturnAddress()
        {
            // Arrange
            var cep = "01310-100"; // CEP formatado com hífen

            // Act
            var result = await _serviceViaCep.GetAddressByCep(cep);

            // Assert
            Assert.That(result, Is.Not.Null, "O resultado não deveria ser nulo para um CEP formatado válido");
            Assert.That(result.UF, Is.EqualTo("SP"));
        }

        /// <summary>
        /// Este teste verifica se o método GetAddressByLogradouro retorna a primeira correspondência correta quando múltiplos endereços correspondem aos parâmetros fornecidos.
        /// </summary>
        /// <returns></returns>
        [Test]
        [Category("Integration")]
        public async Task GetAddressByLogradouro_WhenMultipleResults_ShouldReturnFirstMatch()
        {
            // Arrange
            var uf = "RS";
            var cidade = "Porto Alegre";
            var logradouro = "Santos";

            // Act
            var result = await _serviceViaCep.GetAddressByLogradouro(uf, cidade, logradouro);

            // Assert
            Assert.That(result, Is.Not.Null, "O resultado não deveria ser nulo quando há múltiplas correspondências");
            Assert.That(result, Is.Not.Empty, "O resultado não deveria estar vazio quando há múltiplas correspondências");
            Assert.That(result[0].UF, Is.EqualTo("RS"));
            Assert.That(result[0].Localidade, Is.EqualTo("Porto Alegre"));
        }

        /// <summary>
        /// Limpa recursos e reseta a instância do serviço após cada execução de teste.
        /// </summary>
        /// <remarks>Este método é chamado automaticamente após cada método de teste para garantir um estado limpo para os testes subsequentes.</remarks>
        [TearDown]
        public void TearDown()
        {
            _serviceViaCep = null;
        }
    }
}
