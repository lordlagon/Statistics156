using Flurl;
using Statistics156_Front.Data;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics156_Front.Services
{
    public interface ICentralService
    {
        Task<List<CountAno>> GetConsultaPorTiposAsync(string tipo, string mes);
        Task<List<CountAno>> GetVarSolicitacaoMes();
        Task<List<TopAssunto>> GetTop10Assunto();
        Task<List<TopAssunto>> GetTop10Assunto(BairroSolicitacao bairro);
        Task<List<AssuntoPorBairro>> GetBairroAssuntoAno(string assunto, string ano);
        Task<Chart4BairrosAssunto> GetAssuntoBairros(List<BairroSolicitacao> bairros, AssuntoSolicitacao assunto);
        Task<List<BairroPorAssunto>> QuantidadeSolicitacaoPorBairro(string assunto, string mes, string ano);
        Task<List<DataAssuntoPorBairro>> GetDataAssuntoPorBairro(string assunto, string bairro);
        Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGenero(List<FaixaEtaria> faixasEtarias);
        Task<List<ChartFaixaEtaria>> GetFaixaEtariaGenero(RegionalSolicitacao regional);
        Task<List<ChartFaixaEtaria>> GetFaixaEtariaGenero(RegionalSolicitacao regional, string mes, string ano);
        Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGenero(string assunto, List<FaixaEtaria> faixasEtarias);
        Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGeneroRegional(RegionalSolicitacao regional, List<FaixaEtaria> faixasEtarias);
    }
    public class CentralService : ICentralService
    {
        readonly ISelecoesService _selecoesService;
        public CentralService(ISelecoesService selecoesService)
        {
            _selecoesService = selecoesService;
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
        public async Task<List<TopAssunto>> GetTop10Assunto(BairroSolicitacao bairro)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("topassuntos")
                    .AppendPathSegment("bairro")
                    .AppendPathSegment(bairro.Fk_bairro)
                    .GetJsonAsync<List<TopAssunto>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<List<AssuntoPorBairro>> GetBairroAssuntoAno(string bairro, string ano)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("bairroAssunto")
                    .AppendPathSegment(bairro)
                    .AppendPathSegment(ano)
                    .GetJsonAsync<List<AssuntoPorBairro>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<BairroPorAssunto>> QuantidadeSolicitacaoPorBairro(string assunto, string mes, string ano)
        {
            try
            {
                assunto = string.IsNullOrEmpty(assunto) ? "67" : assunto;
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("assuntoBairroMes")
                    .AppendPathSegment(assunto)
                    .AppendPathSegment(mes)
                    .AppendPathSegment(ano)
                    .GetJsonAsync<List<BairroPorAssunto>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Chart4BairrosAssunto> GetAssuntoBairros(List<BairroSolicitacao> bairros, AssuntoSolicitacao assunto)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("bairros")
                    .AppendPathSegment(assunto.Fk_Assunto)
                    .AppendPathSegment(bairros[0].Fk_bairro)
                    .AppendPathSegment(bairros[1].Fk_bairro)
                    .AppendPathSegment(bairros[2].Fk_bairro)
                    .GetJsonAsync<List<BairrosAssunto>>();

                if (result != null)
                    return GetChartAssuntoBairro(result, bairros);
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private Chart4BairrosAssunto GetChartAssuntoBairro(List<BairrosAssunto> result, List<BairroSolicitacao> bairros)
        {
            var Junho = new ChartAssuntoBairros();
            var Julho = new ChartAssuntoBairros();
            var Agosto = new ChartAssuntoBairros();
            var Setembro = new ChartAssuntoBairros();
            var list = new List<ChartAssuntoBairros>();

            foreach (var item in result)
            {
                if (item.Mes == "Jun")
                {
                    Junho.Mes = "Junho";
                    if (item.FK_Bairro.ToString() == bairros[0].Fk_bairro)
                    {
                        Junho.Bairro1 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[1].Fk_bairro)
                    {
                        Junho.Bairro2 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[2].Fk_bairro)
                    {
                        Junho.Bairro3 = item.Count;
                    }
                }
                if (item.Mes == "Jul")
                {
                    Julho.Mes = "Julho";
                    if (item.FK_Bairro.ToString() == bairros[0].Fk_bairro)
                    {
                        Julho.Bairro1 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[1].Fk_bairro)
                    {
                        Julho.Bairro2 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[2].Fk_bairro)
                    {
                        Julho.Bairro3 = item.Count;
                    }
                }
                if (item.Mes == "Ago")
                {
                    Agosto.Mes = "Agosto";
                    if (item.FK_Bairro.ToString() == bairros[0].Fk_bairro)
                    {
                        Agosto.Bairro1 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[1].Fk_bairro)
                    {
                        Agosto.Bairro2 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[2].Fk_bairro)
                    {
                        Agosto.Bairro3 = item.Count;
                    }
                }
                if (item.Mes == "Set")
                {
                    Setembro.Mes = "Setembro";
                    if (item.FK_Bairro.ToString() == bairros[0].Fk_bairro)
                    {
                        Setembro.Bairro1 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[1].Fk_bairro)
                    {
                        Setembro.Bairro2 = item.Count;
                    }
                    if (item.FK_Bairro.ToString() == bairros[2].Fk_bairro)
                    {
                        Setembro.Bairro3 = item.Count;
                    }
                }
            }
            list.Add(Junho);
            list.Add(Julho);
            list.Add(Agosto);
            list.Add(Setembro);
            return new Chart4BairrosAssunto()
            {
                Bairro1 = bairros[0].Bairro,
                Bairro2 = bairros[1].Bairro,
                Bairro3 = bairros[2].Bairro,
                ListaCharts = list
            };
        }

        public async Task<List<DataAssuntoPorBairro>> GetDataAssuntoPorBairro(string assunto, string bairro)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("dataAssuntoBairro")
                    .AppendPathSegment(assunto)
                    .AppendPathSegment(bairro)
                    .GetJsonAsync<List<DataAssuntoPorBairro>>();

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<ChartFaixaEtaria>> GetFaixaEtariaGenero(RegionalSolicitacao regional)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .AppendPathSegment(regional.Fk_Regional)
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<ChartFaixaEtaria>();

                var faixasEtarias = await _selecoesService.GetFaixasEtariasAsync();
                foreach (var item in faixasEtarias)
                {
                    var t = result.FindAll(f => f.Faixa_etaria == item.Faixa_etaria);
                    var Fe = item.Faixa_etaria;
                    var cM = t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe).Count : 0;
                    var cF = t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe).Count : 0;

                    list.Add(new ChartFaixaEtaria()
                    {
                        Faixa_etaria = Fe,
                        Male = cM,
                        Female = cF
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
        public async Task<List<ChartFaixaEtaria>> GetFaixaEtariaGenero(RegionalSolicitacao regional, string mes, string ano)
        {
            try
            {
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .AppendPathSegment(regional.Fk_Regional)
                    .AppendPathSegment(mes)
                    .AppendPathSegment(ano)
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<ChartFaixaEtaria>();

                var faixasEtarias = await _selecoesService.GetFaixasEtariasAsync();
                foreach (var item in faixasEtarias)
                {
                    var t = result.FindAll(f => f.Faixa_etaria == item.Faixa_etaria);
                    var Fe = item.Faixa_etaria;
                    var cM = t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "M" && w.Faixa_etaria == Fe).Count : 0;
                    var cF = t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe) != null ? t.FirstOrDefault(w => w.Genero == "F" && w.Faixa_etaria == Fe).Count : 0;

                    list.Add(new ChartFaixaEtaria()
                    {
                        Faixa_etaria = Fe,
                        Male = cM,
                        Female = cF
                    });
                    list.OrderByDescending(o => o.Faixa_etaria);
                }
                var i = list.Where(w => w.Faixa_etaria == "unknown").FirstOrDefault();
                list.Remove(i);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    
        public async Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGeneroRegional(RegionalSolicitacao regional, List<FaixaEtaria> faixasEtarias)
        {
            try
            {
                if (faixasEtarias == null || !faixasEtarias.Any())
                    faixasEtarias = await _selecoesService.GetFaixasEtariasAsync();
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .AppendPathSegment("regional")
                    .AppendPathSegment(regional.Fk_Regional)
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<FaixaEtariaGeneroChart>();

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

        public async Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGenero(string assunto, List<FaixaEtaria> faixasEtarias)
        {
            try
            {
                if (faixasEtarias == null || !faixasEtarias.Any())
                    faixasEtarias = await _selecoesService.GetFaixasEtariasAsync();
                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .AppendPathSegment(assunto)
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<FaixaEtariaGeneroChart>();

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
        public async Task<List<FaixaEtariaGeneroChart>> GetFaixaEtariaGenero(List<FaixaEtaria> faixasEtarias)
        {
            try
            {
                if (faixasEtarias == null || !faixasEtarias.Any())
                    faixasEtarias = await _selecoesService.GetFaixasEtariasAsync();

                var result = await AppConfiguration.BaseUrl
                    .AppendPathSegment("analises")
                    .AppendPathSegment("faixaetariagenero")
                    .GetJsonAsync<List<FaixaEtariaGenero>>();
                var list = new List<FaixaEtariaGeneroChart>();

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
