using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class TeacherController : ApiController
    {
        [HttpGet]
        [Route("api/Teacher")]
        public HttpResponseMessage Get()
        {
            using (TeacherEntities entities = new TeacherEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Teacher.ToList());

            }
        }

        [HttpGet]
        [Route("api/Teacher")]
        public HttpResponseMessage Get(int id)
        {
            using (TeacherEntities entities = new TeacherEntities())
            {
                var entity = entities.Teacher.FirstOrDefault(T => T.teacher_id == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Teacher with id = " + id.ToString() + " not found");
                }
            }
        }

        [HttpPost]
        [Route("api/Teacher")]
        public HttpResponseMessage Post([FromBody] Teacher T)
        {
            try
            {
                using (TeacherEntities entities = new TeacherEntities())
                {
                    entities.Teacher.Add(T);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, T);
                    message.Headers.Location = new Uri(Request.RequestUri + T.teacher_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/Teacher")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (TeacherEntities entities = new TeacherEntities())
                {
                    var entity = entities.Teacher.FirstOrDefault(T => T.teacher_id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Teacher with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entities.Teacher.Remove(entity);
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
        [Route("api/Teacher")]
        public HttpResponseMessage Put(int id, [FromBody] Teacher T)
        {
            try
            {
                using (TeacherEntities entities = new TeacherEntities())
                {
                    var entity = entities.Teacher.FirstOrDefault(a => a.teacher_id == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Teacher  with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.teacher_Name = T.teacher_Name;
                        entity.teacher_Speciality = T.teacher_Speciality;
                        entity.teacher_Gender = T.teacher_Gender;
                        entity.teacher_Age = T.teacher_Age;
                        entity.natinal_ID = T.natinal_ID;
                        entity.user_id = T.user_id;
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

        [HttpPost]
        [Route("api/Teacher/TeacherToClass")]
        public HttpResponseMessage TeacherToClass(int TS_ID, [FromBody] int classID)
        {
            try
            {
                using (ScheduleEntities entities = new ScheduleEntities())
                {
                        schedule entity = new schedule();
                        entity.class_ID = classID;
                        entity.teacher_subject_ID = TS_ID;
                        entities.schedules.Add(entity);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, entity);
                        message.Headers.Location = new Uri(Request.RequestUri + entity.SS_ID.ToString());
                        return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Teacher/TeacherToSubject")]
        public HttpResponseMessage TeacherToSubject([FromBody] int teacherID, [FromUri] int SubjectID)
        {
            try
            {
                using (T_SEntities entities = new T_SEntities())
                {
                    T_S ts = new T_S();
                    ts.subject_ID = SubjectID;
                    ts.teacher_ID = teacherID;
                    entities.T_S.Add(ts);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, ts);
                    message.Headers.Location = new Uri(Request.RequestUri + ts.T_S_ID.ToString());
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
