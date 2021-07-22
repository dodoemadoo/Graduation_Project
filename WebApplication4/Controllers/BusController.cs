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
        [HttpGet]
        [Route("api/Bus")]
        public HttpResponseMessage Get()
        {
            using (BusEntities entities = new BusEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Bus.ToList());

            }
        }

        [HttpGet]
        [Route("api/Bus")]
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

        [HttpPost]
        [Route("api/Bus")]
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

        [HttpDelete]
        [Route("api/Bus")]
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

        [HttpPut]
        [Route("api/Bus")]
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

        [HttpPost]
        [Route("api/Bus/BusAllocation")]
        public HttpResponseMessage Post(int routeID, [FromBody] string busPlatNo)
        {
            try
            {
                using (BusRouteEntities entities = new BusRouteEntities())
                {
                    int busID;
                    using (BusEntities busEntities = new BusEntities())
                    {
                        Bus _bus = busEntities.Bus.FirstOrDefault(b => b.bus_number_plate.Equals(busPlatNo));
                        busID = _bus.bus_ID;
                    }
                    Bus_Route BS = entities.Bus_Route.FirstOrDefault(bs => bs.route_ID == routeID);
                    Bus_Route BS3 = entities.Bus_Route.FirstOrDefault(bs => bs.bus_ID == busID);
                    if (BS != null && BS.bus_ID == null)
                    {
                        BS.bus_ID = busID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, BS);
                    }
                    if (BS3 != null && BS3.route_ID == null)
                    {
                        BS3.route_ID = routeID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, BS3);
                    }
                    else
                    {
                        Bus_Route BS2 = new Bus_Route();
                        BS2.bus_ID = busID;
                        BS2.route_ID = routeID;
                        entities.Bus_Route.Add(BS2);
                        entities.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, BS2);
                        message.Headers.Location = new Uri(Request.RequestUri + BS2.bus_Route_ID.ToString());
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
