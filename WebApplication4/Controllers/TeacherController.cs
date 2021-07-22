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
        public IEnumerable<Teacher> Get()
        {
            using (TeacherEntities entities = new TeacherEntities())
            {
                return entities.Teacher.ToList();

            }
        }

        [HttpGet]
        [Route("api/Teacher/5")]
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
        [Route("api/Teacher/5")]
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
        [Route("api/Teacher/5")]
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
        public HttpResponseMessage TeacherToClass(int TS_ID, [FromBody] string className)
        {
            try
            {
                using (ScheduleEntities entities = new ScheduleEntities())
                {
                    int classID;
                    using (ClassEntities classEntities = new ClassEntities())
                    {
                        Class _class = classEntities.Class.FirstOrDefault(c => c.class_name.Equals(className));
                        classID = _class.class_ID;
                    }
                    schedule Sc = entities.schedules.FirstOrDefault(sc => sc.class_ID == classID);
                    schedule Sc3 = entities.schedules.FirstOrDefault(sc => sc.teacher_subject_ID == TS_ID);
                    if (Sc != null && Sc.teacher_subject_ID == null)
                    {
                        Sc.teacher_subject_ID = TS_ID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, Sc);
                    }
                    if (Sc3 != null && Sc3.class_ID == null)
                    {
                        Sc3.class_ID = classID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, Sc3);
                    }
                    else
                    {
                        schedule Sc2 = new schedule();
                        Sc2.class_ID = classID;
                        Sc2.teacher_subject_ID = TS_ID;
                        entities.schedules.Add(Sc2);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, Sc2);
                        message.Headers.Location = new Uri(Request.RequestUri + Sc2.SS_ID.ToString());
                        return message;
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        [Route("api/Teacher/TeacherToSubject")]
        public HttpResponseMessage TeacherToSubject([FromBody] string teacherName, [FromUri] string subjectName)
        {
            try
            {
                using (T_SEntities entities = new T_SEntities())
                {
                    int teacherID, SubjectID;
                    using (TeacherEntities teacher = new TeacherEntities())
                    {
                        Teacher _teacher = teacher.Teacher.FirstOrDefault(t => t.teacher_Name.Equals(teacherName));
                        teacherID = _teacher.teacher_id;
                    }
                    using (SubjectEntities subject = new SubjectEntities())
                    {
                        Subject _subject = subject.Subjects.FirstOrDefault(s => s.subject_Name.Equals(subjectName));
                        SubjectID = _subject.subject_id;
                    }
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
