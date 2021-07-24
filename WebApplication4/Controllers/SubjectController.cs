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
        [HttpGet]
        [Route("api/Subject")]
        public HttpResponseMessage Get()
        {
            using (SubjectEntities entities = new SubjectEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Subjects.ToList());
            }
        }

        [HttpGet]
        [Route("api/Subject")]
        public HttpResponseMessage Get(int sub_id)
        {
            using (SubjectEntities entities = new SubjectEntities())
            {
                var entity = entities.Subjects.FirstOrDefault(A => A.subject_id == sub_id);

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

        [HttpPost]
        [Route("api/Subject")]
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

        [HttpDelete]
        [Route("api/Subject")]
        public HttpResponseMessage Delete(int sub_id)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    var entity = entities.Subjects.FirstOrDefault(A => A.subject_id == sub_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "subject with id = " + sub_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Subjects.Remove(entity);
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
        [Route("api/Subject")]
        public HttpResponseMessage Put(int sub_id, [FromBody] Subject s)
        {
            try
            {
                using (SubjectEntities entities = new SubjectEntities())
                {
                    var entity = entities.Subjects.FirstOrDefault(a => a.subject_id == sub_id);
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
                        entity.semester = s.semester;
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

        [HttpPut]
        [Route("api/Subject/GetGradeSubjects")]
        public HttpResponseMessage GetGradeSubjects(int gradeID)
        {
            using (SubjectEntities entities = new SubjectEntities())
            {
                IEnumerable<Subject> entity = entities.Subjects.Where(S => S.grade_ID == gradeID).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, entity.ToList());
            }
        }
    }
}
