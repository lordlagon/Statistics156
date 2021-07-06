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
        public string[] GetMeses()
        {
            var meses = new string[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez" };
            return meses;
        }
        public async Task<List<CountAno>> GetConsultaPorTiposAsync(string tipo, string mes)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("countano")
                    .AppendPathSegment(tipo)
                    .AppendPathSegment(mes)
                    .GetJsonAsync<List<CountAno>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<CountAno>> GetVarSolicitacaoMes()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("varsolicitacaomes")
                    .GetJsonAsync<List<CountAno>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<TopAssunto>> GetTop10Assunto()
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("topassuntos")
                    .GetJsonAsync<List<TopAssunto>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<AssuntoPorBairro>> GetAssuntoPorBairro(string assunto, string ano)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("bairroAssunto")
                    .AppendPathSegment(assunto)
                    .AppendPathSegment(ano)
                    .GetJsonAsync<List<AssuntoPorBairro>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGenero(string assunto)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .AppendPathSegment(assunto)
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<FaixaEtariaGeneroChart>();

                var faixasEtarias = await GetFaixasEtariasAsync();
                foreach (var item in faixasEtarias)
                {
                    var t = result.FindAll(f => f.Faixa_etaria == item.Faixa_etaria);
                    var Fe = item.Faixa_etaria;
                    var cM = t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe).Count : 0;
                    var cF = t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe).Count : 0;
                    
                    list.Add(new FaixaEtariaGeneroChart()
                    {
                        Faixa_etaria = Fe,
                        CountF = cF,
                        CountM = cM
                    });
                    list.OrderByDescending(o => o.Faixa_etaria);
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
