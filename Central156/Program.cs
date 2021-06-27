using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Central156
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var start = new DownloadFiles();
            start.StartDownLoadAndRead();
        }
    }
    public class DownloadFiles
    {

        public List<string> ListNameFiles { get; set; }
        public DownloadFiles()
        {
            ListNameFiles = new List<string>();
        }
        public void StartDownLoadAndRead()
        {
            var files = GetAllFileNamesCSV156().ToList();
            var FilesDateNow = GetFileCurrent(files)?.ToList();
            if (FilesDateNow != null)
                FilesDateNow.ForEach(f => files.Add(f));
            //DownloadFile(files);
            ReadAllFiles(files);
        }
        private IEnumerable<string> GetFileCurrent(IEnumerable<string> files)
        {
            var newFiles = new List<string>();
            var date = DateTime.Now.ToString("yyyy-MM");
            if (!files.Where(w => w.Contains(date)).Any())
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
                var url = file.Contains("Historico") ? AppConfiguration.DiretorioLocal + "Historico/" + file :
                          file.Contains("Dicionario") ? AppConfiguration.DiretorioLocal + "Dicionario/" + file :
                          AppConfiguration.DiretorioLocal + "Base156/" + file;
                if (!File.Exists(url))
                {
                    webClient.DownloadFile(new Uri(AppConfiguration.BaseUrl + file), url);
                    Console.WriteLine($"{file}: Complete");
                }
            }
        }
        #region Read
        public void ReadAllFiles(IEnumerable<string> files)
        {
            int totalErros = 0;
            int totalLinhas = 0;
            int totalEntradas = 0;

            foreach (var file in files)
            {
                var url = file.Contains("Historico") ? AppConfiguration.DiretorioLocal + "Historico/" + file :
                          file.Contains("Dicionario") ? AppConfiguration.DiretorioLocal + "Dicionario/" + file :
                          AppConfiguration.DiretorioLocal + "Base156/" + file;
                if (File.Exists(url) && url.Contains("Base156/"))
                {
                    // if (url.Contains("2021-01-01_156_-_Base_de_Dados"))
                    //{
                    var (entradas, saidas, Erros) = ReadFile(url, file);
                    totalErros += Erros;
                    totalLinhas += saidas;
                    totalEntradas += entradas;
                    //}
                }
            }
            Console.WriteLine($"Entrada inicial = {totalEntradas} - Saídas Modificadas = {totalLinhas} - Total de Erros={totalErros}");
        }

        public (int entradas, int saidas, int Erros) ReadFile(string patchFile, string fileName)
        {
            var list = new List<string>();
            int countError = 0;
            int count = 0;
            var linhas = File.ReadAllLines(patchFile, Encoding.GetEncoding("ISO-8859-1"));
            string quote = "\"";

            foreach (var item in linhas)
            {
                var linha = RemoverAcentos(item);
                if (linha.StartsWith("-------"))
                    continue;
                
                if (linha.StartsWith("esta registrado no croqui 4135\t"))
                {
                    linha = linha.Replace("\t", ",");
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                    countError++;
                    continue;
                }
                if (linha.StartsWith("estamos aguardando licita"))
                {
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                    countError++;
                    continue;
                }

                if (linha.Contains("7835767") || linha.Contains("7835764") || linha.Contains("7835765"))
                {
                    linha = linha.Replace("\t", ";");
                    count++;
                }
                if (linha.StartsWith(";"))
                {
                    countError++;
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                }
                else if (linha.StartsWith($"{quote};"))
                {
                    countError++;
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                }
                else if (linha.StartsWith($"A") || linha.StartsWith($"a"))
                {
                    countError++;
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                }
                else if (linha.StartsWith($"o") || linha.StartsWith($"O"))
                {
                    countError++;
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                }
                else if (linha.StartsWith($"H") || linha.StartsWith($"h"))
                {
                    countError++;
                    var i = list.Last();
                    list.Remove(list.Last());
                    list.Add($"{i}{linha}");
                }
                else
                    list.Add(linha);
            }
            Console.WriteLine($"{patchFile}: Entrada={linhas.Length} - Saida={list.Count} - Erros:{countError}");
            File.WriteAllLines($"{AppConfiguration.DiretorioLocal}Base/{fileName}", list.ToArray(), Encoding.UTF8);
            if (count > 0)
                Console.WriteLine($"Erros:{count}");
            return (linhas.Length, list.Count, countError);
        }

        public string RemoverAcentos(string texto)
        {
            string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
            string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";

            for (int i = 0; i < comAcentos.Length; i++)
            {
                texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
            }
            return texto;
        }
        #endregion
        #region FileNames
        public string GetDirectoryListingRegexForUrl(string url)
        {
            if (url.Equals(AppConfiguration.BaseUrl))
                return "<a href=\".*\">(?<name>.*)</a>";
            throw new NotSupportedException();
        }
        public IEnumerable<string> GetAllFileNamesCSV156()
        {
            string url = $"{AppConfiguration.BaseUrl}";
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
        #endregion

    }

}