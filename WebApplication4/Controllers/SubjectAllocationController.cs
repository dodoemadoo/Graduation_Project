using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class SubjectAllocationController : ApiController
    {
        // POST: api/Complain
        public HttpResponseMessage Post([FromBody] Subject sub)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    entities.Subjects.Add(sub);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, sub);
                    message.Headers.Location = new Uri(Request.RequestUri + sub.subject_id.ToString());
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
