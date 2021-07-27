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
        [Authorize(Roles = "admin")]
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
                    var firstCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && s.class_ID == slot.class_ID && s.semester == slot.semester).ToList();
                    var secondCondition = schedule.schedules.Where(s => s.slot_ID == slot.slot_ID && s.week_Day == slot.week_Day && teacherSubjectsIDs.Contains((int)s.teacher_subject_ID) && s.semester == slot.semester).ToList();
                    if (firstCondition.Count != 0 || secondCondition.Count != 0)
                        return Request.CreateResponse(HttpStatusCode.BadRequest, false);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, true);
                }
            }
        }

        [Authorize(Roles = "admin,teacher,student,parent")]
        [HttpGet]
        [Route("api/Schedule")]
        public HttpResponseMessage getClassSchedule(int class_ID)
        {
            using (ScheduleJoinEntities obj = new ScheduleJoinEntities())
            {
                var query = from s in obj.schedules
                            join slot in obj.Slots on s.slot_ID equals slot.slot_ID
                            join Teacher_Subject in obj.T_S on s.teacher_subject_ID equals Teacher_Subject.T_S_ID
                            join teacher in obj.Teachers on Teacher_Subject.teacher_ID equals teacher.teacher_id
                            join subject in obj.Subjects on Teacher_Subject.subject_ID equals subject.subject_id
                            where s.class_ID == class_ID
                            select new
                            {
                                s.SS_ID,
                                s.week_Day,
                                slot.slot_ID,
                                slot.slot_Name,
                                slot.slot_FromTime,
                                slot.slot_ToTime,
                                Teacher_Subject.T_S_ID,
                                teacher.teacher_id,
                                teacher.teacher_Name,
                                subject.subject_id,
                                subject.subject_Name,
                                s.semester
                            };
                return Request.CreateResponse(HttpStatusCode.OK, query.ToList());
            }
            /*using (ClassEntities obj = new ClassEntities())
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
            }*/
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/Schedule/Insert")]
        public HttpResponseMessage InsertSlot([FromBody]schedule slot)
        {
            using (ScheduleEntities schedule = new ScheduleEntities())
            {
                var entity = schedule.schedules.FirstOrDefault(s => s.teacher_subject_ID == slot.teacher_subject_ID && s.class_ID == slot.class_ID && s.week_Day == null );
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "You have insereted this subject before");
                }
                else
                {
                    entity.week_Day = slot.week_Day;
                    entity.slot_ID = slot.slot_ID;
                    entity.class_ID = slot.class_ID;
                    entity.teacher_subject_ID = slot.teacher_subject_ID;
                    entity.semester = slot.semester;
                    if (ValidateSlot(entity).StatusCode == HttpStatusCode.OK)
                    {
                        schedule.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot create this slot");
                }
                /*if(ValidateSlot(slot).StatusCode == HttpStatusCode.OK)
                {
                     schedule.schedules.Add(slot);
                     schedule.SaveChanges();

                     var message = Request.CreateResponse(HttpStatusCode.OK, slot);

                     message.Headers.Location = new Uri(Request.RequestUri + slot.slot_ID.ToString());
                     return message;
                 }
                else
                     return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot create this slot");*/
            }

        }

        [Authorize(Roles = "admin")]
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
                        entity.semester = slot.semester;
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

        [Authorize(Roles = "admin")]
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
