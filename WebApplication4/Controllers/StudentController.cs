using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class StudentController : ApiController
    {
        [HttpPut]
        [Route("api/Student/UpdateStudentGrade")]
        public HttpResponseMessage UpdateStudentGrade()
        {
            try
            {
                using (StudentEntities entities = new StudentEntities())
                {
                    IEnumerable<Student> entity = entities.Students.ToList();
                    for (int i = 0; i < entity.Count(); i++)
                    {
                        if (entity.ElementAt(i).grade_ID != 13)
                        {
                            entity.ElementAt(i).grade_ID = entity.ElementAt(i).grade_ID + 1;
                            entities.SaveChanges();
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, entities.Students.ToList());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        [Route("api/Student/GetClassStudents")]
        public HttpResponseMessage GetClassStudents(int classID)
        {
            using (StudentEntities entities = new StudentEntities())
            {
                IEnumerable<Student> entity = entities.Students.Where(S => S.class_ID == classID).ToList();

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity.ToList());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No students in this class.");
                }
            }
        }
    }
}
