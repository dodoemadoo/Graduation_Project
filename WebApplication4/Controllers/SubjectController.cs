using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class SubjectController : ApiController
    {
        public IEnumerable<Subject> Get()
        {
            using (SubjectEntities entities = new SubjectEntities())
            {
                return entities.Subject.ToList();

            }
        }

        public HttpResponseMessage Get(int sub_id)
        {
            using (SubjectEntities entities = new SubjectEntities())
            {
                var entity = entities.Subject.FirstOrDefault(A => A.subject_id == sub_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Subject with id = " + sub_id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Subject sub)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    entities.Subject.Add(sub);
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

        public HttpResponseMessage Delete(int sub_id)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    var entity = entities.Subject.FirstOrDefault(A => A.subject_id == sub_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "subject with id = " + sub_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Subject.Remove(entity);
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

        public HttpResponseMessage Put(int sub_id, [FromBody] Subject s)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    var entity = entities.Subject.FirstOrDefault(a => a.subject_id == sub_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Subject with id = " + sub_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.subject_Name = s.subject_Name;
                        entity.subject_code = s.subject_code;
                        entity.classes_per_week = s.classes_per_week;
                        entity.total_grade = s.total_grade;
                        entity.year_wok = s.year_wok;
                        entity.final_exam_grade = s.final_exam_grade;
                        entity.grade_ID = s.grade_ID;
                        entity.type = s.type;
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
