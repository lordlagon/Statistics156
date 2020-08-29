using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Central156
{
    public class Program
    {
        public string BaseUrl = "ftp://dadosabertos.c3sl.ufpr.br/curitiba/156/";
        public string DiretorioLocal = "C:/Sistemas/156/csv/";

        public static void Main(string[] args)
        {
            WebRequest request = WebRequest.Create("http://dadosabertos.c3sl.ufpr.br/curitiba/156/");
            request.Credentials = CredentialCache.DefaultCredentials;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine(response.StatusDescription);
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        public void DownloadFile()
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completo);
            webClient.DownloadFileAsync(new Uri(BaseUrl), @"c:\temp\arquivo.txt");
        }

        private void Completo(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Download efetuado!");
        }
    }
}
