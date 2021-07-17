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

        public IEnumerable<Messages> Get_sent_messages(int sender_id)
        {
            using (MessageEntities entities = new MessageEntities())
            {
                IEnumerable<Messages> list = entities.Messages.Where(m => m.sender_ID == sender_id).ToList();

                if (list != null)
                {
                    return list.OrderBy(x => x.time).Reverse();
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<Messages> Get_recieved_messages(int reciever_id)
        {
            using (MessageEntities entities = new MessageEntities())
            {
                IEnumerable<Messages> list = entities.Messages.Where(m => m.receiver_ID == reciever_id).ToList();

                if (list != null)
                {
                    return list.OrderBy(x=>x.time).Reverse();
                }
                else
                {
                    return null;
                }
            }
        }

        public HttpResponseMessage Post(int sender_id, int reciever_id, [FromBody] string text, [FromUri] string about)
        {
            try
            {
                using (MessageEntities entities = new MessageEntities())
                {
                    Messages _message = new Messages();
                    _message.sender_ID = sender_id;
                    _message.receiver_ID = reciever_id;
                    _message.message_text = text;
                    _message.about_text = about;
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
