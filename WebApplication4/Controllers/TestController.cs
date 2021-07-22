using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
using System.Security.Claims;

namespace WebApplication4.Controllers
{
    public class TestController : ApiController
    {
        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("api/test/method1")]
        public HttpResponseMessage Post(User myclass)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);

            myclass.type = roles.ToString();

            return Request.CreateResponse(HttpStatusCode.Created, myclass);
        }

        //This resource is For all types of role
        [Authorize(Roles = "admin,student,teacher,parent")]
        [HttpGet]
        [Route("api/test/resource1")]
        public IHttpActionResult GetResource1()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello: " + identity.Name);
        }
    }
}
