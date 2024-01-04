using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RD.API.ViewModels;
using RD_Angular_Core.Server.ViewModels;
using SampleProject.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RD_Angular_Core.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private IConfiguration Configuration;
        private readonly IUsersServices UsersManager;

        LdapAuthentication ldap = new LdapAuthentication("LDAP://172.29.29.188/CN=users,DC=esupport,DC=net");

        public UserAccountController(IUsersServices UsersManage, IConfiguration Configuration)
        {
            this.Configuration = Configuration;
            this.UsersManager = UsersManage;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginVM model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Username))
                {
                    var user = AuthenticateUser(model);
                    var userSystem = UsersManager.Get(p => p.Roles).Where(x => x.userName == user.userName).FirstOrDefault();
                    user = new UserViewModel { userName = userSystem.userName, userGroup = userSystem.Roles.Title, groupId = 0 };
                    if (userSystem != null)
                    {
                        string token = GenerateJsonWebToken(user);
                        return Ok(new { status = true, token = token, userData = user });
                    }
                    return Ok(new { status = false, error = "Invalid User Data" });
                }
                return Ok(new { status = false, error = "Invalid User Data" });

            }
            catch (Exception ex)
            {

                return Ok(new { status = false, error = ex.Message });
            }

        }

        private UserViewModel AuthenticateUser(LoginVM login)
        {
            UserViewModel user = null;

            if (ldap.IsAuthenticated(login.Username, login.Password))
            {
                string spltgroups = ldap.GetGroups("esupport", login.Username, login.Password);
                string[] groups = spltgroups.Split('|');
                UserViewModel _user = new UserViewModel();
                _user.userName = login.Username;
                user = _user;
            }

            return user;
        }

        private string GenerateJsonWebToken(UserViewModel userinfo)
        {
            // _config.GetValue("Token:Key")

            var test = Configuration["Jwt:Key"];
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256Signature);
            DateTime Epirationdate = DateTime.Now.AddHours(6);

            var claims = new[]
            {
             new Claim("userName",userinfo.userName),
             new Claim("userGroup",userinfo.userGroup),
             // new Claim("groupId",userinfo.groupId.ToString()),
             new Claim("expirationDate",Epirationdate.ToString()),
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
     };

            var token = new JwtSecurityToken(
                issuer: Configuration["Jwt:issuer"],
                audience: Configuration["Jwt:audience"],
                claims: claims,
                expires: Epirationdate,
            signingCredentials: Credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateToken(UserViewModel user)
        {
            List<Claim> claims = new List<Claim>
         {
             new Claim(ClaimTypes.NameIdentifier, user.userName),
             new Claim(ClaimTypes.Name, user.userGroup)
         };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration["Jwt:Key"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            var user = User as ClaimsPrincipal;
            var identity = user.Identity as ClaimsIdentity;

            var claim = (from c in user.Claims
                         select c).ToList();
            foreach (var item in claim)
            {
                identity.RemoveClaim(item);
            }

            var currentUser = HttpContext.User;
            if (currentUser.HasClaim(c => c.Type == "userName"))
            {
                var claimitem = currentUser.Claims.FirstOrDefault(c => c.Type == "userName");
                identity.RemoveClaim(claimitem);

            }
            return Ok();
        }

    }

}
