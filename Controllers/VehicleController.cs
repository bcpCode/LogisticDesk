using LogisticDesk.Data;
using LogisticDesk.DTOs;
using LogisticDesk.Enums;
using LogisticDesk.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace LogisticDesk.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly LogiDbContext _context;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetVehicleByStatus(VehicleStatus status)
        {
            var vehicles = await _context.Vehicles.Where(v => v.Status == status).ToListAsync();
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
            return Ok(newVehicle);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateVehicleById(int id, [FromBody] VehicleAddingDto vehicleDto  )
        {
            if (id <= 0)
            {
                return BadRequest("Invalid vehicle ID.");
            }
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            vehicle.PlateNumber = vehicleDto.PlateNumber;
            vehicle.MaxCapacity = Math.Round(vehicleDto.MaxCapacity, 2);
            vehicle.Status = vehicleDto.VehicleStatus;
            vehicle.IsActive = vehicleDto.IsActive;
            await _context.SaveChangesAsync();
            return Ok(vehicle);
        }
    }
}
