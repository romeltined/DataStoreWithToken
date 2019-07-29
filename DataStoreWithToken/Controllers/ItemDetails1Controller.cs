using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataStoreWithToken.Data;
using DataStoreWithToken.Models;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace DataStoreWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetails1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;
        private const string AuthSchemes = JwtBearerDefaults.AuthenticationScheme;

        public ItemDetails1Controller(ApplicationDbContext context, IDataProtectionProvider provider)
        {
            _context = context;
            _protector = provider.CreateProtector("ItemDetailsController");
        }

        // GET: api/ItemDetails1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDetail>>> GetItemDetail()
        {
            return await _context.ItemDetail.ToListAsync();
        }

        // GET: api/ItemDetails1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetItemDetail(int id)
        {
            var itemDetail = await _context.ItemDetail.FindAsync(id);

            if (itemDetail == null)
            {
                return NotFound();
            }

            return itemDetail;

        }

        // PUT: api/ItemDetails1/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemDetail(int id, ItemDetail itemDetail)
        {
            if (id != itemDetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(itemDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemDetailExists(id))
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

        // POST: api/ItemDetails1
        [HttpPost]
        [Authorize(AuthenticationSchemes = AuthSchemes)]
        public async Task<ActionResult<ItemDetail>> PostItemDetail([FromBody] dynamic item)
        {
            ItemRecord itemRecord = JsonConvert.DeserializeObject<ItemRecord>(item.ToString());

            var accessToken = Request.Headers["Authorization"].ToString().Replace("Bearer", "").Trim();
            //var otp = GetOtp(accessToken);

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            // Gets list of claims.
            IEnumerable<Claim> claim = identity.Claims;

            var dataStoreId = claim
                .Where(x => x.Type == "DataStoreId")
                .FirstOrDefault().Value;

            var dataStoreItem = _context.DataStoreItem
                .Where(d => d.DataStoreId == int.Parse(dataStoreId) && 
                            d.StoreItemName == itemRecord.StoreItemName &&
                            d.Completed==false)
                .FirstOrDefault();

            if(dataStoreItem==null)
            {
                return NotFound();
            }

            ItemDetail itemDetail = new ItemDetail();

            itemDetail.DataStoreItemId = dataStoreItem.Id;
            itemDetail.ItemDetailName = itemRecord.ItemDetailName;
            itemDetail.ItemDetailValue = _protector.Protect(itemRecord.ItemDetailValue);

            _context.ItemDetail.Add(itemDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemDetail", new { id = itemDetail.Id }, itemDetail);
        }

        // DELETE: api/ItemDetails1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemDetail>> DeleteItemDetail(int id)
        {
            var itemDetail = await _context.ItemDetail.FindAsync(id);
            if (itemDetail == null)
            {
                return NotFound();
            }

            _context.ItemDetail.Remove(itemDetail);
            await _context.SaveChangesAsync();

            return itemDetail;
        }

        private bool ItemDetailExists(int id)
        {
            return _context.ItemDetail.Any(e => e.Id == id);
        }
    }
}
