using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Mayiboy.Host.Controllers
{
    public class DemoController : ApiController
    {

	    public string Get()
	    {
		    return "This Get Request";
	    }

	    public string Post()
	    {
		    return "This Post Request";
	    }
    }
}
