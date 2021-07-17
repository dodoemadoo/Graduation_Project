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
        public List<Student> GetAlphabetically(bool isAlphabetical, IEnumerable<Student> list)
        {
            using (GradeEntities gradeEntity = new GradeEntities())
            {

                using (UserEntities studentEntity = new UserEntities())
                {
                    if (isAlphabetical)
                    {
                        IEnumerable<Student> newList = list.OrderBy(s => s.student_name);

                        return newList.ToList();
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
                                nonAlphabetical.Add(list.ElementAt(number));

                            }
                        } while (numbers.Count() < list.Count());
                        return nonAlphabetical;
                    }

                }
            }

        }

        [HttpGet]
        public HttpResponseMessage GetBySecondLanguage()
        {
            using (SubjectEntities subjectEntity = new SubjectEntities())
            {
                using (MarkEntities MarkEntity = new MarkEntities())
                {

                    //  select subject id form subject where type = secondlanguage
                    //  select student id , subject id from T_S where subject id in ()
                    //  list of lists of student id in this second language


                    var SubjectList = subjectEntity.Subject.Where(s => s.type == "second_language").ToList();
                    List<int> SubjectIDs = new List<int>();
                    for (int i = 0; i < SubjectList.Count(); i++)
                    {
                        SubjectIDs.Add(SubjectList.ElementAt(i).subject_id);
                    }
                    var MarkList = MarkEntity.mark.Where(s => SubjectIDs.Contains((int)s.subject_ID)).ToList();
                    //  IEnumerable<int> StudentIDs = new List<int>();
                    List<List<int>> StudentsHaveSecondLanguage = new List<List<int>>(SubjectIDs.Count());

                    for (int i = 0; i < SubjectIDs.Count(); i++)
                    {
                        List<int> students = new List<int>();
                        for (int j = 0; j < MarkList.Count(); j++)
                        {
                            if (MarkList.ElementAt(j).subject_ID == SubjectIDs.ElementAt(i))
                            {
                                students.Add((int)MarkList.ElementAt(j).student_ID);
                            }
                        }
                        StudentsHaveSecondLanguage.Add(students);
                    }

                    List<List<Student>> list = GetByGender(StudentsHaveSecondLanguage);

                    return Request.CreateResponse(HttpStatusCode.OK, list);


                }
            }
        }

        [HttpGet]
        public List<List<Student>> GetByGender(List<List<int>> studentIDs)
        {
            using (GradeEntities gradeEntity = new GradeEntities())
            {
                //  Grade _grade = gradeEntity.Grades.FirstOrDefault(g => g.grade_Name == grade);
                // if (_grade != null)
                // {
                using (UserEntities studentEntity = new UserEntities())
                {
                    List<List<Student>> list = new List<List<Student>>();
                    for (int i = 0; i < studentIDs.Count(); i++)
                    {
                        List<Student> males = new List<Student>();
                        List<Student> females = new List<Student>();
                        for (int j = 0; j < studentIDs.ElementAt(i).Count(); j++)
                        {
                            var student = studentEntity.Students.FirstOrDefault(s => s.student_ID == studentIDs.ElementAt(i).ElementAt(j));
                            if (student.student_Gender == "male")
                            {
                                males.Add(student);
                            }
                            else
                                females.Add(student);
                        }
                        list.Add(males);
                        list.Add(females);
                    }
                    return list;
                }

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
