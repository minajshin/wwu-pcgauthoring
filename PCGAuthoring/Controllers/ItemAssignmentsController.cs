using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PCGAuthoring.Data;
using PCGAuthoring.Models;

namespace PCGAuthoring.Controllers
{
    public class ItemAssignmentsController : Controller
    {
        private readonly PCGAuthoringContext _context;

        public ItemAssignmentsController(PCGAuthoringContext context)
        {
            _context = context;
        }

        // GET: ItemAssignments
        public async Task<IActionResult> Index()
        {
            var pCGAuthoringContext = _context.ItemAssignments.Include(i => i.Item).Include(i => i.Room);
            return View(await pCGAuthoringContext.ToListAsync());
        }

        // GET: ItemAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemAssignment = await _context.ItemAssignments
                .Include(i => i.Item)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemAssignment == null)
            {
                return NotFound();
            }

            return View(itemAssignment);
        }

        // GET: ItemAssignments/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Items, "Id", "Id");
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: ItemAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RoomId,Id,Min,Max")] ItemAssignment itemAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Items, "Id", "Id", itemAssignment.ItemId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", itemAssignment.RoomId);
            return View(itemAssignment);
        }

        // GET: ItemAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemAssignment = await _context.ItemAssignments.FindAsync(id);
            if (itemAssignment == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Items, "Id", "Id", itemAssignment.ItemId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", itemAssignment.RoomId);
            return View(itemAssignment);
        }

        // POST: ItemAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,Id,Min,Max")] ItemAssignment itemAssignment)
        {
            if (id != itemAssignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemAssignmentExists(itemAssignment.Id))
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
            ViewData["Id"] = new SelectList(_context.Items, "Id", "Id", itemAssignment.ItemId);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", itemAssignment.RoomId);
            return View(itemAssignment);
        }

        // GET: ItemAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemAssignment = await _context.ItemAssignments
                .Include(i => i.Item)
                .Include(i => i.Room)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemAssignment == null)
            {
                return NotFound();
            }

            return View(itemAssignment);
        }

        // POST: ItemAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemAssignment = await _context.ItemAssignments.FindAsync(id);
            _context.ItemAssignments.Remove(itemAssignment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemAssignmentExists(int id)
        {
            return _context.ItemAssignments.Any(e => e.Id == id);
        }
    }
}
