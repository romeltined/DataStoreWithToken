using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataStoreWithToken.Data;
using DataStoreWithToken.Models;

namespace DataStoreWithToken.Controllers
{
    public class ItemDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ItemDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ItemDetail.Include(i => i.DataStoreItem);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ItemDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemDetail = await _context.ItemDetail
                .Include(i => i.DataStoreItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemDetail == null)
            {
                return NotFound();
            }

            return View(itemDetail);
        }

        // GET: ItemDetails/Create
        public IActionResult Create()
        {
            ViewData["DataStoreItemId"] = new SelectList(_context.DataStoreItem, "Id", "Id");
            return View();
        }

        // POST: ItemDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemDetailName,ItemDetailValue,Cancelled,Created,DataStoreItemId")] ItemDetail itemDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DataStoreItemId"] = new SelectList(_context.DataStoreItem, "Id", "Id", itemDetail.DataStoreItemId);
            return View(itemDetail);
        }

        // GET: ItemDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemDetail = await _context.ItemDetail.FindAsync(id);
            if (itemDetail == null)
            {
                return NotFound();
            }
            ViewData["DataStoreItemId"] = new SelectList(_context.DataStoreItem, "Id", "Id", itemDetail.DataStoreItemId);
            return View(itemDetail);
        }

        // POST: ItemDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemDetailName,ItemDetailValue,Cancelled,Created,DataStoreItemId")] ItemDetail itemDetail)
        {
            if (id != itemDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemDetailExists(itemDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DataStoreItemId"] = new SelectList(_context.DataStoreItem, "Id", "Id", itemDetail.DataStoreItemId);
            return View(itemDetail);
        }

        // GET: ItemDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemDetail = await _context.ItemDetail
                .Include(i => i.DataStoreItem)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemDetail == null)
            {
                return NotFound();
            }

            return View(itemDetail);
        }

        // POST: ItemDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemDetail = await _context.ItemDetail.FindAsync(id);
            _context.ItemDetail.Remove(itemDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemDetailExists(int id)
        {
            return _context.ItemDetail.Any(e => e.Id == id);
        }
    }
}
