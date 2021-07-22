using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class AttendenceController : ApiController
    {
        [HttpGet]
        [Route("api/Attendence")]
        public HttpResponseMessage Get()
        {
            using (AttendeceEntities entities = new AttendeceEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Attendance.ToList());

            }
        }

        [HttpGet]
        [Route("api/Attendence")]
        public HttpResponseMessage Get(int att_id)
        {
            using (AttendeceEntities entities = new AttendeceEntities())
            {
                var entity = entities.Attendance.FirstOrDefault(A => A.attendance_id == att_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Attendance with id = " + att_id.ToString() + " not found");
                }
            }
        }

        [HttpPost]
        [Route("api/Attendence")]
        public HttpResponseMessage Post([FromBody] Attendance att)
        {
            try
            {
                using (AttendeceEntities entities = new AttendeceEntities())
                {
                    entities.Attendance.Add(att);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, att);
                    message.Headers.Location = new Uri(Request.RequestUri + att.attendance_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/Attendence")]
        public HttpResponseMessage Delete(int att_id)
        {
            try
            {
                using (AttendeceEntities entities = new AttendeceEntities())
                {
                    var entity = entities.Attendance.FirstOrDefault(A => A.attendance_id == att_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Attendance with id = " + att_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Attendance.Remove(entity);
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

        [HttpPut]
        [Route("api/Attendence")]
        public HttpResponseMessage Put(int att_id, [FromBody] Attendance att)
        {
            try
            {
                using (AttendeceEntities entities = new AttendeceEntities())
                {
                    var entity = entities.Attendance.FirstOrDefault(a => a.attendance_id == att_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Attendance with id = " + att_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.student_id = att.student_id;
                        entity.SS_ID = att.SS_ID;
                        entity.status = att.status;
                        entity.adate = att.adate;
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
