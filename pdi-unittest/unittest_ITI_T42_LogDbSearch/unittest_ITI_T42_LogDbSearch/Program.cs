using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest_ITI_T42_LogDbSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("http://integration.operando.esilab.org:8091/operando/core/ldbsearch/log/");

            var request = new RestRequest("search", Method.GET);
            //request.AddParameter("user", _user);
            request.AddParameter("dateFrom", "2016-12-12 08:11:21", ParameterType.QueryString);
            request.AddParameter("dateTo", "2020-12-31 00:00:00", ParameterType.QueryString);
            request.AddParameter("logLevel", "INFO", ParameterType.QueryString);
            request.AddParameter("requesterType", "MODULE", ParameterType.QueryString);
            request.AddParameter("requesterId", "", ParameterType.QueryString);
            request.AddParameter("logPriority", "", ParameterType.QueryString);
            request.AddParameter("title", "", ParameterType.QueryString);
            request.AddParameter("keyWords", "[personal]", ParameterType.QueryString);

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            Debug.Assert(content != null && content != "", "Error no Log");

            List<RootObject> responseDes = ((Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(content)).ToObject<List<RootObject>>();

            Debug.Assert(responseDes.Count > 0, "Error no Log");

            foreach (RootObject item in responseDes)
            {
                Debug.Assert(item.description != "", "Description not valid");
                Debug.Assert(item.title != "", "Title not valid");
                Debug.Assert(item.logLevel == "INFO", "logLevel not valid");
                Debug.Assert(item.requesterType == "MODULE", "logLevel not valid");
            }

            request = new RestRequest("search", Method.GET);
            //request.AddParameter("user", _user);
            request.AddParameter("dateFrom", "2016-12-10 08:11:21", ParameterType.QueryString);
            request.AddParameter("dateTo", "2020-12-31 00:00:00", ParameterType.QueryString);
            request.AddParameter("logLevel", "ERROR", ParameterType.QueryString);
            request.AddParameter("requesterType", "MODULE", ParameterType.QueryString);
            request.AddParameter("requesterId", "", ParameterType.QueryString);
            request.AddParameter("logPriority", "", ParameterType.QueryString);
            request.AddParameter("title", "", ParameterType.QueryString);
            request.AddParameter("keyWords", "", ParameterType.QueryString);

            // execute the request
            response = client.Execute(request);
            content = response.Content; // raw content as string

            responseDes = ((Newtonsoft.Json.Linq.JArray)JsonConvert.DeserializeObject(content)).ToObject<List<RootObject>>();

            Debug.Assert(responseDes.Count > 0, "Error no Log");

            foreach (RootObject item in responseDes)
            {
                Debug.Assert(item.description != "", "Description not valid");
                Debug.Assert(item.title != "", "Title not valid");
                Debug.Assert(item.logLevel == "ERROR", "logLevel not valid");
                Debug.Assert(item.requesterType == "MODULE", "logLevel not valid");
            }


            Console.WriteLine(content);
            Console.WriteLine(response.ErrorMessage);
            Console.WriteLine(response.ErrorException);


        }

    }

    public class RootObject
    {
        public string logDate { get; set; }
        public string requesterType { get; set; }
        public string requesterId { get; set; }
        public string logPriority { get; set; }
        public string logLevel { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }
}
