using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCGAuthoring.Data;
using PCGAuthoring.Models;
using PCGAuthoring.Models.ViewModels;
using System.Text.Json;

namespace PCGAuthoring.Controllers
{
    public class RoomsController : Controller
    {
        private readonly PCGAuthoringContext _context;

        public RoomsController(PCGAuthoringContext context)
        {
            _context = context;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.ToListAsync());
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomItems)
                .ThenInclude(i => i.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }
            PopulateAssignedItemData(room);
            return View(room);
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var room = new Room();
            room.RoomItems = new List<ItemAssignment>();
            PopulateAssignedItemData(room);
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Room room, string[] selectedItems, string[] selectedMins, string[] selectedMaxs)
        {
            room.RoomItems = new List<ItemAssignment>();

            foreach (var item in selectedItems)
            {
                var key = int.Parse(item) - 1;
                var minVal = int.Parse(selectedMins[key]);
                var maxVal = int.Parse(selectedMaxs[key]);
                var itemToAdd = new ItemAssignment
                {
                    RoomId = room.Id,
                    ItemId = int.Parse(item),
                    Min = minVal,
                    Max = maxVal
                };
                room.RoomItems.Add(itemToAdd);
            }

            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
               .Include(r => r.RoomItems)
               .ThenInclude(i => i.Item)
               .AsNoTracking()
               .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            PopulateAssignedItemData(room);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Room room, string[] selectedItems, string[] selectedMins, string[] selectedMaxs)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            var roomToUpdate = await _context.Rooms
                .Include(r => r.RoomItems)
                .ThenInclude(i => i.Item)
                .FirstOrDefaultAsync(r => r.Id == id);


            if (roomToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Room>(
                roomToUpdate,
                "",
                r => r.Name, r => r.RoomItems))
            {
                UpdateRoomItems(selectedItems, selectedMins, selectedMaxs, roomToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            UpdateRoomItems(selectedItems, selectedMins, selectedMaxs, roomToUpdate);
            PopulateAssignedItemData(roomToUpdate);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // GET: Rooms/Generate/5
        public async Task<IActionResult> Generate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.RoomItems)
                .ThenInclude(i => i.Item)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound();
            }
            //PopulateAssignedItemData(room);

            var jsonStr = JsonSerializer.Serialize(room);


            // Creat db row
            var newRequest = new Request()
            {
                Status = ReqState.PENDING,
                AuthoringData = jsonStr
            };

            _context.Requests.Add(newRequest);
            await _context.SaveChangesAsync();

            // Pass json string to the view
            ViewBag.JsonStr = jsonStr;

            return View();
        }




        //-------------------
        // Helper methods
        //-------------------

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }


        private void PopulateAssignedItemData(Room room)
        {
            var allItems = _context.Items;
            var viewModel = new List<AssignedItemData>();
            foreach (var item in allItems)
            {
                var viewitem = new AssignedItemData();
                viewitem.AssignedItemId = item.Id;
                viewitem.AssignedItemName = item.Name;

                var t = room.RoomItems.Where(i => i.ItemId == item.Id).SingleOrDefault();
                viewitem.AssignedMin = (t != null) ? t.Min : 0;
                viewitem.AssignedMax = (t != null) ? t.Max : 0;
                viewModel.Add(viewitem);
            }
            ViewData["Items"] = viewModel;
        }


        private void UpdateRoomItems(string[] selectedItems, string[] selectedMins, string[] selectedMaxs, Room roomToUpdate)
        {
            foreach (var item in selectedItems)
            {
                var key = int.Parse(item) - 1;
                var minVal = int.Parse(selectedMins[key]);
                var maxVal = int.Parse(selectedMaxs[key]);
                var obj = roomToUpdate.RoomItems.Where(i => i.ItemId == int.Parse(item)).SingleOrDefault();
                if (obj != null)
                {
                    if (minVal != 0 || maxVal != 0)
                    {
                        obj.Min = minVal;
                        obj.Max = maxVal;
                    }
                    else
                    {
                        _context.Remove(obj);
                    }
                }
                else
                {
                    if (minVal != 0 || maxVal != 0)
                    {
                        roomToUpdate.RoomItems.Add(new ItemAssignment
                        {
                            RoomId = roomToUpdate.Id,
                            ItemId = key,
                            Min = minVal,
                            Max = maxVal
                        });
                    }
                }
            }
        }
    }
}
