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
        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/ClassList/GetAlphabetically")]
        public List<List<Student>> GetAlphabetically(bool isAlphabetical, List<List<Student>> list, int _grade)
        {

            using (GradeEntities gradeEntity = new GradeEntities())
            {

                using (UserEntities studentEntity = new UserEntities())
                {
                    if (list.Count != 0 &&  list != null)
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
                                    int number = rand.Next(0, list.Count()-1);
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
                        return list;
                }
            }

        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/ClassList/bySecondLanguage")]
        public List<List<Student>> GetBySecondLanguage(bool isBySecondlanguage)
        {
            using (SubjectEntities subjectEntity = new SubjectEntities())
            {
                using (MarkEntities MarkEntity = new MarkEntities())
                {
                    using (StudentEntities StudentEntity = new StudentEntities())
                    {


                        //  select subject id form subject where type = secondlanguage
                        //  select student id , subject id from T_S where subject id in ()
                        //  list of lists of student id in this second language

                        if (isBySecondlanguage)
                        {
                            var SubjectList = subjectEntity.Subjects.Where(s => s.type == "second_language").ToList();
                            List<int> SubjectIDs = new List<int>();
                            for (int i = 0; i < SubjectList.Count(); i++)
                            {
                                SubjectIDs.Add(SubjectList.ElementAt(i).subject_id);
                            }

                            var MarkList = MarkEntity.mark.Where(s => SubjectIDs.Contains((int)s.subject_ID)).ToList();
                            List<List<Student>> StudentsHaveSecondLanguage = new List<List<Student>>(SubjectIDs.Count());
                            List<Student> StudentList = StudentEntity.Students.ToList();
                            for (int i = 0; i < SubjectIDs.Count(); i++)
                            {
                                List<Student> students = new List<Student>();
                                for (int j = 0; j < MarkList.Count(); j++)
                                {
                                    if (MarkList.ElementAt(j).subject_ID == SubjectIDs.ElementAt(i))
                                    {
                                        Student student = new Student();
                                        for (int k = 0; k < StudentList.Count(); k++)
                                        {
                                            if (StudentList.ElementAt(k).student_ID == MarkList.ElementAt(j).student_ID)
                                            {
                                                student = StudentList.ElementAt(k);
                                                break;
                                            }
                                        }
                                        students.Add(student);
                                    }
                                }
                                StudentsHaveSecondLanguage.Add(students);
                            }

                            return StudentsHaveSecondLanguage;
                        }
                        else
                            return null;
                    }
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/ClassList/byGender")]
        public List<List<Student>> GetByGender(List<List<Student>> students, int _grade, bool isByGender)
        {
            if (isByGender)
            {
                using (GradeEntities gradeEntity = new GradeEntities())
                {
                    using (StudentEntities studentEntity = new StudentEntities())
                    {
                        if (students != null)
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
                                if(males.Count != 0)
                                    list.Add(males);
                                if (females.Count != 0)
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
                            
                            if(males.Count != 0)
                                list.Add(males);
                            if (females.Count != 0)
                                list.Add(females);
                            return list;
                        }
                    }

                }
            }
            else
                return students;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("api/ClassList")]
        public HttpResponseMessage MakeClassList([FromBody]string grade, bool isAlphabetical, bool isBySecondlanguage, bool isByGender)
        {
            using (GradeEntities gradeEntity = new GradeEntities())
            {
                Grade _grade = gradeEntity.Grade.FirstOrDefault(g => g.grade_Name == grade);
                if (_grade != null)
                {
                    using (ClassEntities obj = new ClassEntities())
                    {
                        var classes = obj.Class.Where(c => c.grade_id == _grade.grade_id).ToList();
                        if (isAlphabetical && !isBySecondlanguage && !isByGender)
                        {
                            if (Get_capacity(_grade.grade_id).StatusCode == HttpStatusCode.OK)
                            {
                                using (StudentEntities studentEntity = new StudentEntities())
                                {
                                    List<Student> studentlist = studentEntity.Students.Where(s => s.grade_ID == _grade.grade_id).ToList();
                                    studentlist.OrderBy(s => s.student_name);

                                    List<KeyValuePair<Class, List<Student>>> assignedClasses = new List<KeyValuePair<Class, List<Student>>>();
                                    while (studentlist.Count != 0)
                                    {
                                        Class c = classes.ElementAt(0);
                                        for (int k = 1; k < classes.Count(); k++)
                                        {
                                            if (classes.ElementAt(k).class_capacity > c.class_capacity)
                                            {
                                                c = classes.ElementAt(k);
                                            }
                                        }
                                        List<Student> currStudents = studentlist.GetRange(0, Math.Min(c.class_capacity, studentlist.Count));
                                        assignedClasses.Add(new KeyValuePair<Class, List<Student>>(c, currStudents));
                                        studentlist.RemoveRange(0, Math.Min(c.class_capacity, studentlist.Count));
                                        //students.RemoveRange(0, c.class_capacity);
                                        classes.Remove(c);
                                    }
                                    InsertStudentsClass(assignedClasses);
                                    return Request.CreateResponse(HttpStatusCode.OK, assignedClasses);
                                }
                            }
                            else
                                return Request.CreateResponse(HttpStatusCode.BadRequest,"Number of students not assigned to  class: " + Get_capacity(_grade.grade_id).Content.ReadAsStringAsync().Result);
                        }
                        else
                        {
                            var bySecondLanguage = GetBySecondLanguage(isBySecondlanguage);
                            var byGender = GetByGender(bySecondLanguage, _grade.grade_id, isByGender);
                            var byAlphabetical = GetAlphabetically(isAlphabetical, byGender, _grade.grade_id);
                            if (classes.Count < byAlphabetical.Count)
                            {
                                int diff = byAlphabetical.Count - classes.Count;
                                return Request.CreateResponse(HttpStatusCode.BadRequest, "Number of class list is more than the number of assigned classes: "+ diff);
                            }
                            else
                            {
                                List<KeyValuePair<Class, List<Student>>> assignedClasses = new List<KeyValuePair<Class, List<Student>>>();
                                List<List<Student>> students = new List<List<Student>>();
                                for (int i = 0; i < byAlphabetical.Count(); i++)
                                {
                                    students.Add(byAlphabetical.ElementAt(i));
                                }

                                for (int i = 0; i < byAlphabetical.Count(); i++)
                                {
                                    var currClass = new Class();
                                    List<Class> temp = new List<Class>();
                                    if (classes.Count == 0)
                                        break;

                                    for (int j = 0; j < classes.Count(); j++)
                                    {
                                        if (byAlphabetical.ElementAt(i).Count() <= classes.ElementAt(j).class_capacity)
                                            temp.Add(classes.ElementAt(j));
                                    }
                                    if (temp.Count != 0)
                                    {
                                        Class c = temp.ElementAt(0);
                                        for (int k = 1; k < temp.Count(); k++)
                                        {
                                            if (temp.ElementAt(k).class_capacity < c.class_capacity)
                                            {
                                                c = temp.ElementAt(k);
                                            }
                                        }
                                        currClass = c;
                                        assignedClasses.Add(new KeyValuePair<Class, List<Student>>(currClass, byAlphabetical.ElementAt(i)));
                                        classes.Remove(currClass);
                                        students.Remove(byAlphabetical.ElementAt(i));
                                    }
                                    else
                                    {
                                        Class c = classes.ElementAt(0);
                                        for (int k = 1; k < classes.Count(); k++)
                                        {
                                            if (classes.ElementAt(k).class_capacity > c.class_capacity)
                                            {
                                                c = classes.ElementAt(k);
                                            }
                                        }
                                        currClass = c;
                                        List<Student> currStudents = byAlphabetical.ElementAt(i).GetRange(0, currClass.class_capacity);
                                        assignedClasses.Add(new KeyValuePair<Class, List<Student>>(currClass, currStudents));
                                        byAlphabetical.ElementAt(i).RemoveRange(0, currClass.class_capacity - 1);
                                        students.ElementAt(i).RemoveRange(0, currClass.class_capacity);
                                        classes.Remove(currClass);
                                        i--;
                                    }

                                }
                                if (students.Count != 0)
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Number of students not assigned to  class: "+students.Count);
                                else
                                {
                                    InsertStudentsClass(assignedClasses);
                                    return Request.CreateResponse(HttpStatusCode.OK, assignedClasses);
                                }
                            }
                        }
                    }
                }
                else
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid grade name...!");
            }
        }

        public void InsertStudentsClass(List<KeyValuePair<Class, List<Student>>> assignedClasses)
        {
            using (StudentEntities StudentEntity = new StudentEntities())
            {
                for (int i = 0; i < assignedClasses.Count(); i++)
                {
                    for (int j = 0; j < assignedClasses.ElementAt(i).Value.Count(); j++)
                    {
                        //assignedClasses.ElementAt(i).Value.ElementAt(j).student_ID = assignedClasses.ElementAt(i).Key.class_ID;
                        int id = assignedClasses.ElementAt(i).Value.ElementAt(j).student_ID;
                        var entity = StudentEntity.Students.FirstOrDefault(s => s.student_ID == id);
                        entity.class_ID = assignedClasses.ElementAt(i).Key.class_ID;
                        StudentEntity.SaveChanges();
                    }
                }
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        [Route("api/ClassList/Get_capacity")]
        public HttpResponseMessage Get_capacity([FromBody] int gradeID)
        {
            using (ClassEntities entities = new ClassEntities())
            {
                int studentCapacity = 0, classCapacity = 0;
                using (StudentEntities studentEntities = new StudentEntities())
                {
                    IEnumerable<Student> list = studentEntities.Students.Where(s => s.grade_ID == gradeID).ToList();
                    if (list != null)
                    {
                        studentCapacity = list.Count();
                    }
                }
                IEnumerable<Class> list2 = entities.Class.Where(c => c.grade_id == gradeID).ToList();
                if (list2 != null)
                {
                    for (int i = 0; i < list2.Count(); i++)
                    {
                        classCapacity += list2.ElementAt(i).class_capacity;
                    }
                }
                int capacity = studentCapacity - classCapacity;
                if (capacity <= 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, 0);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, capacity);
                }
            }

        }
    }
} 
