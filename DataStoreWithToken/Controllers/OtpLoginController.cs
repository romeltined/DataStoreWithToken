using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataStoreWithToken.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataStoreWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtpLoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public OtpLoginController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }
        // GET: api/OtpLogin
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/OtpLogin/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OtpLogin
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            IActionResult response = Unauthorized();
            //var dataStore = AuthenticateOtp(value);

            var token = _context.Token
                .Where(o => o.OtpHash == value)
                .FirstOrDefault();

            if (token != null)
            {

                var tokenString = GenerateJSONWebToken(token.Id.ToString(), token.OtpHash, token.DataStoreId.ToString());

                token.TokenHash = tokenString;
                token.Activated = true;
                _context.Update(token);
                _context.SaveChanges();

                response = Ok(new { token = tokenString });
 
            }

            return response;
        }

        private string GenerateJSONWebToken(string id, string otp, string dataStoreId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Id", id),
                new Claim("Otp",otp),
                new Claim("DataStoreId", dataStoreId)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(5256000),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string AuthenticateOtp(string value)
        {
            return "memel" + value;
            throw new NotImplementedException();
        }

        // PUT: api/OtpLogin/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
