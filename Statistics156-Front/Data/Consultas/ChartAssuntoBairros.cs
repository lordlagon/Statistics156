using System.Collections.Generic;

namespace Statistics156_Front.Data
{
    public class ChartAssuntoBairros
    {
        public string Mes { get; set; }
        public int Bairro1 { get; set; }
        public int Bairro2 { get; set; }
        public int Bairro3 { get; set; }
    }
    public class Chart4BairrosAssunto 
    {
        public string Bairro1 { get; set; }
        public string Bairro2 { get; set; }
        public string Bairro3 { get; set; }
       public List<ChartAssuntoBairros> ListaCharts { get; set; }
    }
    public class BairrosAssunto
    {
        public int FK_Bairro { get; set; }
        public string Mes { get; set; }
        public int Count { get; set; }
    }

}
