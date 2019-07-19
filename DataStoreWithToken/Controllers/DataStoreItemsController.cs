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
    public class DataStoreItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataStoreItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DataStoreItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DataStoreItem.Include(d => d.DataStore);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DataStoreItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStoreItem = await _context.DataStoreItem
                .Include(d => d.DataStore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataStoreItem == null)
            {
                return NotFound();
            }

            return View(dataStoreItem);
        }

        // GET: DataStoreItems/Create
        public IActionResult Create()
        {
            ViewData["DataStoreId"] = new SelectList(_context.DataStore, "Id", "Id");
            return View();
        }

        // POST: DataStoreItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreItemName,Completed,Created,DataStoreId")] DataStoreItem dataStoreItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataStoreItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DataStoreId"] = new SelectList(_context.DataStore, "Id", "Id", dataStoreItem.DataStoreId);
            return View(dataStoreItem);
        }

        // GET: DataStoreItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStoreItem = await _context.DataStoreItem.FindAsync(id);
            if (dataStoreItem == null)
            {
                return NotFound();
            }
            ViewData["DataStoreId"] = new SelectList(_context.DataStore, "Id", "Id", dataStoreItem.DataStoreId);
            return View(dataStoreItem);
        }

        // POST: DataStoreItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreItemName,Completed,Created,DataStoreId")] DataStoreItem dataStoreItem)
        {
            if (id != dataStoreItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataStoreItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataStoreItemExists(dataStoreItem.Id))
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
            ViewData["DataStoreId"] = new SelectList(_context.DataStore, "Id", "Id", dataStoreItem.DataStoreId);
            return View(dataStoreItem);
        }

        // GET: DataStoreItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStoreItem = await _context.DataStoreItem
                .Include(d => d.DataStore)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataStoreItem == null)
            {
                return NotFound();
            }

            return View(dataStoreItem);
        }

        // POST: DataStoreItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataStoreItem = await _context.DataStoreItem.FindAsync(id);
            _context.DataStoreItem.Remove(dataStoreItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataStoreItemExists(int id)
        {
            return _context.DataStoreItem.Any(e => e.Id == id);
        }
    }
}
