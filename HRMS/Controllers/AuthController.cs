using HRMS.DbContexts;
using HRMS.Dtos.Auth;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HRMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HRMSContext _dbContext;//= new HRMSContext();
        public AuthController(HRMSContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Login(LoginDto loginDto)
        {
            // Admin == admin
            var user = _dbContext.Users.FirstOrDefault(x => x.Username.ToUpper() == loginDto.Username.ToUpper());
            if(user == null)
            {
                return Unauthorized(new Exception("Invalid Username Or Password")); // 401
            }


            // Admin@123 == $2a$11$MhHLoubmXdikbYbxiZf.LuhXK4HBduXLNYafEOZNLslcwdMQ1gpyC => Salt, Cost Factor
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
            {
                return Unauthorized(new Exception("Invalid Username Or Password")); // 401
            }



            // Generate Token

            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(User user)
        {
            // Claims
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name , user.Username));

            // Role => Admin, Hr, Manager, Developer....
            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                var employee = _dbContext.Employees.Include(x => x.Lookups).FirstOrDefault(x => x.UserId == user.Id);
                claims.Add(new Claim(ClaimTypes.Role, employee.Lookups.Name));
            }


            // Signing Token => Secret Key
            // "WHAFWEI#!@S!!112312WQEQW@RWQEQW432" => [65,46,36....]
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("WHAFWEI#!@S!!112312WQEQW@RWQEQW432"));//;
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);



            var tokenSettings = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(1)
                );


            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(tokenSettings);
            return token;
        }
    }
}
