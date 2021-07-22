using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class ClassController : ApiController
    {
        [HttpGet]
        [Route("api/Class")]
        public HttpResponseMessage Get()
        {
            using (ClassEntities entities = new ClassEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Class.ToList());

            }
        }

        [HttpGet]
        [Route("api/Class")]
        public HttpResponseMessage Get(int id)
        {
            using (ClassEntities entities = new ClassEntities())
            {
                var entity = entities.Class.FirstOrDefault(c => c.class_ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "class  with id = " + id.ToString() + " not found to update");
                }
            }
        }

        [HttpPost]
        [Route("api/Class")]
        public HttpResponseMessage Post([FromBody] Class c)
        {
            try
            {
                using (ClassEntities entities = new ClassEntities())
                {
                    entities.Class.Add(c);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, c);
                    message.Headers.Location = new Uri(Request.RequestUri + c.class_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("api/Class")]
        public HttpResponseMessage Put(int id, [FromBody] Class c)
        {
            try
            {
                using (ClassEntities entities = new ClassEntities())
                {
                    var entity = entities.Class.FirstOrDefault(a => a.class_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "class  with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.building_ID = c.building_ID;
                        entity.class_capacity = c.class_capacity;
                        entity.class_name = c.class_name;
                        entity.class_Type = c.class_Type;
                        entity.grade_id = c.grade_id;
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

        [HttpDelete]
        [Route("api/Class")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (ClassEntities entities = new ClassEntities())
                {
                    var entity = entities.Class.FirstOrDefault(c => c.class_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "class  with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entities.Class.Remove(entity);
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

        [HttpPost]
        [Route("api/Class/ClassAllocation")]
        public HttpResponseMessage ClassAllocation([FromBody] Class c)
        {
            try
            {
                using (ClassEntities entities = new ClassEntities())
                {
                    entities.Class.Add(c);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, c);
                    message.Headers.Location = new Uri(Request.RequestUri + c.class_ID.ToString());
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
