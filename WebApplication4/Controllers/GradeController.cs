using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class GradeController : ApiController
    {
        [HttpGet]
        [Route("api/Grade")]
        public HttpResponseMessage Get()
        {
            using (GradeEntities entities = new GradeEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Grade.ToList());

            }
        }

        public HttpResponseMessage Get(int grade_id)
        {
            using (GradeEntities entities = new GradeEntities())
            {
                var entity = entities.Grade.FirstOrDefault(g => g.grade_id == grade_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Grade with id = " + grade_id.ToString() + " not found");
                }
            }
        }

        [HttpPost]
        [Route("api/Grade")]
        public HttpResponseMessage Post([FromBody] Grade grade)
        {
            try
            {
                using (GradeEntities entities = new GradeEntities())
                {
                    entities.Grade.Add(grade);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, grade);
                    message.Headers.Location = new Uri(Request.RequestUri + grade.grade_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/Grade")]
        public HttpResponseMessage Delete(int grade_id)
        {
            try
            {
                using (GradeEntities entities = new GradeEntities())
                {
                    var entity = entities.Grade.FirstOrDefault(A => A.grade_id == grade_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Grade with id = " + grade_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Grade.Remove(entity);
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
        [Route("api/Grade")]
        public HttpResponseMessage Put(int grade_id, [FromBody] Grade grade)
        {
            try
            {
                using (GradeEntities entities = new GradeEntities())
                {
                    var entity = entities.Grade.FirstOrDefault(a => a.grade_id == grade_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "grade with id = " + grade_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.grade_Capcity = grade.grade_Capcity;
                        entity.grade_Name = grade.grade_Name;
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
