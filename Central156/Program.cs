using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Central156
{
    public class Program
    {
        public string BaseUrl = "http://dadosabertos.c3sl.ufpr.br/curitiba/156/";
        public string DiretorioLocal = @"C:/Sistemas/TCC/156/csv/";

        public static void Main(string[] args)
        {
            var start = new Program();
            start.StartDownLoad();
        }
        public void StartDownLoad()
        {
            var files = GetAllCSV156().ToList();
            var FilesDateNow = GetFileCurrent(files)?.ToList();
            if(FilesDateNow != null)
                FilesDateNow.ForEach(f => files.Add(f));
            DownloadFile(files);
        }
        
        private IEnumerable<string> GetFileCurrent(IEnumerable<string> files)
        {
            var newFiles = new List<string>();
            var date = DateTime.Now.ToString("yyyy-MM");
            if(!files.Where(w => w.Contains(date)).Any())
            {
                return new string[] {
                    $"{date}-01_156_-_Base_de_Dados.csv",
                    $"{date}-01_156_-_Historico_-_Base_de_Dados.csv"
                };
            }
            return null;
        }

        public void DownloadFile(IEnumerable<string> files)
        {
            using (var webClient = new WebClient())
            {
                foreach (var file in files)
                {
                    var url = file.Contains("Historico") ? DiretorioLocal + "Historico/" + file :
                              file.Contains("Dicionario") ? DiretorioLocal + "Dicionario/" + file :
                              DiretorioLocal + "Base/" + file;
                    if (!File.Exists(url))
                        webClient.DownloadFile(new Uri(BaseUrl + file), url);
                }
            }
        }

        public IEnumerable<string> GetAllCSV156()
            => new string[]{
                "156_-_Base_de_Dados.csv",
                "156_-_Dicionario_de_Dados.xlsx",
                "156_-_Historico_-_Base_de_Dados.csv",
                "156_-_Historico_-_Dicionario_de_Dados.xlsx",
                "2015-11-26_156_-_Dicionario_de_Dados.xlsx",
                "2016-02-17_156_-_Historico_-_Dicionario_de_Dados.xlsx",
                "2016-09-01_156_-_Base_de_Dados.csv",
                "2016-09-01_156_-_Historico_-_Base_de_Dados.csv",
                "2016-10-01_156_-_Base_de_Dados.csv",
                "2016-10-01_156_-_Historico_-_Base_de_Dados.csv",
                "2016-11-01_156_-_Base_de_Dados.csv",
                "2016-11-01_156_-_Historico_-_Base_de_Dados.csv",
                "2016-12-01_156_-_Base_de_Dados.csv",
                "2016-12-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-01-01_156_-_Base_de_Dados.csv",
                "2017-01-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-02-01_156_-_Base_de_Dados.csv",
                "2017-02-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-03-01_156_-_Base_de_Dados.csv",
                "2017-03-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-04-01_156_-_Base_de_Dados.csv",
                "2017-04-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-06-01_156_-_Base_de_Dados.csv",
                "2017-06-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-07-01_156_-_Base_de_Dados.csv",
                "2017-07-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-08-01_156_-_Base_de_Dados.csv",
                "2017-08-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-09-01_156_-_Base_de_Dados.csv",
                "2017-09-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-10-01_156_-_Base_de_Dados.csv",
                "2017-10-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-11-01_156_-_Base_de_Dados.csv",
                "2017-11-01_156_-_Historico_-_Base_de_Dados.csv",
                "2017-12-01_156_-_Base_de_Dados.csv",
                "2017-12-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-01-01_156_-_Base_de_Dados.csv",
                "2018-01-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-02-01_156_-_Base_de_Dados.csv",
                "2018-02-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-03-01_156_-_Base_de_Dados.csv",
                "2018-03-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-04-01_156_-_Base_de_Dados.csv",
                "2018-04-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-04-26_156_-_Base_de_Dados.csv",
                "2018-04-26_156_-_Historico_-_Base_de_Dados.csv",
                "2018-05-01_156_-_Base_de_Dados.csv",
                "2018-05-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-06-01_156_-_Base_de_Dados.csv",
                "2018-06-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-07-01_156_-_Base_de_Dados.csv",
                "2018-07-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-08-01_156_-_Base_de_Dados.csv",
                "2018-08-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-09-01_156_-_Base_de_Dados.csv",
                "2018-09-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-10-01_156_-_Base_de_Dados.csv",
                "2018-10-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-11-01_156_-_Base_de_Dados.csv",
                "2018-11-01_156_-_Historico_-_Base_de_Dados.csv",
                "2018-12-01_156_-_Base_de_Dados.csv",
                "2018-12-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-01-01_156_-_Base_de_Dados.csv",
                "2019-01-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-02-01_156_-_Base_de_Dados.csv",
                "2019-02-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-03-01_156_-_Base_de_Dados.csv",
                "2019-03-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-04-01_156_-_Base_de_Dados.csv",
                "2019-04-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-05-01_156_-_Base_de_Dados.csv",
                "2019-05-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-06-01_156_-_Base_de_Dados.csv",
                "2019-06-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-07-01_156_-_Base_de_Dados.csv",
                "2019-07-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-08-01_156_-_Base_de_Dados.csv",
                "2019-08-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-09-01_156_-_Base_de_Dados.csv",
                "2019-09-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-10-01_156_-_Base_de_Dados.csv",
                "2019-10-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-11-01_156_-_Base_de_Dados.csv",
                "2019-11-01_156_-_Historico_-_Base_de_Dados.csv",
                "2019-12-01_156_-_Base_de_Dados.csv",
                "2019-12-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-01-01_156_-_Base_de_Dados.csv",
                "2020-01-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-02-01_156_-_Base_de_Dados.csv",
                "2020-02-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-03-01_156_-_Base_de_Dados.csv",
                "2020-03-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-03-11_156_-_Base_de_Dados.csv",
                "2020-03-11_156_-_Historico_-_Base_de_Dados.csv",
                "2020-04-01_156_-_Base_de_Dados.csv",
                "2020-04-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-05-01_156_-_Base_de_Dados.csv",
                "2020-05-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-06-01_156_-_Base_de_Dados.csv",
                "2020-06-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-07-01_156_-_Base_de_Dados.csv",
                "2020-07-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-08-01_156_-_Base_de_Dados.csv",
                "2020-08-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-09-01_156_-_Base_de_Dados.csv",
                "2020-09-01_156_-_Historico_-_Base_de_Dados.csv",
                "2020-10-01_156_-_Base_de_Dados.csv",
                "2020-10-01_156_-_Historico_-_Base_de_Dados.csv",
            };
    }
}