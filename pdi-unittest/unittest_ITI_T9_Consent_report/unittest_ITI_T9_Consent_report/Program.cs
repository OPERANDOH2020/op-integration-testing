using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest_ITI_T9_Consent_report
{
    class Program
    {
        static void Main(string[] args)
        {
            //var serializer = new YamlSerializer();

            var client = new RestClient("http://localhost:29756/api/");

            string mode = "READ";
            if (args.Length>1 && args[0]!=null)
            {
                mode = args[0].ToString();
            }
            string id = "pat";
            if (args.Length > 2 && args[1] != null)
            {
                id = args[1].ToString();
            }

            var request = new RestRequest("", Method.POST);

            switch (mode)
            {
                case "READ":
                    //2) Read a UPP - GET / user_privacy_policy /{ user - id}
                    request = new RestRequest("consent/" + id + "/", Method.GET);
                    break;
                default:
                    break;
            }

            //client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", "key-f28ca9730862959738de8b244678e4f9");

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            object value = JsonConvert.DeserializeObject<OSPConsents>(content);

            JToken token = JObject.Parse(content);
            string code = "";
            string type = "";
            string message = "";

            switch (mode)
            {
                case "READ":
                    code = (string)token.SelectToken("code");
                    type = (string)token.SelectToken("type");
                    message = (string)token.SelectToken("message");
                    Debug.Assert(code == "2", "Code Error");
                    Debug.Assert(type == "error", "Type Error");
                    break;
                default:
                    break;
            }



        }


    }

    public class OSPConsents
    {
        /// <summary>
        /// The unique ID of the OSP user is subscribed to and these consent policies concern. 
        /// </summary>
        public string osp_id { get; set; }
        /// <summary>
        /// OSP access policies
        /// </summary>
        public AccessPolicy[] access_policies;

        public OSPConsents()
        {
            osp_id = "pat";

            access_policies = new AccessPolicy[2];
            access_policies[0] = new AccessPolicy();
            access_policies[0].subject = "doctor";
            access_policies[0].permission = true;
            access_policies[0].action = AccessPolicy.Action.Access.ToString();
            access_policies[0].resource = "personal_information/full_name/given_name";
            access_policies[0].attributes = new PolicyAttribute[1];
            access_policies[0].attributes[0] = new PolicyAttribute();
            access_policies[0].attributes[0].attribute_name = "policy attribute name1";
            access_policies[0].attributes[0].attribute_value = "policy attribute value1";

            access_policies[1] = new AccessPolicy();
            access_policies[1].subject = "nurse";
            access_policies[1].permission = true;
            access_policies[1].action = AccessPolicy.Action.Access.ToString();
            access_policies[1].resource = "personal_information/full_name/given_name";
            access_policies[1].attributes = new PolicyAttribute[1];
            access_policies[1].attributes[0] = new PolicyAttribute();
            access_policies[1].attributes[0].attribute_name = "policy attribute name1";
            access_policies[1].attributes[0].attribute_value = "policy attribute value1";


        }

    }

    public class AccessPolicy
    {
        /// <summary>
        /// A description of the subject who the policies grants/doesn't grant to carry out.
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// Grant or deny the subject access to the resource via the operation defined in this policy
        /// </summary>
        public bool permission { get; set; }
        public enum Action { Collect, Access, Use, Disclose, Archive }
        /// <summary>
        /// The action being carried out on the private date e.g. accessing, disclosing to a third party. 
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// The identifier of the resource that the policy concerns (e.g. URL)
        /// </summary>
        public string resource { get; set; }
        /// <summary>
        /// The set of context attributes attached to the policy (e.g. subject role, subject purpose)
        /// </summary>
        public PolicyAttribute[] attributes;

    }

    public class PolicyAttribute
    {
        /// <summary>
        /// name
        /// </summary>
        public string attribute_name { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string attribute_value { get; set; }
    }
}

