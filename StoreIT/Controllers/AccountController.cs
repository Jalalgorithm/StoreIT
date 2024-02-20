using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
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
