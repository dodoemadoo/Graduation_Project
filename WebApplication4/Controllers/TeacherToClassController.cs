using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class TeacherToClassController : ApiController
    {
        // POST: api/Complain
        public HttpResponseMessage Post(int TS_ID,[FromBody] string className)
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


    }
}
