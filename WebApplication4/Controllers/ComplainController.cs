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
        // GET: api/Complain
        public HttpResponseMessage Get()
        {
            using (ComplainSMSEntities entities = new ComplainSMSEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Complain.ToList());

            }
        }

        // GET: api/Complain/5
        public HttpResponseMessage Get(int comp_id)
        {
            using (ComplainSMSEntities entities = new ComplainSMSEntities())
            {
                var entity = entities.Complain.FirstOrDefault(c => c.complain_id == comp_id);

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

        // POST: api/Complain
        public HttpResponseMessage Post([FromBody] Complain comp)
        {
            try
            {
                using (ComplainSMSEntities entities = new ComplainSMSEntities())
                {
                    entities.Complain.Add(comp);
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

        // PUT: api/Complain/5
        public HttpResponseMessage Put(int comp_id, [FromBody] Complain comp)
        {
            try
            {
                using (ComplainSMSEntities entities = new ComplainSMSEntities())
                {
                    var entity = entities.Complain.FirstOrDefault(c => c.complain_id == comp_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complain with id = " + comp_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.complain_Info = comp.complain_Info;
                        entity.parent_ID = comp.parent_ID;
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

        // DELETE: api/Complain/5
        public HttpResponseMessage Delete(int comp_id)
        {
            try
            {
                using (ComplainSMSEntities entities = new ComplainSMSEntities())
                {
                    var entity = entities.Complain.FirstOrDefault(c => c.complain_id == comp_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complain with id = " + comp_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Complain.Remove(entity);
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
