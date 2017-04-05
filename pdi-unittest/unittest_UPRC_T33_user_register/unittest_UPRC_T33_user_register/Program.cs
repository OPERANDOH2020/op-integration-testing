using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest_UPRC_T9_user_register
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginData _LoginData = new LoginData();
            string _user = JsonConvert.SerializeObject(_LoginData);

            LoginUser _LoginUser = new LoginUser();
            string _userTicket = JsonConvert.SerializeObject(_LoginUser);

            var client = new RestClient("http://integration.operando.esilab.org:8135/operando/interfaces/aapi/");

            string mode = "CREATE";
            if (args.Length >1 && args[0]!=null)
            {
                mode = args[0];
            }
            string username = "aaaaa";
            if (args.Length > 2 && args[1] != null)
            {
                username = args[1];
            }

            var request = new RestRequest("", Method.POST);

            switch (mode)
            {
                case "CREATE":
                    //1) Create a UPP - POST to / user_privacy_policy
                    request = new RestRequest("/aapi/user/register", Method.POST);
                    //request.AddParameter("user", _user);
                    request.AddParameter("application/json", _user, ParameterType.RequestBody);
                    break;
                case "UPDATE":
                    //2) Update a UPP - PUT / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/user/" + username + "/", Method.PUT);
                    request.AddParameter("application/json", _user, ParameterType.RequestBody);
                    break;
                case "DELETE":
                    //3) Delete a UPP - DELETE / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/user/" + username + "/", Method.DELETE);
                    break;
                case "GET":
                    //4) Read a UPP - GET / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/user/" + username + "/", Method.GET);
                    break;
                case "TICKET":
                    //5) Read a UPP - Ticket / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/tickets", Method.POST);
                    request.AddParameter("application/json", _userTicket, ParameterType.RequestBody);
                    break;
                case "TICKET-SERVICE":
                    //5) Read a UPP - Ticket / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/tickets", Method.POST);
                    request.AddParameter("application/json", _userTicket, ParameterType.RequestBody);
                    IRestResponse responseTicket = client.Execute(request);
                    string ticket = responseTicket.Content.ToString(); // raw content as string
                    responseTicket = null;
                    request = new RestRequest("/aapi/tickets/"+ ticket, Method.POST);
                    request.AddParameter("application/json", "/gatekeeper", ParameterType.RequestBody);
                    break;
                case "TICKET-SERVICE-ERROR":
                    //5) Read a UPP - Ticket / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/tickets", Method.POST);
                    request.AddParameter("application/json", _userTicket, ParameterType.RequestBody);
                    IRestResponse responseTicketErr = client.Execute(request);
                    string ticketErr = responseTicketErr.Content.ToString(); // raw content as string
                    responseTicket = null;
                    request = new RestRequest("/aapi/tickets/" + ticketErr, Method.POST);
                    request.AddParameter("application/json", "/falseServiceID", ParameterType.RequestBody);
                    break;
                case "TICKET-SERVICE-VALIDATE":
                    //5) Read a UPP - Ticket / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/tickets", Method.POST);
                    request.AddParameter("application/json", _userTicket, ParameterType.RequestBody);
                    IRestResponse responseTicketGet = client.Execute(request);
                    string ticketGet = responseTicketGet.Content.ToString(); // raw content as string
                    responseTicketGet = null;
                    request = new RestRequest("/aapi/tickets/" + ticketGet, Method.POST);
                    request.AddParameter("application/json", "/gatekeeper", ParameterType.RequestBody);
                    IRestResponse responseTicketService = client.Execute(request);
                    string ticketService = responseTicketService.Content.ToString(); // raw content as string
                    responseTicketService = null;
                    request = new RestRequest("/aapi/tickets/" + ticketService+ "/validate?serviceId=/gatekeeper", Method.GET);
                    break;
                case "TICKET-SERVICE-VALIDATE-ERROR":
                    //5) Read a UPP - Ticket / user_privacy_policy /{ user - id}
                    request = new RestRequest("/aapi/tickets", Method.POST);
                    request.AddParameter("application/json", _userTicket, ParameterType.RequestBody);
                    IRestResponse responseTicketGetErr = client.Execute(request);
                    string ticketGetErr = responseTicketGetErr.Content.ToString(); // raw content as string
                    responseTicketGet = null;
                    request = new RestRequest("/aapi/tickets/" + ticketGetErr, Method.POST);
                    request.AddParameter("application/json", "/gatekeeper", ParameterType.RequestBody);
                    IRestResponse responseTicketServiceErr = client.Execute(request);
                    string ticketServiceErr = responseTicketServiceErr.Content.ToString(); // raw content as string
                    responseTicketServiceErr = null;
                    request = new RestRequest("/aapi/tickets/" + ticketServiceErr + "/validate?serviceId=/falseServiceID", Method.GET);
                    break;
                default:
                    break;
            }

            //client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", "key-f28ca9730862959738de8b244678e4f9");
            //request.AddHeader("Content-Type","application/json");
            //request.AddHeader("Accept", "*/*");

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            switch (mode)
            {
                case "CREATE":
                    Debug.Assert(response.StatusCode.ToString() == "Created", "Error CREATE user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "UPDATE":
                    Debug.Assert(response.StatusCode.ToString() == "Accepted", "Error UPDATE user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "DELETE":
                    Debug.Assert(response.StatusCode.ToString() == "Accepted", "Error DELETE user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "GET":
                    Debug.Assert(response.StatusCode.ToString() == "OK", "Error GET user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "TICKET":
                    Debug.Assert(response.StatusCode.ToString() == "Created", "Error TICKET user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "TICKET-SERVICE":
                    Debug.Assert(response.StatusCode.ToString() == "OK", "Error TICKET-SERVICE user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "TICKET-SERVICE-ERROR":
                    Debug.Assert(response.StatusCode.ToString() == "InternalServerError", "Error TICKET-SERVICE-ERROR user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "TICKET-SERVICE-VALIDATE":
                    Debug.Assert(response.StatusCode.ToString() == "OK", "Error TICKET-SERVICE-VALIDATE user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                case "TICKET-SERVICE-VALIDATE-ERROR":
                    Debug.Assert((response.StatusCode.ToString() == "InternalServerError" || response.StatusCode.ToString() == "NotFound"), "Error TICKET-SERVICE-VALIDATE-ERROR user");
                    Debug.Assert(response.Content != "", "Content Error");
                    break;
                default:
                    break;
            }

        }

        public class OptionalAttr
        {
            public string attrName { get; set; }
            public string attrValue { get; set; }
        }

        public class PrivacySetting
        {
            public string settingName { get; set; }
            public string settingValue { get; set; }
        }

        public class RequiredAttr
        {
            public string attrName { get; set; }
            public string attrValue { get; set; }
        }

        public class LoginData
        {
            public List<OptionalAttr> optionalAttrs { get; set; }
            public string password { get; set; }
            public List<PrivacySetting> privacySettings { get; set; }
            public List<RequiredAttr> requiredAttrs { get; set; }
            public string username { get; set; }

            public LoginData()
            {
                username = "test"+DateTime.Now.Minute;
                password = "TesT" + DateTime.Now.Minute;

                optionalAttrs = new List<OptionalAttr>();
                OptionalAttr _OptionalAttr = new OptionalAttr();
                _OptionalAttr.attrName = "string";
                _OptionalAttr.attrValue = "string";
                optionalAttrs.Add(_OptionalAttr);

                privacySettings = new List<PrivacySetting>();
                PrivacySetting _PrivacySetting = new PrivacySetting();
                _PrivacySetting.settingName = "string";
                _PrivacySetting.settingValue = "string";
                privacySettings.Add(_PrivacySetting);

                requiredAttrs = new List<RequiredAttr>();
                RequiredAttr _RequiredAttr = new RequiredAttr();
                _RequiredAttr.attrName = "role";
                _RequiredAttr.attrValue = "Gaslini -  Tutor";
                requiredAttrs.Add(_RequiredAttr);



            }
        }

        public class LoginUser
        {
            public string password { get; set; }
            public string username { get; set; }

            public LoginUser()
            {
                username = "test40";
                password = "TesT40";
            }
        }
    }
}
