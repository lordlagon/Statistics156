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
            var person = await "http://127.0.0.1:5000/"
                .AppendPathSegment("api")
                .AppendPathSegment("tasks").GetJsonAsync<List<CountryInfo>>();
                
            return person;
        }
    }
}
