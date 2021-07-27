using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class ReportController : ApiController
    {
        [HttpGet]
        [Route("api/GenerateReport/Attendence")]
        public HttpResponseMessage Get(string type, bool AllGrades, string period, string gradeName)
        {
            int Attended = 0, AttendencePercentage = 0, ssID = 0;
            IEnumerable<Student> list = new List<Student>();
            IEnumerable<schedule> list_SS = new List<schedule>();
            string message = null;
            if ((type.Equals("attendence")) && (AllGrades.Equals(false)))
            {
                using (GradeEntities entities = new GradeEntities())
                {
                    var entity = entities.Grade.FirstOrDefault(A => A.grade_Name.Equals(gradeName));
                    int gradeID = entity.grade_id;
                    using (AttendeceEntities attEntities = new AttendeceEntities())
                    {
                        using (StudentEntities studentEntities = new StudentEntities())
                        {
                            list = studentEntities.Students.Where(s => s.grade_ID == gradeID).ToList();
                        }
                        using (ScheduleEntities scheduleEntities = new ScheduleEntities())
                        {
                            if (period.Equals("whole year"))
                            {
                                list_SS = scheduleEntities.schedules.ToList();
                            }
                            else
                            {
                                list_SS = scheduleEntities.schedules.Where(s => s.semester == period).ToList();

                            }
                        }
                        for (int j = 0; j < list_SS.Count(); j++)
                        {
                            ssID = list_SS.ElementAt(j).SS_ID;
                            for (int i = 0; i < list.Count(); i++)
                            {
                                int stud_id = list.ElementAt(i).student_ID;
                                var att = attEntities.Attendance.FirstOrDefault(A => A.student_id == stud_id && A.SS_ID == ssID);
                                if (att != null)
                                {
                                    Attended += 1;
                                }
                            }
                        }
                    }
                }
                message = "Percentage of students attendence in grade " + gradeName + " during this period is: ";
            }
            if ((type.Equals("attendence")) && (AllGrades.Equals(true)))
            {
                using (AttendeceEntities attEntities = new AttendeceEntities())
                {
                    using (StudentEntities studentEntities = new StudentEntities())
                    {
                        list = studentEntities.Students.ToList();
                    }
                    using (ScheduleEntities scheduleEntities = new ScheduleEntities())
                    {
                        if (period.Equals("whole year"))
                        {
                            list_SS = scheduleEntities.schedules.ToList();
                        }
                        else
                        {
                            list_SS = scheduleEntities.schedules.Where(s => s.semester == period).ToList();

                        }
                    }
                    for (int j = 0; j < list_SS.Count(); j++)
                    {
                        ssID = list_SS.ElementAt(j).SS_ID;
                        for (int i = 0; i < list.Count(); i++)
                        {
                            int stud_id = list.ElementAt(i).student_ID;
                            var att = attEntities.Attendance.FirstOrDefault(A => A.student_id == stud_id && A.SS_ID == ssID);
                            if (att != null)
                            {
                                Attended += 1;
                            }
                        }
                    }
                }
                message = "Percentage of students attendence in All grades during this period is: ";
            }
            AttendencePercentage = (Attended * 100) / list.Count();
            return Request.CreateResponse(HttpStatusCode.OK, message + AttendencePercentage + "%");
        }
        [HttpGet]
        [Route("api/GenerateReport/TeacherNeed")]
        public HttpResponseMessage GenerateTeacherNeedReport()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
