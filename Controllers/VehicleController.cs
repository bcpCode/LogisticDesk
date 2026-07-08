using LogisticDesk.Data;
using LogisticDesk.DTOs;
using LogisticDesk.Enums;
using LogisticDesk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LogisticDesk.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        // Veritabanımızı (mutfağı) temsil eden değişken
        private readonly LogiDbContext _context;

        // Kurucu metot (Constructor): Garson işe başladığı an mutfağın anahtarı ona verilir
        public VehicleController(LogiDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicle([FromBody] VehicleAddingDto vehicleDto)
        {
            if (vehicleDto == null)
            {
                return BadRequest("Vehicle data is required.");
            }
            Vehicle newVehicle = new Vehicle
            {
                PlateNumber = vehicleDto.PlateNumber,
                MaxCapacity = Math.Round(vehicleDto.MaxCapacity,2),
                Status = VehicleStatus.Available,
                CreatedAt = DateTime.UtcNow,
                IsActive = false
            };
            _context.Vehicles.Add(newVehicle);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
