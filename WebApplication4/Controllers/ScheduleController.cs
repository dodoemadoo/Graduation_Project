using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class ScheduleController : ApiController
    {
        [HttpGet]
        [Route("api/Schedule/Validate")]
        public HttpResponseMessage ValidateSlot(schedule slot)
        {
            using (ScheduleEntities schedule = new ScheduleEntities())
            {
                var firstCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && s.class_ID == slot.class_ID).ToList();
                var secondCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && s.teacher_subject_ID == slot.teacher_subject_ID).ToList();
                var thirdCondition = schedule.schedules.Where(s => s.class_ID == slot.class_ID && s.teacher_subject_ID == slot.teacher_subject_ID).ToList();
                if (firstCondition.Count == 0 || secondCondition.Count == 0 || thirdCondition.Count == 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, false);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, true);
            }
        }

        [HttpPost]
        [Route("api/Schedule/Insert")]
        public HttpResponseMessage InsertSlot([FromBody]schedule slot)
        {
            using (ScheduleEntities schedule = new ScheduleEntities())
            {
               if(ValidateSlot(slot).StatusCode == HttpStatusCode.OK)
               {
                    schedule.schedules.Add(slot);
                    schedule.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.OK, slot);

                    message.Headers.Location = new Uri(Request.RequestUri + slot.slot_ID.ToString());
                    return message;
                }
               else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot create this slot");
               
                //return Request.CreateResponse(HttpStatusCode.OK, "");
            }
        }
    }
}
