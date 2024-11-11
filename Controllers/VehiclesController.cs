using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetManagementSystem.Data;
using FleetManagementSystem.Models;

namespace FleetManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly FleetManagementSystemContext _context;

        public VehiclesController(FleetManagementSystemContext context)
        {
            _context = context;
        }

        // GET: api/vehicles/locations
        [HttpGet("locations")]
        public async Task<IActionResult> GetVehicleLocations()
        {
            var vehicles = await _context.Vehicle.ToListAsync(); // Fetch vehicles from your data source
            var locations = vehicles.Select(v => new 
            {
                v.VehicleId,
                v.Latitude,
                v.Longitude,
                v.Timestamp
            }).ToList();

            return Ok(locations); // Return the list of vehicle locations as JSON
        }

        // POST: api/vehicles/location
        [HttpPost("location")]
        public async Task<IActionResult> PostVehicleLocation([FromBody] Vehicle vehicle)
        {
            // Input validation
            if (vehicle == null)
            {
                return BadRequest("Vehicle data is required.");
            }

            if (string.IsNullOrWhiteSpace(vehicle.VehicleId))
            {
                return BadRequest("VehicleId cannot be null or empty.");
            }

            if (vehicle.Latitude < -90 || vehicle.Latitude > 90)
            {
                return BadRequest("Latitude must be between -90 and 90.");
            }

            if (vehicle.Longitude < -180 || vehicle.Longitude > 180)
            {
                return BadRequest("Longitude must be between -180 and 180.");
            }

            // Find the existing vehicle in the database
            var existingVehicle = await _context.Vehicle.FindAsync(vehicle.VehicleId);
            
            if (existingVehicle == null)
            {
                // Create a new vehicle record
                var newVehicle = new Vehicle
                {
                    VehicleId = vehicle.VehicleId,
                    Latitude = vehicle.Latitude,
                    Longitude = vehicle.Longitude,
                    Timestamp = vehicle.Timestamp
                };

                // Add the new vehicle to the context
                await _context.Vehicle.AddAsync(newVehicle);
            }
            else
            {
                // Update the existing vehicle's properties
                existingVehicle.Latitude = vehicle.Latitude;
                existingVehicle.Longitude = vehicle.Longitude;
                existingVehicle.Timestamp = vehicle.Timestamp;
            }
            
            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return a NoContent response indicating success
            return NoContent();
        }

        // // GET: Vehicles
        // public async Task<IActionResult> Index(string searchString)
        // {
        //      if (_context.Vehicle == null)
        // {
        //     return Problem("Entity set 'FleetManagementSystemContext.Movie'  is null.");
        // }

        // var vehicles = from m in _context.Vehicle
        //             select m;

        // if (!String.IsNullOrEmpty(searchString))
        // {
        //     vehicles = vehicles.Where(s => s.VehicleId!.ToUpper().Contains(searchString.ToUpper()));
        // }

        //     return View(await vehicles.ToListAsync());
        // }

        // // GET: Vehicles/Details/5
        // public async Task<IActionResult> Details(string id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var vehicle = await _context.Vehicle
        //         .FirstOrDefaultAsync(m => m.VehicleId == id);
        //     if (vehicle == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(vehicle);
        // }

        // // GET: Vehicles/Create
        // public IActionResult Create()
        // {
        //     return View();
        // }

        // // POST: Vehicles/Create
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Create([Bind("VehicleId,Latitude,Longitude")] Vehicle vehicle)
        // {
        //     // Set the Timestamp to DateTime.Now when creating a new vehicle
        //     vehicle.Timestamp = DateTime.Now;
            
        //     if (ModelState.IsValid)
        //     {
        //         _context.Add(vehicle);
        //         await _context.SaveChangesAsync();
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(vehicle);
        // }

        // // GET: Vehicles/Edit/5
        // public async Task<IActionResult> Edit(string id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var vehicle = await _context.Vehicle.FindAsync(id);
        //     if (vehicle == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(vehicle);
        // }

        // // POST: Vehicles/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(string id, [Bind("VehicleId,Latitude,Longitude,Timestamp")] Vehicle vehicle)
        // {
        //     if (id != vehicle.VehicleId)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(vehicle);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!VehicleExists(vehicle.VehicleId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(vehicle);
        // }

        // // GET: Vehicles/Delete/5
        // public async Task<IActionResult> Delete(string id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var vehicle = await _context.Vehicle
        //         .FirstOrDefaultAsync(m => m.VehicleId == id);
        //     if (vehicle == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(vehicle);
        // }

        // // POST: Vehicles/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(string id)
        // {
        //     var vehicle = await _context.Vehicle.FindAsync(id);
        //     if (vehicle != null)
        //     {
        //         _context.Vehicle.Remove(vehicle);
        //     }

        //     await _context.SaveChangesAsync();
        //     return RedirectToAction(nameof(Index));
        // }

        // private bool VehicleExists(string id)
        // {
        //     return _context.Vehicle.Any(e => e.VehicleId == id);
        // }

        // public IActionResult GetVehicleLocations()
        // {
        //     var vehicles = _context.Vehicle.ToList(); // Fetch vehicles from your data source
        //     return Json(vehicles);
        // }

        // // POST: Vehicles/UpdateLocation/5
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> UpdateLocation(string id, double latitude, double longitude)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var vehicle = await _context.Vehicle.FindAsync(id);
        //     if (vehicle == null)
        //     {
        //         return NotFound();
        //     }

        //     // Update latitude and longitude
        //     vehicle.Latitude = latitude;
        //     vehicle.Longitude = longitude;

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(vehicle);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!VehicleExists(vehicle.VehicleId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(vehicle);
        // }
        // // POST: Vehicles/UpdateAllLocations
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> UpdateAllLocations()
        // {
        //     // Fetch all vehicles from the database
        //     var vehicles = await _context.Vehicle.ToListAsync();

        //     // Update latitude and longitude for each vehicle
        //     foreach (var vehicle in vehicles)
        //     {
        //         vehicle.Latitude += 1;   // Increment latitude by 1
        //         vehicle.Longitude += 0.1; // Increment longitude by 0.1
        //         vehicle.Timestamp = DateTime.Now;
        //     }

        //     // Save changes to the database
        //     await _context.SaveChangesAsync();

        //     // Redirect back to the index or another appropriate view
        //     return RedirectToAction(nameof(Index));
        // }
    }
}
