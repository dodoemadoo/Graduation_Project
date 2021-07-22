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

        [HttpGet]
        [Route("api/Class/Get_nonAssigned_classes")]
        public HttpResponseMessage Get_nonAssigned_classes()
        {
            using (ClassEntities entities = new ClassEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Class.Where(c => c.grade_id == null).ToList());
            }
        }

        [HttpGet]
        [Route("api/Class/Get_Classes_for_Grade")]
        public HttpResponseMessage Get_Classes_for_Grade()
        {
            using (ClassEntities entities = new ClassEntities())
            {
                IEnumerable<Class> list = entities.Class.ToList();
                List<KeyValuePair<string, string>> list2 = new List<KeyValuePair<string, string>>();
                string gradeName;
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list.ElementAt(i).grade_id != null)
                    {
                        using (GradeEntities gradeEntities = new GradeEntities())
                        {
                            int gradeId = (int)list.ElementAt(i).grade_id;
                            Grade _grade = gradeEntities.Grade.FirstOrDefault(g => g.grade_id == gradeId);
                            gradeName = _grade.grade_Name;
                        }
                        list2.Add(new KeyValuePair<string, string>(list.ElementAt(i).class_name, gradeName));
                    }
                    else
                    {
                        list2.Add(new KeyValuePair<string, string>(list.ElementAt(i).class_name, "Not Assigned"));
                    }

                }
                return Request.CreateResponse(HttpStatusCode.OK, list2.ToList());
            }
        }

        [HttpGet]
        [Route("api/Class/Get_capacity")]
        public HttpResponseMessage Get_capacity([FromBody] int gradeID)
        {
            using (ClassEntities entities = new ClassEntities())
            {
                int studentCapacity = 0, classCapacity = 0;
                using (StudentEntities studentEntities = new StudentEntities())
                {
                    IEnumerable<Student> list = studentEntities.Students.Where(s => s.grade_ID == gradeID).ToList();
                    if (list != null)
                    {
                        studentCapacity = list.Count();
                    }
                }
                IEnumerable<Class> list2 = entities.Class.Where(c => c.grade_id == gradeID).ToList();
                if (list2 != null)
                {
                    for (int i = 0; i < list2.Count(); i++)
                    {
                        classCapacity += list2.ElementAt(i).class_capacity;
                    }
                }
                int capacity = studentCapacity - classCapacity;
                if (capacity <= 0)
                {
                    bool check = true;
                    return Request.CreateResponse(HttpStatusCode.OK, check);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, capacity);
                }
            }

        }
    }
}
