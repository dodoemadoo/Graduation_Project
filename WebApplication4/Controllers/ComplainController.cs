using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class ComplainController : ApiController
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Complain")]
        public HttpResponseMessage Get()
        {
            using (ComplainEntities entities = new ComplainEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Complains.ToList());

            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Complain")]
        public HttpResponseMessage Get(int comp_id)
        {
            using (ComplainEntities entities = new ComplainEntities())
            {
                var entity = entities.Complains.FirstOrDefault(c => c.complain_id == comp_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Complain with id = " + comp_id.ToString() + " not found ");
                }
            }
        }

        [Authorize(Roles = "student")]
        [HttpPost]
        [Route("api/Complain")]
        public HttpResponseMessage Post([FromBody] Complain comp)
        {
            try
            {
                using (ComplainEntities entities = new ComplainEntities())
                {
                    entities.Complains.Add(comp);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, comp);
                    message.Headers.Location = new Uri(Request.RequestUri + comp.complain_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("api/Complain")]
        public HttpResponseMessage Put(int comp_id, [FromBody] Complain comp)
        {
            try
            {
                using (ComplainEntities entities = new ComplainEntities())
                {
                    var entity = entities.Complains.FirstOrDefault(c => c.complain_id == comp_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complain with id = " + comp_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.complain_Info = comp.complain_Info;
                        entity.student_ID = comp.student_ID;
                        entity.administrator_ID = comp.administrator_ID;
                        entity.Status = comp.Status;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("api/Complain")]
        public HttpResponseMessage Delete(int comp_id)
        {
            try
            {
                using (ComplainEntities entities = new ComplainEntities())
                {
                    var entity = entities.Complains.FirstOrDefault(c => c.complain_id == comp_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complain with id = " + comp_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Complains.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
