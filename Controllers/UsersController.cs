using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using WearHouse.Data;
using WearHouse.Models;

namespace WearHouse.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly WearHouseDbContext dbContext;

        public UsersController(WearHouseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await dbContext.Users.Where(u => u.Email == request.Email).SingleOrDefaultAsync();
            if (user != null)
            {
                if (user.Password == request.Password)
                {
                    return Ok(user);
                }
            }
            return NotFound();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var isEmailValid = new EmailAddressAttribute().IsValid(request.Email);

            string passwordPattern = @"^(?=.*\d)(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~]).{8,}$";
            var isPasswordValid = request.Password == request.ConfirmPassword 
                && request.Password.Length >= 8 
                && Regex.IsMatch(request.Password, passwordPattern)
                ;
            if (isEmailValid && isPasswordValid)
            {
                var user = new User()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = request.Password,
                };

                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();

                return Created("", user);
            }
            return BadRequest();
        }
    }
}
