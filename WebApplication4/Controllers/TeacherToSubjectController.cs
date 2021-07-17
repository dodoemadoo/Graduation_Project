using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class TeacherToSubjectController : ApiController
    {
        // POST: api/Complain
        public HttpResponseMessage Post([FromBody] string teacherName, [FromUri]string subjectName)
        {
            try
            {
                using (T_SEntities entities = new T_SEntities())
                {
                    int teacherID, SubjectID;
                    using(TeacherEntities teacher = new TeacherEntities())
                    {
                        Teacher _teacher = teacher.Teacher.FirstOrDefault(t => t.teacher_Name.Equals(teacherName));
                        teacherID = _teacher.teacher_id;
                    }
                    using (SubjectEntities subject = new SubjectEntities())
                    {
                        Subject _subject = subject.Subject.FirstOrDefault(s => s.subject_Name.Equals(subjectName));
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
