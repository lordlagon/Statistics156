namespace Statistics156_Front.Data
{
    public class TipoSolicitacao
    {
        public string Fk_Tipo { get; set; }
        public string Tipo { get; set; }
    }

    public class ConsultaPorTipo
    {
        public string Tipo { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public int Count { get; set; }
    }

}
