using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics156_Front.Data
{
    public partial class CountryInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalCases { get; set; }
        public int TotalDeaths { get; set; }
        public string DeathPercent => (this.TotalDeaths * 100) / this.TotalCases + "%";
        public List<CountryInfo> GetCountryInfos()
        {
            var countryInfos = new List<CountryInfo>
            {
                new CountryInfo { Id = 1, Name = "Usa", TotalCases = 142178, TotalDeaths = 22484 },
                new CountryInfo { Id = 2, Name = "Italy", TotalCases = 14218, TotalDeaths = 2484 },
                new CountryInfo { Id = 3, Name = "brasil", TotalCases = 1428, TotalDeaths = 234 },
                new CountryInfo { Id = 4, Name = "russia", TotalCases = 1422178, TotalDeaths = 92484 },
                new CountryInfo { Id = 5, Name = "china", TotalCases = 1421, TotalDeaths = 284 },
                new CountryInfo { Id = 6, Name = "paragua", TotalCases = 173248, TotalDeaths = 24864 },
                new CountryInfo { Id = 7, Name = "uruguai", TotalCases = 17238, TotalDeaths = 24384 }
            };
            return countryInfos;
        }
    }
}
