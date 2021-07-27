using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class DriverAllocationController : ApiController
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Driver")]
        public HttpResponseMessage Get()
        {
            using (DriverEntities entities = new DriverEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Driver.ToList());

            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Driver")]
        public HttpResponseMessage Get(int driver_id)
        {
            using (DriverEntities entities = new DriverEntities())
            {
                var entity = entities.Driver.FirstOrDefault(d => d.driver_ID == driver_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Driver with id = " + driver_id.ToString() + " not found");
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Driver")]
        public HttpResponseMessage Post([FromBody] Driver driver)
        {
            try
            {
                using (DriverEntities entities = new DriverEntities())
                {
                    entities.Driver.Add(driver);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, driver);
                    message.Headers.Location = new Uri(Request.RequestUri + driver.driver_ID.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("api/Driver")]
        public HttpResponseMessage Delete(int driver_id)
        {
            try
            {
                using (DriverEntities entities = new DriverEntities())
                {
                    var entity = entities.Driver.FirstOrDefault(A => A.driver_ID == driver_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Driver with id = " + driver_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Driver.Remove(entity);
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

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("api/Driver")]
        public HttpResponseMessage Put(int driver_id, [FromBody] Driver driver)
        {
            try
            {
                using (DriverEntities entities = new DriverEntities())
                {
                    var entity = entities.Driver.FirstOrDefault(a => a.driver_ID == driver_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Driver with id = " + driver_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.driver_Name = driver.driver_Name;
                        entity.driver_National_ID = driver.driver_National_ID;
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

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Driver/DriverAllocation")]
        public HttpResponseMessage Post([FromBody] int driverID, [FromUri]int busID)
        {
            try
            {
                using (BusRouteEntities entities = new BusRouteEntities())
                {
                    Bus_Route BS = entities.Bus_Route.FirstOrDefault(bs => bs.bus_ID == busID);
                    if (BS != null && BS.driver_ID == null)
                    {
                        BS.driver_ID = driverID;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, BS);
                    }
                    else
                    {
                        Bus_Route BS2 = new Bus_Route();
                        BS2.bus_ID = busID;
                        BS2.driver_ID = driverID;
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
