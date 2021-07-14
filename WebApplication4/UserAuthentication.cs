using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.Models;

namespace WebApplication4
{
    public class UserAuthentication : IDisposable
    {
        //SECURITY_DBEntities it is your context class
        UserEntities context = new UserEntities();
        //This method is used to check and validate the user credentials
        public User ValidateUser(string username, string password)
        {
            return context.Users.FirstOrDefault(user =>
            user.userName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.password == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}