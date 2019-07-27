using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataStoreWithToken.Data;
using DataStoreWithToken.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;

namespace DataStoreWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Bearer")]
    //[Authorize(AuthenticationSchemes = AuthSchemes)]
    public class DataStoreItems1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;
        private IConfiguration _config;
        private const string AuthSchemes = JwtBearerDefaults.AuthenticationScheme;

        public DataStoreItems1Controller(ApplicationDbContext context, IConfiguration config, IDataProtectionProvider provider)
        {
            _context = context;
            _config = config;
            _protector = provider.CreateProtector("ItemDetailsController");
        }

        // GET: api/DataStoreItems1
        [HttpGet]
        //[Authorize(Policy = "Bearer")]
        public async Task<ActionResult<IEnumerable<DataStoreItem>>> GetDataStoreItem()
        {
 

            return await _context.DataStoreItem.ToListAsync();
        }

        // GET: api/DataStoreItems1/5
        [HttpGet("{storeItemName}")]
        //[Authorize]
        [Authorize(AuthenticationSchemes = AuthSchemes)]
        public IActionResult GetDataStoreItem(string storeItemName)
        {

            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            var otp = GetOtp(accessToken);

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // Gets list of claims.
            IEnumerable<Claim> claim = identity.Claims;

            var otpHash = claim
                .Where(x => x.Type == "Otp")
                .FirstOrDefault().Value;

            var token = _context.Token
                .Where(o => o.OtpHash == otpHash)
                .FirstOrDefault();

            if (token!=null)
            {
                if (token.Revoked == true)
                    return Unauthorized();

            }

            // Gets name from claims. Generally it's an email address. c => c.Type == "Otp"
            var dataStoreId = claim
                .Where(x => x.Type == "DataStoreId")
                .FirstOrDefault().Value;


            //var dataStoreItem2 = await _context.DataStoreItem.FindAsync(1);

            var dataStoreItem = _context.DataStoreItem
                .Where(i => i.DataStoreId == int.Parse(dataStoreId) && i.StoreItemName==storeItemName)
                .FirstOrDefault();
            //.ToList();

            if (dataStoreItem == null)
            {
                return NotFound();
            }

            var itemDetails = _context.ItemDetail
                .Where(i=>i.DataStoreItemId== dataStoreItem.Id)
                .ToList();

            if (itemDetails == null)
            {
                return NotFound();
            }
 
            foreach(ItemDetail d in itemDetails)
            {
                d.ItemDetailValue = _protector.Unprotect(d.ItemDetailValue);
            }
            
            var content = JsonConvert.SerializeObject(itemDetails);

            return (Ok(content));

        }

        // PUT: api/DataStoreItems1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDataStoreItem(int id, DataStoreItem dataStoreItem)
        {
            if (id != dataStoreItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(dataStoreItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataStoreItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DataStoreItems1
        [HttpPost]
        public async Task<ActionResult<DataStoreItem>> PostDataStoreItem(DataStoreItem dataStoreItem)
        {
            _context.DataStoreItem.Add(dataStoreItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDataStoreItem", new { id = dataStoreItem.Id }, dataStoreItem);
        }

        // DELETE: api/DataStoreItems1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DataStoreItem>> DeleteDataStoreItem(int id)
        {
            var dataStoreItem = await _context.DataStoreItem.FindAsync(id);
            if (dataStoreItem == null)
            {
                return NotFound();
            }

            _context.DataStoreItem.Remove(dataStoreItem);
            await _context.SaveChangesAsync();

            return dataStoreItem;
        }

        private bool DataStoreItemExists(int id)
        {
            return _context.DataStoreItem.Any(e => e.Id == id);
        }


        protected string GetOtp(string token)
        {
            //string secret = "this is a string used for encrypt and decrypt token";
            //var key = Encoding.ASCII.GetBytes(secret);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey, // new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true
            };
            var claims = handler.ValidateToken(token, validations, out var tokenSecure);
            return claims.FindFirst(c => c.Type == "Otp").Value;
        }
    }
}
