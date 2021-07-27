using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{
    public class LoginController : ApiController
    {
        [Authorize(Roles = "admin,student,parent,teacher")]
        [HttpPost]
        [Route("api/Login")]
        public HttpResponseMessage Post()
        {
            using (UserEntities userEntity = new UserEntities())
            {
                var identity = (ClaimsIdentity)User.Identity;
                int id = int.Parse(identity.Name);
                string password = identity.FindFirst("Password").Value.ToString();
                User user = userEntity.Users.FirstOrDefault(e => e.user_id == id && e.password.Equals(password));
                if (user != null)
                {
                    using (UserEntities context = new UserEntities())
                    {
                        if (user.type == "student")
                        {
                            var data = context.Users
                        .Join(
                            context.Students,
                            user_id => user_id.user_id,
                            SUser_id => SUser_id.user_id,
                            (user_id, SUser_id) => new
                            {
                                Student_ID = SUser_id.student_ID,
                                SName = SUser_id.student_name,
                                S_user_id = user_id.user_id,
                                password = user_id.password,
                                type = user_id.type
                            }
                        ).Where(e => e.S_user_id == id).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }
                        else if (user.type == "admin")
                        {
                            var data = context.Users
                        .Join(
                            context.Administrators,
                            user_id => user_id.user_id,
                            AUser_id => AUser_id.user_id,
                            (user_id, AUser_id) => new
                            {
                                administrator_id = AUser_id.administrator_id,
                                AName = AUser_id.adminstrator_Name,
                                A_user_id = user_id.user_id,
                                password = user_id.password,
                                type = user_id.type
                            }
                        ).Where(e => e.A_user_id == id).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }
                        else if (user.type == "teacher")
                        {
                            var data = context.Users
                        .Join(
                            context.Teachers,
                            user_id => user_id.user_id,
                            TUser_id => TUser_id.user_id,
                            (user_id, TUser_id) => new
                            {
                                Teacher_ID = TUser_id.teacher_id,
                                TName = TUser_id.teacher_Name,
                                S_user_id = user_id.user_id,
                                password = user_id.password,
                                type = user_id.type
                            }
                        ).Where(e => e.S_user_id == id).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }
                        else
                        {
                            var data = context.Users
                        .Join(
                            context.Parents,
                            user_id => user_id.user_id,
                            PUser_id => PUser_id.user_id,
                            (user_id, PUser_id) => new
                            {
                                Parent_ID = PUser_id.parent_id,
                                SName = PUser_id.father_Name,
                                S_user_id = user_id.user_id,
                                password = user_id.password,
                                type = user_id.type
                            }
                        ).Where(e => e.S_user_id == id).ToList();
                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Data...!");
                }
            }
        }
    }
}
