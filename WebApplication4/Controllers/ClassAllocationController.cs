using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class ClassAllocationController : ApiController
    {
        // POST: api/Complain
        public HttpResponseMessage Post([FromBody] Class c)
        {
            try
            {
                using (ClassSMSEntities entities = new ClassSMSEntities())
                {
                    entities.Class.Add(c);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, c);
                    message.Headers.Location = new Uri(Request.RequestUri + c.class_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


    }
}
