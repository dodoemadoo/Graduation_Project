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
        public IEnumerable<Teacher> Get()
        {
            using (TeacherEntities entities = new TeacherEntities())
            {
                return entities.Teacher.ToList();

            }
        }

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
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Teacher with id = " + id.ToString() + " not found to update");
                }
            }
        }

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
    }
}
