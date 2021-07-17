using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class BuildingController : ApiController
    {
        public IEnumerable<Building> Get()
        {
            using (BuildingEntities entities = new BuildingEntities())
            {
                return entities.Building.ToList();

            }
        }

        public HttpResponseMessage Get(int building_id)
        {
            using (BuildingEntities entities = new BuildingEntities())
            {
                var entity = entities.Building.FirstOrDefault(B => B.building_ID== building_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Building with id = " + building_id.ToString() + " not found");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Building B)
        {
            try
            {
                using (BuildingEntities entities = new BuildingEntities ())
                {
                    entities.Building.Add(B);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, B);
                    message.Headers.Location = new Uri(Request.RequestUri + B.building_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int Building_id)
        {
            try
            {
                using (BuildingEntities entities = new BuildingEntities())
                {
                    var entity = entities.Building.FirstOrDefault(B => B.building_ID == Building_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Building with id = " + Building_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Building.Remove(entity);
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

        public HttpResponseMessage Put(int building_id, [FromBody] Building b)
        {
            try
            {
                using (BuildingEntities entities = new BuildingEntities())
                {
                    var entity = entities.Building.FirstOrDefault(a => a.building_ID == building_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Attendance with id = " + building_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.building_Name = b.building_Name;
                        entity.building_location = b.building_location;
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
