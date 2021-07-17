using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class ClassListController : ApiController
    {
        [HttpGet]
        public List<List<Student>> GetAlphabetically(bool isAlphabetical, List<List<Student>> list, int _grade)
        {

            using (GradeEntities gradeEntity = new GradeEntities())
            {

                using (UserEntities studentEntity = new UserEntities())
                {
                    if (!list.Equals(null))
                    {

                        if (isAlphabetical)
                        {
                            List<List<Student>> orderedStudents = new List<List<Student>>();
                            for (int i = 0; i < list.Count(); i++)
                            {
                                List<Student> newList = list.ElementAt(i).OrderBy(s => s.student_name).ToList();
                                orderedStudents.Add(newList);
                            }
                            return orderedStudents;
                        }
                        else
                        {
                            Random rand = new Random();
                            List<List<Student>> nonAlphabetical = new List<List<Student>>();
                            for (int i = 0; i < list.Count(); i++)
                            {
                                List<int> numbers = new List<int>();
                                List<Student> temp = new List<Student>();
                                do
                                {
                                    int number = rand.Next(0, list.Count());
                                    if (!numbers.Contains(number))
                                    {
                                        numbers.Add(number);
                                        temp.Add(list.ElementAt(i).ElementAt(number));

                                    }
                                } while (numbers.Count() < list.ElementAt(i).Count());
                                nonAlphabetical.Add(temp);
                            }
                            return nonAlphabetical;
                        }

                    }
                    else
                        return null;
                    /*else
                    {
                        IEnumerable<Student> studentlist = studentEntity.Students.Where(s => s.grade_ID == _grade).ToList();
                        if (isAlphabetical)
                        {
                            studentlist.OrderBy(s => s.student_name);
                            return studentlist.ToList();
                        }
                        else
                        {
                            Random rand = new Random();
                            List<int> numbers = new List<int>();
                            List<Student> nonAlphabetical = new List<Student>();

                            do
                            {
                                int number = rand.Next(0, list.Count());
                                if (!numbers.Contains(number))
                                {
                                    numbers.Add(number);
                                    nonAlphabetical.Add(studentlist.ElementAt(number));
                                }
                            } while (numbers.Count() < list.Count());

                            return nonAlphabetical;
                        }
                    }*/
                }
            }

        }

        [HttpGet]
        public List<List<Student>> GetBySecondLanguage(bool isBySecondlanguage)
        {
            using (SubjectEntities subjectEntity = new SubjectEntities())
            {
                using (MarkEntities MarkEntity = new MarkEntities())
                {
                    using (UserEntities StudentEntity = new UserEntities())
                    {


                        //  select subject id form subject where type = secondlanguage
                        //  select student id , subject id from T_S where subject id in ()
                        //  list of lists of student id in this second language

                        if (isBySecondlanguage)
                        {
                            var SubjectList = subjectEntity.Subject.Where(s => s.type == "second_language").ToList();
                            List<int> SubjectIDs = new List<int>();
                            for (int i = 0; i < SubjectList.Count(); i++)
                            {
                                SubjectIDs.Add(SubjectList.ElementAt(i).subject_id);
                            }

                            var MarkList = MarkEntity.mark.Where(s => SubjectIDs.Contains((int)s.subject_ID)).ToList();
                            //  IEnumerable<int> StudentIDs = new List<int>();
                            List<List<Student>> StudentsHaveSecondLanguage = new List<List<Student>>(SubjectIDs.Count());

                            for (int i = 0; i < SubjectIDs.Count(); i++)
                            {
                                List<Student> students = new List<Student>();
                                for (int j = 0; j < MarkList.Count(); j++)
                                {
                                    if (MarkList.ElementAt(j).subject_ID == SubjectIDs.ElementAt(i))
                                    {
                                        var student = StudentEntity.Students.FirstOrDefault(s => s.student_ID == MarkList.ElementAt(j).student_ID);
                                        students.Add(student);
                                    }
                                }
                                StudentsHaveSecondLanguage.Add(students);
                            }

                            // List<List<Student>> list = GetByGender(StudentsHaveSecondLanguage,1);

                            return StudentsHaveSecondLanguage;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        [HttpGet]
        public List<List<Student>> GetByGender(List<List<Student>> students, int _grade, bool isByGender)
        {
            if (isByGender)
            {
                using (GradeEntities gradeEntity = new GradeEntities())
                {
                    using (UserEntities studentEntity = new UserEntities())
                    {
                        if (!students.Equals(null))
                        {
                            List<List<Student>> list = new List<List<Student>>();
                            for (int i = 0; i < students.Count(); i++)
                            {
                                List<Student> males = new List<Student>();
                                List<Student> females = new List<Student>();
                                for (int j = 0; j < students.ElementAt(i).Count(); j++)
                                {
                                    if (students.ElementAt(i).ElementAt(j).student_Gender == "male")
                                    {
                                        males.Add(students.ElementAt(i).ElementAt(j));
                                    }
                                    else
                                        females.Add(students.ElementAt(i).ElementAt(j));
                                }
                                list.Add(males);
                                list.Add(females);
                            }
                            return list;
                        }
                        else
                        {
                            List<Student> males = new List<Student>();
                            males = studentEntity.Students.Where(s => s.student_Gender == "male" && s.grade_ID == _grade).ToList();
                            List<Student> females = new List<Student>();
                            females = studentEntity.Students.Where(s => s.student_Gender == "female" && s.grade_ID == _grade).ToList();

                            List<List<Student>> list = new List<List<Student>>();

                            list.Add(males);
                            list.Add(females);
                            return list;
                        }
                    }

                }
            }
            else
                return null;
        }


        [HttpGet]
        public HttpResponseMessage Get(string grade, bool isAlphabetical, bool isBySecondlanguage, bool isByGender)
        {
            using (GradeEntities gradeEntity = new GradeEntities())
            {
                Grade _grade = gradeEntity.Grade.FirstOrDefault(g => g.grade_Name == grade);
                if (_grade != null)
                {
                    using (ClassSMSEntities obj = new ClassSMSEntities())
                    {
                        var classes = obj.Classes.Where(c => c.grade_id == _grade.grade_id).ToList();
                        var bySecondLanguage = GetBySecondLanguage(isBySecondlanguage);
                        var byGender = GetByGender(bySecondLanguage, _grade.grade_id, isByGender);
                        var byAlphabetical = GetAlphabetically(isAlphabetical, byGender, _grade.grade_id);
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid grade name...!");
            }
        }




        // GET: api/ClassList/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ClassList
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ClassList/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ClassList/5
        public void Delete(int id)
        {
        }
    }
}
