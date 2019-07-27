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

namespace DataStoreWithToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemDetails1Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtector _protector;

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
        public async Task<ActionResult<ItemDetail>> PostItemDetail(ItemDetail itemDetail)
        {
            itemDetail.ItemDetailValue = _protector.Protect(itemDetail.ItemDetailValue);
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
