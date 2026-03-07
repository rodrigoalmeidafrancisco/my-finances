using Developer.ExtensionCore;
using Developer.HttpCore;
using Domain.Contracts.Servicies;
using Domain.Models.DTOs.ViaCep;
using Shared.Settings;
using System.Net;

namespace Data.Servicies
{
    public class ServiceViaCep : IServiceViaCep
    {
        private readonly BaseHttpHandler _baseHttpHandler;

        public ServiceViaCep()
        {
            _baseHttpHandler = new BaseHttpHandler(SettingApp.Services.ViaCep._LinkBase);
        }

        /// <summary>
        /// Obtém o endereço completo a partir do CEP informado.
        /// </summary>
        /// <param name="cep">O CEP do endereço.</param>
        /// <returns>O endereço correspondente ao CEP informado.</returns>
        public async Task<DtoViaCep_Endereco> GetAddressByCep(string cep)
        {
            DtoViaCep_Endereco endereco = null;
            ResultHttp response = await _baseHttpHandler.GetAsync($"{cep}/json");

            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                endereco = response.DataString.ToNewtonsoftDeserializeJson<DtoViaCep_Endereco>();
                endereco = endereco == null || endereco.Erro ? null : endereco;
            }

            return endereco;
        }

        /// <summary>
        /// Obtém uma lista de endereços a partir do logradouro, cidade e estado informados.
        /// </summary>
        /// <param name="uf">O estado (UF) do endereço.</param>
        /// <param name="cidade">A cidade do endereço.</param>
        /// <param name="logradouro">O logradouro do endereço.</param>
        /// <returns>Uma lista de endereços que correspondem aos parâmetros informados.</returns>
        public async Task<List<DtoViaCep_Endereco>> GetAddressByLogradouro(string uf, string cidade, string logradouro)
        {
            List<DtoViaCep_Endereco> endereco = null;
            ResultHttp response = await _baseHttpHandler.GetAsync($"{uf}/{cidade}/{logradouro}/json");

            if (response.HttpStatusCode.Equals(HttpStatusCode.OK))
            {
                endereco = response.DataString.ToNewtonsoftDeserializeJson<List<DtoViaCep_Endereco>>();
                endereco = endereco == null || endereco.Count == 0 ? [] : endereco;
            }

            return endereco;
        }

    }
}
