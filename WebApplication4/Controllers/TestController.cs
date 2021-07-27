using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class TestController : ApiController
    {
        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpGet]
        [Route("api/test/method1")]
        public HttpResponseMessage Post(User user)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value);

            user.type = roles.ToString();

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }

        //This resource is For all types of role
        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpGet]
        [Route("api/test/resource1")]
        public IHttpActionResult GetResource1()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello: " + identity.Name + "           " +identity.FindFirst("Password") + "           " + identity.FindFirst("Type") + "           " + ClaimTypes.Role);
        }
    }
}
