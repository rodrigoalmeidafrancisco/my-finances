using Domain.Models.DTOs.ViaCep;

namespace Domain.Contracts.Servicies
{
    public interface IServiceViaCep
    {
        Task<DtoViaCep_Endereco> GetAddressByCep(string cep);
        Task<List<DtoViaCep_Endereco>> GetAddressByLogradouro(string uf, string cidade, string logradouro);
    }
}
