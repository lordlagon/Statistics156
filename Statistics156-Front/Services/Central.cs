using Flurl;
using Statistics156_Front.Data;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics156_Front.Services
{
    public class Central
    {

        public async Task<List<CountryInfo>> GetCentralAsync()
        {
            var person = await AppConfiguration.BaseUrl

                .AppendPathSegment("country").GetJsonAsync<List<CountryInfo>>();

            return person;
        }

        public async Task<List<TipoSolicitacao>> GetTiposAsync()
        {
            var tipos = await AppConfiguration.BaseUrl
                .AppendPathSegment("selecao")
                .AppendPathSegment("tipos")
                .GetJsonAsync<List<TipoSolicitacao>>();

            return tipos;
        }
        public async Task<List<ConsultaPorTipo>> GetConsultaPorTiposAsync(string tipo, string mes)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("consulta")
                    .AppendPathSegment(tipo)
                    .AppendPathSegment(mes)
                    .GetJsonAsync<List<ConsultaPorTipo>>();

                return result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
