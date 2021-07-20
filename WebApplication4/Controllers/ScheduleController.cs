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
                if (firstCondition.Count != 0 || secondCondition.Count != 0 || thirdCondition.Count != 0)
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
            }
        }

        [HttpPut]
        [Route("api/Schedule/Update")]
        public HttpResponseMessage updateSlot(int slot_id, [FromBody] schedule slot)
        {
            try
            {
                using (ScheduleEntities schedule = new ScheduleEntities())
                {
                    var entity = schedule.schedules.FirstOrDefault(s => s.slot_ID  == slot_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Mark with id = " + slot_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.week_Day = slot.week_Day;
                        entity.slot_ID = slot.slot_ID;
                        entity.class_ID = slot.class_ID;
                        entity.teacher_subject_ID = slot.teacher_subject_ID;
                        schedule.SaveChanges();
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
