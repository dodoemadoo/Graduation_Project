using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication4
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserAuthentication OBJ = new UserAuthentication())
            {
                var user = OBJ.ValidateUser(int.Parse(context.UserName), context.Password);
                if (user == null)
                {
                    context.SetError("invalid_grant", "Username or password is incorrect");
                    return;
                }
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, user.type));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()));
                //identity.AddClaim(new Claim("Email", user.UserEmailID));  

                context.Validated(identity);
            }
        }
    }
}