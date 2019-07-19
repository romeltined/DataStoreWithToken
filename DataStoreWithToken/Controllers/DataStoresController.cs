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
    public class DataStoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DataStoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DataStores
        public async Task<IActionResult> Index()
        {
            return View(await _context.DataStore.ToListAsync());
        }

        // GET: DataStores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStore = await _context.DataStore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataStore == null)
            {
                return NotFound();
            }

            return View(dataStore);
        }

        // GET: DataStores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DataStores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StoreName")] DataStore dataStore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dataStore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dataStore);
        }

        // GET: DataStores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStore = await _context.DataStore.FindAsync(id);
            if (dataStore == null)
            {
                return NotFound();
            }
            return View(dataStore);
        }

        // POST: DataStores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StoreName")] DataStore dataStore)
        {
            if (id != dataStore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dataStore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataStoreExists(dataStore.Id))
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
            return View(dataStore);
        }

        // GET: DataStores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataStore = await _context.DataStore
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataStore == null)
            {
                return NotFound();
            }

            return View(dataStore);
        }

        // POST: DataStores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataStore = await _context.DataStore.FindAsync(id);
            _context.DataStore.Remove(dataStore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DataStoreExists(int id)
        {
            return _context.DataStore.Any(e => e.Id == id);
        }
    }
}
