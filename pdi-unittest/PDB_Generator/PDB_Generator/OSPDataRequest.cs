using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDB_Generator
{
    public class OSPDataRequest
    {
        public enum ActionEnum { Collect, Access, Use, Disclose, Archive }
        public string RequesterId { get; set; }
        public string Subject { get; set; }
        public string RequestedUrl { get; set; }
        public ActionEnum Action { get; set; }
    }
}
