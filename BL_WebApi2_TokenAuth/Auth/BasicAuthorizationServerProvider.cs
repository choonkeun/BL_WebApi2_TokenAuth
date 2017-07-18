using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;
using BL_WebApi2_TokenAuth.Auth;
using BL_WebApi2_TokenAuth.Models;
using System.Collections.Generic;

namespace BL_WebApi2_TokenAuth
{
    public class BasicAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {

        static string ConnString = string.Empty;
        static BasicAuthorizationServerProvider() 
        {
            //<add name="ConnString" connectionString="data source=(LocalDB)\v11.0;Integrated Security=True;AttachDbFilename=|DataDirectory|\App_Data\NORTHWND.MDF;" providerName="System.Data.SqlClient" />
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain.CurrentDomain.SetData("DataDirectory", baseDir);      //--WCFBasicService/App_Data
            ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // request: Content-Type: x-www-form-urlencoded, username=Davolio&password=Nancy&grant_type=password
            context.Validated(); 
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            Employee employee = EmployeeRepository.GetEmployeeByUserName(ConnString, context.UserName, context.Password);

            if (employee.ReportsTo < 2)  //admin
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("username", "admin"));
                identity.AddClaim(new Claim("title", employee.Title));
                identity.AddClaim(new Claim("employeeId", employee.EmployeeID.ToString()));
                context.Validated(identity);
            }
            else if (employee.ReportsTo < 5)  //user
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                identity.AddClaim(new Claim("username", "user"));
                identity.AddClaim(new Claim("title", employee.Title));
                identity.AddClaim(new Claim("employeeId", employee.EmployeeID.ToString()));
                context.Validated(identity);

            }
            //if (context.UserName == "admin" && context.Password == "admin")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            //    identity.AddClaim(new Claim("username", "admin"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "test admin"));
            //    context.Validated(identity);
            //}
            //else if (context.UserName == "user" && context.Password == "user")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "test user"));
            //    context.Validated(identity);
            //}
            else
            {
                context.SetError("invalid_grant", "Provided username and password is incorrect");
                return;
            }
        }
    }
}