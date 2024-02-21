using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StoreIT.ApiModel;
using StoreIT.Data;
using StoreIT.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StoreIT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AccountController(IConfiguration configuration , ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register (UserRegisterDto userDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();

            }

            var emailCount = await _context.Users.CountAsync(u => u.Email == userDto.Email);

            if (emailCount > 0)
            {
                return BadRequest("Email already exist , Try logging in");
            }

            var passwordHasher = new PasswordHasher<User>();
            var encryptedPassword = passwordHasher.HashPassword(new User(), userDto.Password);


            var user = new User()
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = encryptedPassword,
                Role = "Client",
                CreatedAt = DateTime.Now
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var jwt = CreateJwt(user);

            var CurrentUser = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,

            };

            var response = new
            {
                Token = jwt,
                Data = CurrentUser
            };

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login (string email , string password)
        {
            var user = await _context.Users.FindAsync(email);

            if (user == null)
            {
                return NotFound("Email or password is incorrect");
            }

            var passwordHasher = new PasswordHasher<User>();
            var verifyPassowrd = passwordHasher.VerifyHashedPassword(new User(), user.Password, password);

            if(verifyPassowrd == PasswordVerificationResult.Failed)
            {
                return BadRequest("Email or password incorrect");

            }

            var jwt = CreateJwt(user);

            var userDetails = new UserProfileDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
            };

            var responses = new
            {
                token = jwt,
                Data = userDetails
            };

            return Ok(responses);

        }


        private string CreateJwt (User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim("id" , "" + user.Id),
                new Claim("role" , user.Role)
            };

            string strKey = _configuration["JwtSetting:Key"]!;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(strKey));

            var cred = new SigningCredentials(key , SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(

                issuer: _configuration["JwtSetting:Issuer"],
                audience: _configuration["JwtSetting:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }
    }
}
