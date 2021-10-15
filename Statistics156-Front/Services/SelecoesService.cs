using Flurl;
using Statistics156_Front.Data;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Statistics156_Front.Services
{
    public interface ISelecoesService
    {
        Task<List<TipoSolicitacao>> GetTiposAsync();
        List<BairroSolicitacao> GetBairrosAsync();
        Task<List<RegionalSolicitacao>> GetRegionaisAsync();
        Task<List<AssuntoSolicitacao>> GetAssuntosAsync();
        Task<List<SubdivisaoSolicitacao>> GetSubdivisoesAsync();
        Task<List<FaixaEtaria>> GetFaixasEtariasAsync();
        string[] GetMeses();
        string[] GetAnos();
    }
    public class SelecoesService : ISelecoesService
    {
        const string bairroJson = "C:\\Sistemas\\TCC\\Central156\\Statistics156-Front\\Data\\bairros.json";
        public async Task<List<AssuntoSolicitacao>> GetAssuntosAsync()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("selecao")
                    .AppendPathSegment("assuntos")
                    .GetJsonAsync<List<AssuntoSolicitacao>>();

                result.Remove(result.FirstOrDefault(w => w.Assunto == "NA"));

                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public List<BairroSolicitacao> GetBairrosAsync()
        {
            try
            {
                var bairros = LoadJson<BairroSolicitacao>(bairroJson);
                return bairros;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<RegionalSolicitacao>> GetRegionaisAsync()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("selecao")
                    .AppendPathSegment("regionais")
                    .GetJsonAsync<List<RegionalSolicitacao>>();

                result.Remove(result.FirstOrDefault(w => w.Regional == "NA"));

                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<SubdivisaoSolicitacao>> GetSubdivisoesAsync()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("selecao")
                    .AppendPathSegment("subdivisao")
                    .GetJsonAsync<List<SubdivisaoSolicitacao>>();

                result.Remove(result.FirstOrDefault(w => w.Subdivisao == "NA"));

                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<TipoSolicitacao>> GetTiposAsync()
        {
            try
            {
                var tipos = await AppConfiguration.BaseUrl
                    .AppendPathSegment("selecao")
                    .AppendPathSegment("tipos")
                    .GetJsonAsync<List<TipoSolicitacao>>();

                tipos.Remove(tipos.FirstOrDefault(w => w.Tipo == "NA"));

                return tipos;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<FaixaEtaria>> GetFaixasEtariasAsync()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("selecao")
                    .AppendPathSegment("faixa_etaria")
                    .GetJsonAsync<List<FaixaEtaria>>();
                result.Remove(result.FirstOrDefault(w => w.Faixa_etaria == "unknown"));

                return result;

            }
            catch (Exception)
            {

                return null;
            }
        }
        public List<T> LoadJson<T>(string fileJson)
        {
            using StreamReader r = new(fileJson);
            return JsonConvert.DeserializeObject<List<T>>(r.ReadToEnd());
        }

        public string[] GetMeses()
        {
            var meses = new string[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
            return meses;
        }
        public string[] GetAnos()
        {
            var meses = new string[] { "2014", "2015", "2016", "2017", "2018", "2019", "2020", "2021" };
            return meses;
        }
    }
}
