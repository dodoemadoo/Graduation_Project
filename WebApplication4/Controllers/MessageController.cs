using WebApplication4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication4.Controllers
{
    public class MessageController : ApiController
    {
        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpGet]
        [Route("api/Message/Get_sent_messages")]
        public HttpResponseMessage Get_sent_messages(int sender_id)
        {
            using (MessageEntities entities = new MessageEntities())
            {
                List<Messages> list = entities.Messages.Where(m => m.sender_ID == sender_id).ToList();

                if (list.Count != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, list.OrderBy(x => x.time).Reverse());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"There is no messages");
                }
            }
        }

        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpGet]
        [Route("api/Message/Get_recieved_messages")]
        public HttpResponseMessage Get_recieved_messages(int reciever_id)
        {
            using (MessageEntities entities = new MessageEntities())
            {
                List<Messages> list = entities.Messages.Where(m => m.receiver_ID == reciever_id).ToList();

                if (list != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, list.OrderBy(x=>x.time).Reverse());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no messages");
                }
            }
        }

        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpPost]
        [Route("api/Message")]
        public HttpResponseMessage Post([FromBody] Messages m)
        {
            try
            {
                using (MessageEntities entities = new MessageEntities())
                {
                    Messages _message = new Messages();
                    _message.sender_ID = m.sender_ID;
                    _message.receiver_ID = m.receiver_ID;
                    _message.message_text = m.message_text;
                    _message.about_text = m.about_text;
                    _message.time = DateTime.UtcNow;
                    entities.Messages.Add(_message);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, _message);
                    message.Headers.Location = new Uri(Request.RequestUri + _message.message_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpDelete]
        [Route("api/Message")]
        public HttpResponseMessage Delete(int msg_id)
        {
            try
            {
                using (MessageEntities entities = new MessageEntities())
                {
                    var entity = entities.Messages.FirstOrDefault(M => M.message_id == msg_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Message with id = " + msg_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        entities.Messages.Remove(entity);
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

    }
}
