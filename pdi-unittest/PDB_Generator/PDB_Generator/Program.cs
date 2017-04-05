using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDB_Generator
{
    public class Generator
    {

        private static String PDB_URL = "http://integration.operando.esilab.org:8096/operando/core/pdb";

        private static String PC_URL = "http://integration.operando.esilab.org:8095/operando/core/pc";

        private static String OSP_ID = null;


        static void Main(string[] args)
        {

            // createOSP
            var client = new RestClient(PDB_URL);
            var request = new RestRequest("/OSP", Method.POST);
            string policy = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory+"/aslbergamo_gat.json");
            request.AddParameter("application/json", policy, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode.ToString()!= "Created")
            {
                throw new Exception("Error on creation of PDB");
            }
            ResponseStandard myresponse= JsonConvert.DeserializeObject<ResponseStandard>(response.Content);
            UPP myUPP = JsonConvert.DeserializeObject<UPP>(myresponse.message);
            string ospId = myUPP.policy_text;

            // ospQuerybyFriendlyName
            client = new RestClient(PDB_URL);
            request = new RestRequest("/OSP/?filter=%7B%27policyText%27:%27" + ospId + "%27%7D", Method.GET);
            response = client.Execute(request);
            UPP[] myUPPArray = JsonConvert.DeserializeObject<UPP[]>(response.Content);
            string osp_policy_id = myUPPArray[myUPPArray.Length-1].osp_policy_id;


            //create delete UPP && loadDemoUPP
            string userId = "pete";

            client = new RestClient(PDB_URL);
            request = new RestRequest("/user_privacy_policy/"+ userId, Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);
            
            client = new RestClient(PDB_URL);
            request = new RestRequest("/user_privacy_policy/", Method.POST);
            string pete = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/"+ userId + ".json");
            request.AddParameter("application/json", pete, ParameterType.RequestBody);
            response = client.Execute(request);

            // READ UPP
            client = new RestClient(PDB_URL);
            request = new RestRequest("/user_privacy_policy/"+ userId + "/", Method.GET);
            response = client.Execute(request);
            
            // compute PC
            client = new RestClient(PC_URL);
            request = new RestRequest("/osp_policy_computer?user_id=" + userId +"&osp_id=" + osp_policy_id, Method.POST);
            request.AddHeader("Content-Type", "application/json");
            response = client.Execute(request);
            myresponse = JsonConvert.DeserializeObject<ResponseStandard>(response.Content);
            if (myresponse.type=="error")
            {
                throw new Exception("Error: "+ myresponse.message);
            }

            //evaluate PC
            String accessRequest = createRequest(osp_policy_id);

            request = new RestRequest("/osp_policy_evaluator?user_id=" + userId + "&osp_id=" + osp_policy_id, Method.POST);
            request.AddParameter("application/json", accessRequest, ParameterType.RequestBody);
            response = client.Execute(request);
            //myresponse = JsonConvert.DeserializeObject<ResponseStandard>(response.Content);

            accessRequest = createRequestTwo(osp_policy_id);

            request = new RestRequest("/osp_policy_evaluator?user_id=" + userId + "&osp_id=" + osp_policy_id, Method.POST);
            request.AddParameter("application/json", accessRequest, ParameterType.RequestBody);
            response = client.Execute(request);
            //myresponse = JsonConvert.DeserializeObject<ResponseStandard>(response.Content);

        }

        private static String toJSONRequest(List<OSPDataRequest> request)
        {
            String jsonRequest = "[";
            foreach (OSPDataRequest dReq in request)
            {
                jsonRequest += "{";
                jsonRequest += "\"requester_id\": \"" + dReq.RequesterId + "\", ";
                jsonRequest += "\"subject\": \"" + dReq.Subject + "\", ";
                jsonRequest += "\"requested_url\": \"" + dReq.RequestedUrl + "\", ";
                jsonRequest += "\"action\": \"" + dReq.Action + "\", ";
                jsonRequest += "\"attributes\": []";
                jsonRequest += "},";
            }
            jsonRequest = jsonRequest.Substring(0, jsonRequest.Length - 1);
            jsonRequest += "]";
            return jsonRequest;
        }

        private static String createRequest(string OSP_ID)
        {
            List<OSPDataRequest> ospRequest = new List<OSPDataRequest>();
            OSPDataRequest osD = new OSPDataRequest();
            osD.Action = OSPDataRequest.ActionEnum.Access;
            osD.RequesterId = OSP_ID ;
            osD.Subject = "doctor";
            osD.RequestedUrl = "/GA_Patients/debitiDuranteGioco";
            ospRequest.Add(osD);

            return toJSONRequest(ospRequest);
        }

        private static String createRequestTwo(string OSP_ID)
        {
            List<OSPDataRequest> ospRequest = new List<OSPDataRequest>();
            OSPDataRequest osD = new OSPDataRequest();
            osD.Action = OSPDataRequest.ActionEnum.Access;
            osD.RequesterId = OSP_ID;
            osD.Subject="doctor";
            osD.RequestedUrl ="/GA_Patients/ProvinciaResidenza";
            ospRequest.Add(osD);

            return toJSONRequest(ospRequest);
        }

    }
}

