using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unittest_ITI_T3_Anonymized_report
{
    class Program
    {
        static void Main(string[] args)
        {

            var client = new RestClient("http://ae.integration.operando.esilab.org:8092/operando/core/ae/personaldata/search");

            RootObject myObj = new RootObject();

            myObj.requesterId = "1";
            myObj.query = "SELECT DISTINCT`operando_personaldatadb`.`occupation`.`DESCRIPTION_0` AS `OCCUPATION`,`operando_personaldatadb`.`salary_class`.`DESCRIPTION_0` AS `SALARY_CLASS`,`operando_personaldatadb`.`genders`.`DESCRIPTION_0` AS `GENDER`,`operando_personaldatadb`.`education`.`DESCRIPTION_0` AS `EDUCATION`,`operando_personaldatadb`.`countries`.`DESCRIPTION_0` AS `COUNTRY`,`operando_personaldatadb`.`race`.`DESCRIPTION_0` AS `RACE`,`operando_personaldatadb`.`generic_personal_data`.`EMAIL_ADDRESS` AS `EMAIL_ADDRESS`,`operando_personaldatadb`.`generic_personal_data`.`CELL_PHONE_NUMBER`  AS `CELL_PHONE_NUMBER`,`operando_personaldatadb`.`generic_personal_data`.`SURNAME` AS `SURNAME`,`operando_personaldatadb`.`generic_personal_data`.`NUMBER_OF_CHILDREN`  AS `NUMBER_OF_CHILDREN`,`operando_personaldatadb`.`generic_personal_data`.`RESIDENCE_POST_CODE`   AS `RESIDENCE_POST_CODE`,`operando_personaldatadb`.`generic_personal_data`.`NAME` AS `NAME`,`operando_personaldatadb`.`generic_personal_data`.`IDENTIFICATION_NUMBER`  AS `IDENTIFICATION_NUMBER`,`operando_personaldatadb`.`generic_personal_data`.`DATE_OF_BIRTH` AS `DATE_OF_BIRTH`,`operando_personaldatadb`.`generic_personal_data`.`SSN` AS `SSN`,`operando_personaldatadb`.`marital_status`.`DESCRIPTION_0` AS `MARITAL_STATUS`,`operando_personaldatadb`.`work_class`.`DESCRIPTION_0` AS `WORK_CLASS`FROM ((((((((`operando_personaldatadb`.`occupation` JOIN `operando_personaldatadb`.`salary_class`)       JOIN `operando_personaldatadb`.`genders`)      JOIN `operando_personaldatadb`.`education`)     JOIN `operando_personaldatadb`.`countries`)   JOIN `operando_personaldatadb`.`race`)  JOIN `operando_personaldatadb`.`generic_personal_data`     ON ((    (`operando_personaldatadb`.`occupation`.`ID` =                 `operando_personaldatadb`.`generic_personal_data`.`OCCUPATION_ID`)          AND (`operando_personaldatadb`.`salary_class`.`ID` =                `operando_personaldatadb`.`generic_personal_data`.`SALARY_CLASS_ID`)         AND (`operando_personaldatadb`.`genders`.`ID` =                 `operando_personaldatadb`.`generic_personal_data`.`GENDER_ID`)        AND (`operando_personaldatadb`.`education`.`ID` =`operando_personaldatadb`.`generic_personal_data`.`EDUCATION_ID`)        AND (`operando_personaldatadb`.`countries`.`ID` =              `operando_personaldatadb`.`generic_personal_data`.`NATIVE_COUNTRY_ID`)         AND (`operando_personaldatadb`.`race`.`ID` =                        `operando_personaldatadb`.`generic_personal_data`.`RACE_ID`)))) JOIN `operando_personaldatadb`.`marital_status` ON ((`operando_personaldatadb`.`marital_status`.`ID` =        `operando_personaldatadb`.`generic_personal_data`.`MARITAL_STATUS_ID`)))JOIN `operando_personaldatadb`.`work_class`ON ((`operando_personaldatadb`.`work_class`.`ID` =          `operando_personaldatadb`.`generic_personal_data`.`WORK_CLASS_ID`)))";
            myObj.queryName = "personalData";
            myObj.dataTypeMap = "{\"MARITAL_STATUS\":\"String\",\"NAME\":\"String\",\"SURNAME\":\"String\",\"OCCUPATION\":\"String\",\"NUMBER_OF_CHILDREN\":\"Integer\",\"COUNTRY\":\"String\",\"SALARY_CLASS\":\"String\",\"WORK_CLASS\":\"String\",\"GENDER\":\"String\",\"DATE_OF_BIRTH\":\"String\",\"IDENTIFICATION_NUMBER\":\"String\",\"EDUCATION\":\"String\",\"CELL_PHONE_NUMBER\":\"String\",\"RACE\":\"String\",\"EMAIL_ADDRESS\":\"String\"}";
            myObj.attributeTypeMap = "{\"MARITAL_STATUS\":\"INSENSITIVE_ATTRIBUTE\",\"NAME\":\"IDENTIFYING_ATTRIBUTE\",\"SURNAME\":\"IDENTIFYING_ATTRIBUTE\",\"OCCUPATION\":[[\"Tech-suppo\",\"Technical\",\"*\\r\"],[\"Craft-repa\",\"Technical\",\"*\\r\"],[\"Other-serv\",\"Other\",\"*\\r\"],[\"Sales\",\"Nontechnic\",\"*\\r\"],[\"Exec-manag\",\"Nontechnic\",\"*\\r\"],[\"Prof-speci\",\"Technical\",\"*\\r\"],[\"Handlers-c\",\"Nontechnic\",\"*\\r\"],[\"Machine-op\",\"Technical\",\"*\\r\"],[\"Adm-cleric\",\"Other\",\"*\\r\"],[\"Farming-fi\",\"Other\",\"*\\r\"],[\"Transport-\",\"Other\",\"*\\r\"],[\"Priv-house\",\"Other\",\"*\\r\"],[\"Protective\",\"Other\",\"*\\r\"],[\"Armed-Forc\",\"Other\",\"*\\r\"]],\"NUMBER_OF_CHILDREN\":\"INSENSITIVE_ATTRIBUTE\",\"COUNTRY\":[[\"United-States\",\"North America\",\"*\\r\"],[\"Cambodia\",\"Asia\",\"*\\r\"],[\"England\",\"Europa\",\"*\\r\"],[\"Puerto-Rico\",\"North America\",\"*\\r\"],[\"Canada\",\"North America\",\"*\\r\"],[\"Germany\",\"Europe\",\"*\\r\"],[\"Outlying-US(Guam-USVI-etc)\",\"North America\",\"*\\r\"],[\"India\",\"Asia\",\"*\\r\"],[\"Japan\",\"Asia\",\"*\\r\"],[\"Greece\",\"Europe\",\"*\\r\"],[\"South\",\"Africa\",\"*\\r\"],[\"China\",\"Asia\",\"*\\r\"],[\"Cuba\",\"North America\",\"*\\r\"],[\"Iran\",\"Asia\",\"*\\r\"],[\"Honduras\",\"North America\",\"*\\r\"],[\"Philippines\",\"Asia\",\"*\\r\"],[\"Italy\",\"Europe\",\"*\\r\"],[\"Poland\",\"Europe\",\"*\\r\"],[\"Jamaica\",\"North America\",\"*\\r\"],[\"Vietnam\",\"Asia\",\"*\\r\"],[\"Mexico\",\"North America\",\"*\\r\"],[\"Portugal\",\"Europe\",\"*\\r\"],[\"Ireland\",\"Europe\",\"*\\r\"],[\"France\",\"Europe\",\"*\\r\"],[\"Dominican-Republic\",\"North America\",\"*\\r\"],[\"Laos\",\"Asia\",\"*\\r\"],[\"Ecuador\",\"South America\",\"*\\r\"],[\"Taiwan\",\"Asia\",\"*\\r\"],[\"Haiti\",\"North America\",\"*\\r\"],[\"Columbia\",\"South America\",\"*\\r\"],[\"Hungary\",\"Europe\",\"*\\r\"],[\"Guatemala\",\"North America\",\"*\\r\"],[\"Nicaragua\",\"South America\",\"*\\r\"],[\"Scotland\",\"Europe\",\"*\\r\"],[\"Thailand\",\"Asia\",\"*\\r\"],[\"Yugoslavia\",\"Europe\",\"*\\r\"],[\"El-Salvador\",\"North America\",\"*\\r\"],[\"Trinadad&Tobago\",\"South America\",\"*\\r\"],[\"Peru\",\"South America\",\"*\\r\"],[\"Hong\",\"Asia\",\"*\\r\"],[\"Holand-Netherlands\",\"Europe\",\"*\\r\"]],\"SALARY_CLASS\":[[\"<=50K\",\"*\",\"*\"],[\">50K\",\"*\",\"*\"]],\"WORK_CLASS\":[[\"Private\",\"Non-Government\",\"*\\r\"],[\"Self-emp-not-inc\",\"Non-Government\",\"*\\r\"],[\"Self-emp-inc\",\"Non-Government\",\"*\\r\"],[\"Federal-gov\",\"Government\",\"*\\r\"],[\"Local-gov\",\"Government\",\"*\\r\"],[\"State-gov\",\"Government\",\"*\\r\"],[\"Without-pay\",\"Unemployed\",\"*\\r\"],[\"Never-worked\",\"Unemployed\",\"*\\r\"]],\"GENDER\":\"INSENSITIVE_ATTRIBUTE\",\"DATE_OF_BIRTH\":\"IDENTIFYING_ATTRIBUTE\",\"IDENTIFICATION_NUMBER\":\"IDENTIFYING_ATTRIBUTE\",\"EDUCATION\":[[\"Bachelors\",\"Undergraduate\",\"Higher education\"],[\"Some-college\",\"Undergraduate\",\"Higher education\"],[\"11th\",\"High School\",\"Secondary education\"],[\"HS-grad\",\"High School\",\"Secondary education\"],[\"Prof-school\",\"Professional Education\",\"Higher education\"],[\"Assoc-acdm\",\"Professional Education\",\"Higher education\"],[\"Assoc-voc\",\"Professional Education\",\"Higher education\"],[\"9th\",\"High School\",\"Secondary education\"],[\"7th-8th\",\"High School\",\"Secondary education\"],[\"12th\",\"High School\",\"Secondary education\"],[\"Masters\",\"Graduate\",\"Higher education\"],[\"1st-4th\",\"Primary School\",\"Primary education\"],[\"10th\",\"High School\",\"Secondary education\"],[\"Doctorate\",\"Graduate\",\"Higher education\"],[\"5th-6th\",\"Primary School\",\"Primary education\"],[\"Preschool\",\"Primary School\",\"Primary education\"]],\"CELL_PHONE_NUMBER\":\"IDENTIFYING_ATTRIBUTE\",\"RACE\":\"INSENSITIVE_ATTRIBUTE\",\"EMAIL_ADDRESS\":\"IDENTIFYING_ATTRIBUTE\"}";
            myObj.kanonymity = 2;

            string _myObj = JsonConvert.SerializeObject(myObj);


            var request = new RestRequest("", Method.POST);
            request.AddParameter("application/json", _myObj, ParameterType.RequestBody);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("postman-token", "efc35b48-6699-5666-afd5-aa226487a17a");

            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string

            //object value = JsonConvert.DeserializeObject(content);

            JToken token = JObject.Parse(content);
            string code = "";
            string type = "";
            string message = "";

            code = (string)token.SelectToken("code");
            type = (string)token.SelectToken("type");
            message = (string)token.SelectToken("message");
            Debug.Assert(code == "4", "Error code");
            Debug.Assert(type == "ok", "Type Error");

            String[] messageArray = message.Split(']');

            for (int i = 0; i < messageArray.Length; i++)
            {
                messageArray[i] = messageArray[i].Replace("[", "").Replace("]", "");
            }
            DataTable mytb = new DataTable("Dati");
            String[] colonne = messageArray[0].Split(',');

            foreach (String item in colonne)
            {
                mytb.Columns.Add(item.Replace(" ", ""));
            }

            for (int i = 1; i < messageArray.Length; i++)
            {
                if (String.IsNullOrEmpty(messageArray[i]))
                {
                    continue;
                }
                DataRow riga = mytb.NewRow();
                String[] valori = messageArray[i].Split(',');
                for (int r = 0; r < colonne.Length; r++)
                {
                    riga[r] = valori[r];
                }
                mytb.Rows.Add(riga);
            }
        }
        public class RootObject
        {
            public string requesterId { get; set; }
            public string query { get; set; }
            public string queryName { get; set; }
            public string dataTypeMap { get; set; }
            public string attributeTypeMap { get; set; }
            public int kanonymity { get; set; }
        }

    }
}
