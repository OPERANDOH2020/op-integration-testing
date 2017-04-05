using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Diagnostics;

namespace EmailNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.mailgun.net/v3/mg.operando.eu");
            var request = new RestRequest("messages", Method.POST);
            request.AddParameter("from", "gilad@mg.operando.eu");
            var to = "daniele.detecterror@progettidiimpresa.it, giulia.detecterror@progettidiimpresa.it, federico.dibernardo@progettidiimpresa.it";
            request.AddParameter("to", to.Replace("'", ""));
            var subject = "Operando mailgun test send email via rest api";
            request.AddParameter("subject", subject.Replace("'", ""));
            var text = "Operando mailgun test send email via rest api using RestSharp";
            request.AddParameter("text", text.Replace("'", ""));
            client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", "key-f28ca9730862959738de8b244678e4f9");

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string
            Debug.Assert(content.IndexOf("Queued. Thank you.")>=0, "Error on send mail");
            Console.Write(content);
        }
    }
}
