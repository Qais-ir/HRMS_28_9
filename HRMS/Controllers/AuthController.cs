using HRMS.DbContexts;
using HRMS.Dtos.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return Ok();
        }
    }
}
