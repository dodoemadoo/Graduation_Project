using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class BusController : ApiController
    {
        public IEnumerable<Bus> Get()
        {
            using (BusEntities entities = new BusEntities())
            {
                return entities.Bus.ToList();

            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (BusEntities entities = new BusEntities())
            {
                var entity = entities.Bus.FirstOrDefault(B => B.bus_ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus with id = " + id.ToString() + " not found to update");
                }
            }
        }

        public HttpResponseMessage Post([FromBody] Bus B)
        {
            try
            {
                using (BusEntities entities = new BusEntities())
                {
                    entities.Bus.Add(B);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, B);
                    message.Headers.Location = new Uri(Request.RequestUri + B.bus_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (BusEntities entities = new BusEntities())
                {
                    var entity = entities.Bus.FirstOrDefault(B => B.bus_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entities.Bus.Remove(entity);
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

        public HttpResponseMessage Put(int id, [FromBody] Bus B)
        {
            try
            {
                using (BusEntities entities = new BusEntities())
                {
                    var entity = entities.Bus.FirstOrDefault(a => a.bus_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Bus with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.bus_capacity = B.bus_capacity;
                        entity.bus_number_plate = B.bus_number_plate;
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
