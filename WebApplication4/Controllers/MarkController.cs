using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class MarkController : ApiController
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Mark")]
        public HttpResponseMessage Get()
        {
            using (MarkEntities entities = new MarkEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.mark.ToList());

            }
        }

        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpGet]
        [Route("api/Mark")]
        public HttpResponseMessage Get(int mark_id)
        {
            using (MarkEntities entities = new MarkEntities())
            {
                var entity = entities.mark.FirstOrDefault(m => m.mark_ID == mark_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "mark  with id = " + mark_id.ToString() + " not found ");
                }
            }
        }

        [Authorize(Roles = "admin,teacher")]
        [HttpPost]
        [Route("api/Mark")]
        public HttpResponseMessage Post([FromBody] mark m)
        {
            try
            {
                using (MarkEntities entities = new MarkEntities())
                {
                    entities.mark.Add(m);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, m);
                    message.Headers.Location = new Uri(Request.RequestUri + m.mark_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "admin,teacher")]
        [HttpDelete]
        [Route("api/Mark")]
        public HttpResponseMessage Delete(int mark_id)
        {
            try
            {
                using (MarkEntities entities = new MarkEntities())
                {
                    var entity = entities.mark.FirstOrDefault(m => m.mark_ID == mark_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Mark with id = " + mark_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.mark.Remove(entity);
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

        [Authorize(Roles = "admin,teacher")]
        [HttpPut]
        [Route("api/Mark")]
        public HttpResponseMessage Put(int mark_id, [FromBody] mark m)
        {
            try
            {
                using (MarkEntities entities = new MarkEntities())
                {
                    var entity = entities.mark.FirstOrDefault(a => a.mark_ID == mark_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Mark with id = " + mark_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.year_wok = m.year_wok;
                        entity.final_exam_grade = m.final_exam_grade;
                        entity.student_ID = m.student_ID;
                        entity.subject_ID = m.subject_ID;
                        entity.year = m.year;
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
    }
}
