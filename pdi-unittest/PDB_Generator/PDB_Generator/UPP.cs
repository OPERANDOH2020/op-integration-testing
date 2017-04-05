using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDB_Generator
{
    public class Attribute
    {
        public object attribute_name { get; set; }
        public object attribute_value { get; set; }
    }

    public class Workflow
    {
        public string requester_id { get; set; }
        public string subject { get; set; }
        public string requested_url { get; set; }
        public string action { get; set; }
        public List<Attribute> attributes { get; set; }
    }

    public class Attribute2
    {
        public object attribute_name { get; set; }
        public object attribute_value { get; set; }
    }

    public class Policy
    {
        public string subject { get; set; }
        public bool permission { get; set; }
        public string action { get; set; }
        public string resource { get; set; }
        public List<Attribute2> attributes { get; set; }
    }

    public class UPP
    {
        public string osp_policy_id { get; set; }
        public string policy_text { get; set; }
        public string policy_url { get; set; }
        public List<Workflow> workflow { get; set; }
        public List<Policy> policies { get; set; }
    }
}
