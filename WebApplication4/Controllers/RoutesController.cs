using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class RoutesController : ApiController
    {
        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Routes")]
        public HttpResponseMessage Get()
        {
            using (RouteEntities entities = new RouteEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Route.ToList());

            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/Routes")]
        public HttpResponseMessage Get(int id)
        {
            using (RouteEntities entities = new RouteEntities())
            {
                var entity = entities.Route.FirstOrDefault(R => R.route_ID == id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route  with id = " + id.ToString() + " not found");
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Routes")]
        public HttpResponseMessage Post([FromBody] Route R)
        {
            try
            {
                using (RouteEntities entities = new RouteEntities())
                {
                    entities.Route.Add(R);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, R);
                    message.Headers.Location = new Uri(Request.RequestUri + R.route_ID.ToString());
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
        [Route("api/Routes")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (RouteEntities entities = new RouteEntities())
                {
                    var entity = entities.Route.FirstOrDefault(R => R.route_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entities.Route.Remove(entity);
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
        [Route("api/Routes")]
        public HttpResponseMessage Put(int id, [FromBody] Route R)
        {
            try
            {
                using (RouteEntities entities = new RouteEntities())
                {
                    var entity = entities.Route.FirstOrDefault(a => a.route_ID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Route  with id = " + id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.starting_point = R.starting_point;
                        entity.ending_point = R.ending_point;
                        entity.route_Description = R.route_Description;
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
