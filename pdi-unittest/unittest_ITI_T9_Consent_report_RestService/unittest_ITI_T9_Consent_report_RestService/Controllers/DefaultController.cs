using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class ConsentController : ApiController
    {
        public OSPConsents Get()
        {
            return new OSPConsents
            {

            };
        }
    }

}
