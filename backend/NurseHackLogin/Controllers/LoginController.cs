using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NurseHackLogin.ModelDB;
using NurseHackLogin.Models.Request;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Microsoft.AspNetCore.Components.RouteAttribute;

namespace NurseHackLogin.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LoginController: ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        [HttpPost("/api/account/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            try
            {
                nursehackdbContext context = new nursehackdbContext();
                var resp = await context.UserAuth.FirstOrDefaultAsync(i => i.Email == login.User && i.Password == login.Password);

                if (resp == null)
                {
                    return Unauthorized();
                }

                var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])), SecurityAlgorithms.HmacSha256);
                int hours = Convert.ToInt16((5));
                var exp = DateTime.UtcNow.AddHours(hours);
                var header = new JwtHeader(signingCredentials);
                //var payload = new JwtPayload(_configuration["Token:Issuer"], null, new List<Claim> { new Claim("role", login.Role) }, null, exp, DateTime.UtcNow);
                var payload = new JwtPayload("nurseHack2020", null, new List<Claim>(), null, exp, DateTime.UtcNow);

                // var contextUser = new Dictionary<string, object> { { "enterpriseToken", user.EnterpriseToken }, { "entepriseId", user.Id }, { "enterpriseNit", user.PartyIdentificationId } };
                var contextUser = new Dictionary<string, object> { { "user", login.User }, { "user_id", resp.UserId } };
                var principalContext = new Dictionary<string, object> { { "user", contextUser } };
                payload.Add("context", principalContext);

                var token = new JwtSecurityToken(header, payload);
                return Ok(new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(token), PasswordExpiration = exp });

            } catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }

        }
}
