using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IptApis.Controller
{
    public class TestController : ApiController
    {
        public string Get()
        {
            return "Hello World";
        }
    }
}
