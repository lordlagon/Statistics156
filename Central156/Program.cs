using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

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

        public string GetDirectoryListingRegexForUrl(string url)
        {
            if (url.Equals(BaseUrl))
                return "<a href=\".*\">(?<name>.*)</a>";
            throw new NotSupportedException();
        }
        
        public void StartDownLoad()
        {
            var files = GetAllFileNamesCSV156().ToList();
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
            using var webClient = new WebClient();

            foreach (var file in files)
            {
                var url = file.Contains("Historico") ? DiretorioLocal + "Historico/" + file :
                          file.Contains("Dicionario") ? DiretorioLocal + "Dicionario/" + file :
                          DiretorioLocal + "Base/" + file;
                if (!File.Exists(url))
                    webClient.DownloadFile(new Uri(BaseUrl + file), url);
            }
        }
        public IEnumerable<string> GetAllFileNamesCSV156()
        {
            string url = $"{BaseUrl}";
            var names = new List<string>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using StreamReader reader = new StreamReader(response.GetResponseStream());
                string html = reader.ReadToEnd();
                Regex regex = new Regex(GetDirectoryListingRegexForUrl(url));
                MatchCollection matches = regex.Matches(html);
                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            if (match.Value.Contains(".csv"))
                                names.Add(match.Groups["name"].ToString());
                        }
                    }
                }
            }
            return names;
        }
    }
}