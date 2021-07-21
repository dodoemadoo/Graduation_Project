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
                using (T_SEntities obj = new T_SEntities())
                {
                    var teacher = obj.T_S.FirstOrDefault(t => t.T_S_ID == slot.teacher_subject_ID);
                    var teacherSubjects = obj.T_S.Where(t => t.teacher_ID == teacher.teacher_ID).ToList();
                    List<int> teacherSubjectsIDs = new List<int>();
                    for (int i = 0; i < teacherSubjects.Count(); i++)
                    {
                        teacherSubjectsIDs.Add(teacherSubjects.ElementAt(i).T_S_ID);
                    }
                    var firstCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && s.class_ID == slot.class_ID).ToList();
                    var secondCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && teacherSubjectsIDs.Contains((int)s.teacher_subject_ID)).ToList();
                    if (firstCondition.Count != 0 || secondCondition.Count != 0)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, false);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }
        }

        [HttpGet]
        [Route("api/Schedule/Validate")]
        public HttpResponseMessage getClassSchedule(int class_ID)
        {
            using (ClassEntities obj = new ClassEntities())
            {
                using (ScheduleEntities schedule = new ScheduleEntities())
                {
                    var entity = obj.Class.FirstOrDefault(c => c.class_ID == class_ID);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Class with id = " + class_ID.ToString() + " not found");
                    }
                    else
                    {
                        var class_Schedule = schedule.schedules.Where(s => s.class_ID == class_ID).OrderBy(s=> s.week_Day);
                        class_Schedule.OrderBy(s => s.slot_ID);
                        return Request.CreateResponse(HttpStatusCode.OK, class_Schedule);
                    }
                }
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
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "slot with id = " + slot_id.ToString() + " not found to update");
                    }
                    else
                    {
                        entity.week_Day = slot.week_Day;
                        entity.slot_ID = slot.slot_ID;
                        entity.class_ID = slot.class_ID;
                        entity.teacher_subject_ID = slot.teacher_subject_ID;
                        if (ValidateSlot(entity).StatusCode == HttpStatusCode.OK)
                        {
                            schedule.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, entity);
                        }
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot update this slot");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/Schedule/Update")]
        public HttpResponseMessage deleteSlot(int slot_id)
        {
            try
            {
                using (ScheduleEntities schedule = new ScheduleEntities())
                {
                    var entity = schedule.schedules.FirstOrDefault(s => s.slot_ID== slot_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Slot with id = " + slot_id.ToString() + " not found to delete");
                    }
                    else
                    {
                        schedule.schedules.Remove(entity);
                        schedule.SaveChanges();
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
