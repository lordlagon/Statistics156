namespace Statistics156_Front.Data
{
    public class TipoSolicitacao
    {
        public string Fk_Tipo { get; set; }
        public string Tipo { get; set; }
    }

    public class DataCompleta
    {
        public string Fk_data { get; set; }
        public string Datacompleta { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string DiaSemana { get; set; }
        public string Trimestre { get; set; }
        public string Semestre { get; set; }
    }

    public class ConsultaPorTipo
    {
        public string Tipo { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public int Count { get; set; }
    }
    public class CountAno
    {
        public string Ano { get; set; }
        public int Count { get; set; }
        public string Tipo { get; set; }
        public string Mes { get; set; }
    }
    public class TopAssunto
    {
        public string Ano { get; set; }
        public int Count { get; set; }
        public string Assunto { get; set; }
        public string Subdivisao { get; set; }
    }
    public class AssuntoPorBairro
    {
        public int Count { get; set; }
        public string Assunto { get; set; }
    }


    public class FaixaEtaria
    {
        public string Fk_faixa_etaria_ibge { get; set; }
        public string Faixa_etaria { get; set; }
    }


    public class FaixaEtariaGenero
    {
        public int Count { get; set; }
        public string Faixa_etaria { get; set; }
        public string Genero { get; set; }
    }



    public class FaixaEtariaGeneroChart
    {
        public int CountM { get; set; }
        public int CountF { get; set; }
        public string Faixa_etaria { get; set; }
    }


}
