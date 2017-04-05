using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest_ITI_T39_UPP_Configuration
{
    class Program
    {
        static void Main(string[] args)
        {
            //var serializer = new YamlSerializer();

            UserPrivacyPolicy _UserPrivacyPolicy = new UserPrivacyPolicy();
            //string _UserPrivacyPolicySerialized = serializer.Serialize(_UserPrivacyPolicy);
            string _UserPrivacyPolicySerialized = JsonConvert.SerializeObject(_UserPrivacyPolicy);


            var client = new RestClient("http://integration.operando.esilab.org:8096/operando/core/pdb/");

            string mode = "CREATE";
            if (args.Length>1 && args[0] != null)
            {
                mode = args[0].ToString();
            }
            string id = "0";
            if (args.Length > 2 && args[1] != null)
            {
                id = args[1].ToString();
            }

            var request = new RestRequest("", Method.POST);

            switch (mode)
            {
                case "CREATE":
                    //1) Create a UPP - POST to / user_privacy_policy
                    request = new RestRequest("user_privacy_policy", Method.POST);

                    //request.AddParameter("Content-Type", "application/json",ParameterType.HttpHeader);
                    //request.AddParameter("Accept", "*/*", ParameterType.HttpHeader);
                    //request.XmlSerializer.ContentType = "application/json";
                    //request.RequestFormat = DataFormat.Json;
                    //request.JsonSerializer.ContentType = "application/json; charset=utf-8";

                    //request.AddParameter("name", "upp");
                    //request.AddParameter("in", "body");
                    //request.AddParameter("description", "description");
                    //request.AddParameter("required", "true");
                    //request.AddParameter("schema", _UserPrivacyPolicySerialized);
                    request.AddParameter("application/json", _UserPrivacyPolicySerialized, ParameterType.RequestBody);
                    break;
                case "READ":
                    //2) Read a UPP - GET / user_privacy_policy /{ user - id}
                    request = new RestRequest("user_privacy_policy/" + id + "/", Method.GET);
                    break;
                case "DELETE":
                    //3) Delete a UPP - DELETE / user_privacy_policy /{ user - id}
                    request = new RestRequest("user_privacy_policy/" + id + "/", Method.DELETE);
                    break;
                case "UPDATE":
                    //4) Update a UPP - PUT / user_privacy_policy /{ user - id}
                    request = new RestRequest("user_privacy_policy/" + id + "/", Method.PUT);
                    //request.AddParameter("name", "upp");
                    //request.AddParameter("in", "body");
                    //request.AddParameter("description", "description");
                    //request.AddParameter("required", "true");
                    //request.AddParameter("schema", _UserPrivacyPolicySerialized);
                    request.AddParameter("application/json", _UserPrivacyPolicySerialized, ParameterType.RequestBody);
                    break;
                default:
                    break;
            }

            //client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("api", "key-f28ca9730862959738de8b244678e4f9");

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            //object value = JsonConvert.DeserializeObject(content);

            JToken token = JObject.Parse(content);
            string code = "";
            string type = "";
            string message = "";

            switch (mode)
            {
                case "CREATE":
                    code = (string)token.SelectToken("code");
                    type = (string)token.SelectToken("type");
                    message = (string)token.SelectToken("message");
                    Debug.Assert(code == "4", "Error code");
                    Debug.Assert(type == "ok", "Type Error");

                    break;
                case "READ":
                    code = (string)token.SelectToken("code");
                    type = (string)token.SelectToken("type");
                    message = (string)token.SelectToken("message");
                    Debug.Assert(code != "1", "Error code");
                    Debug.Assert(type != "error", "Type Error");

                    break;
                case "DELETE":
                    code = (string)token.SelectToken("code");
                    type = (string)token.SelectToken("type");
                    message = (string)token.SelectToken("message");
                    Debug.Assert(code == "4", "Error code");
                    Debug.Assert(type == "ok", "Type Error");

                    break;
                case "UPDATE":
                    code = (string)token.SelectToken("code");
                    type = (string)token.SelectToken("type");
                    message = (string)token.SelectToken("message");
                    Debug.Assert(code == "4", "Error code");
                    Debug.Assert(type == "ok", "Type Error");
                    break;
                default:
                    break;
            }
        }
    }


    public class UserPrivacyPolicy
    {
        /// <summary>
        /// The unique operando id of the user this policy is about. Each OPERANDO user has one and only one UPP.
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// User stated preferences (questionnaire answers)
        /// </summary>
        public User_Preferences[] user_preferences;
        /// <summary>
        /// The user policies for each OSP they subscribe to.
        /// </summary>
        public OSPConsents[] subscribed_osp_policies;
        /// <summary>
        /// The user settings for each of their services
        /// </summary>
        public OSPSettings[] subscribed_osp_settings;

        public UserPrivacyPolicy()
        {
            user_id = "pat";

            user_preferences = new User_Preferences[1];
            user_preferences[0] = new User_Preferences();
            user_preferences[0].informationtype = "/personal_information/full_name/given_name";
            user_preferences[0].category = "Medical";
            user_preferences[0].preference = User_Preferences.Preference.High.ToString();
            user_preferences[0].role = "health_care_professional";
            user_preferences[0].action = AccessPolicy.Action.Access.ToString();
            user_preferences[0].purpose = "Primary Care";
            user_preferences[0].recipient = "none";


            subscribed_osp_policies = new OSPConsents[1];
            subscribed_osp_policies[0] = new OSPConsents();
            subscribed_osp_policies[0].osp_id = "YellowPages";
            subscribed_osp_policies[0].access_policies = new AccessPolicy[1];
            subscribed_osp_policies[0].access_policies[0] = new AccessPolicy();
            subscribed_osp_policies[0].access_policies[0].subject = "doctor";
            subscribed_osp_policies[0].access_policies[0].permission = true;
            subscribed_osp_policies[0].access_policies[0].action = AccessPolicy.Action.Access.ToString();
            subscribed_osp_policies[0].access_policies[0].resource = "personal_information/full_name/given_name";
            subscribed_osp_policies[0].access_policies[0].attributes = new PolicyAttribute[1];
            subscribed_osp_policies[0].access_policies[0].attributes[0] = new PolicyAttribute();
            subscribed_osp_policies[0].access_policies[0].attributes[0].attribute_name = "policy attribute name1";
            subscribed_osp_policies[0].access_policies[0].attributes[0].attribute_value = "policy attribute value1";

            subscribed_osp_settings = new OSPSettings[1];
            subscribed_osp_settings[0] = new OSPSettings();
            subscribed_osp_settings[0].osp_id = "0123456789";
            subscribed_osp_settings[0].osp_settings = new PrivacySetting[1];
            subscribed_osp_settings[0].osp_settings[0] = new PrivacySetting();
            subscribed_osp_settings[0].osp_settings[0].id = 0123456789;
            subscribed_osp_settings[0].osp_settings[0].description = "description";
            subscribed_osp_settings[0].osp_settings[0].name = "name";
            subscribed_osp_settings[0].osp_settings[0].setting_key = "key";
            subscribed_osp_settings[0].osp_settings[0].setting_value = "value";
        }
    }

    public class User_Preferences
    {
        /// <summary>
        /// The type of private information; e.g. is it information that identifies the user (e.g. id number)? is it location information about the user? Is it about their activities?
        /// </summary>
        public string informationtype { get; set; }
        /// <summary>
        /// The category of the service invading the privacy of the user.
        /// </summary>
        public string category { get; set; }

        public enum Preference { High, Medium, Low }
        /// <summary>
        /// The user's privacy preference. High means they are sensitive to disclosing private information. Medium they have concerns; and low means they have few privacy concerns with this question.
        /// </summary>
        public string preference { get; set; }
        /// <summary>
        /// The role of a person or service that the private information is being disclosed to or used by. This is an optional parameter in the case where users drill down to more detailed preferences.
        /// </summary>
        public string role { get; set; }
        /// <summary>
        /// The action being carried out on the private date e.g. accessing, disclosing to a third party. This is an optional parameter in the case where users drill down to more detailed preferences. 
        /// </summary>
        public string action { get; set; }
        /// <summary>
        /// The purpose for which the service is using the private data. This is an optional parameter in the case where users drill down to more detailed preferences.
        /// </summary>
        public string purpose { get; set; }
        /// <summary>
        /// The recipient of any disclosed privacy information. This is an optional parameter in the case where users drill down to more detailed preferences.
        /// </summary>
        public string recipient { get; set; }

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


    public class OSPSettings
    {
        /// <summary>
        /// The unique ID of the OSP user is subscribed to and these settings concern. 
        /// </summary>
        public string osp_id { get; set; }
        /// <summary>
        /// The list of privacy settings at an OSP
        /// </summary>
        public PrivacySetting[] osp_settings;

    }

    public class PrivacySetting
    {
        /// <summary>
        /// PrivacySetting Unique Identifier
        /// </summary>
        public Int64 id { get; set; }
        /// <summary>
        /// Description of the setting
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Short name of the setting(e.g. visibility)
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Targeted setting key
        /// </summary>
        public string setting_key { get; set; }
        /// <summary>
        /// Targeted setting value
        /// </summary>
        public string setting_value { get; set; }

    }
}
