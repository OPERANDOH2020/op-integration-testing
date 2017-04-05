using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            RootObject _RootObject = new RootObject();
            _RootObject.userId = "001";
            _RootObject.requesterType = "MODULE";
            _RootObject.requesterId = "1001";
            _RootObject.logPriority = "NORMAL";
            _RootObject.logDataType = "INFO";
            _RootObject.title = "Privacy settings discrepancy";
            _RootObject.description = "The privacy settings for user 001 with OSP 12 are not as required. This requires action.";
            string keyword = "privacy";
            _RootObject.keywords = new List<string>();
            _RootObject.keywords.Add(keyword);


            string _RootObjectArraySerialized = JsonConvert.SerializeObject(_RootObject);

            var client = new RestClient("http://integration.operando.esilab.org:8090/operando/core/ldb/");

            
            var request = new RestRequest("log", Method.POST);
            request.AddParameter("application/json", _RootObjectArraySerialized, ParameterType.RequestBody);
                    

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            JToken token = JObject.Parse(content);
            string code = "";
            string type = "";
            string message = "";

            code = (string)token.SelectToken("code");
            type = (string)token.SelectToken("type");
            message = (string)token.SelectToken("message");
            Debug.Assert(code == "4", "Error code");
            Debug.Assert(type == "ok", "Type Error");
        }

    }

    public class RootObject
    {
        public string userId { get; set; }
        public string requesterType { get; set; }
        public string requesterId { get; set; }
        public string logPriority { get; set; }
        public string logDataType { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<string> keywords { get; set; }
    }
}
