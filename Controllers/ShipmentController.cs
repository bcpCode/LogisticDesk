using LogisticDesk.Data;
using LogisticDesk.DTOs;
using LogisticDesk.Enums;
using LogisticDesk.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticDesk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly LogiDbContext _context;
        public ShipmentController(LogiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetShipments()
        {
            var shipments = await _context.Shipments.ToListAsync();
            return Ok(shipments);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipmentById(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(shipment);
        }


        [HttpPost("warehouse")] // URL: api/Shipment/warehouse
        public async Task<IActionResult> AddShipmentToWarehouse([FromBody] ShipmentAddingDto shipmentDto)
        {
            if (shipmentDto == null)
            {
                return BadRequest("Shipment data cannot be null.");
            }

            Shipment newShipment = new Shipment
            {
                TrackingNumber = shipmentDto.TrackingNumber ?? Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                Weight = Math.Round(shipmentDto.Weight, 2),
                ReceiverAddress = shipmentDto.ReceiverAddress,
                VehicleId = null, // Not assigned to any vehicle yet
                CreatedAt = DateTime.UtcNow
            };

            _context.Shipments.Add(newShipment);
            await _context.SaveChangesAsync();

            return Ok(newShipment);
        }


        [HttpPost("assign-direct")] // URL: api/Shipment/assign-direct
        public async Task<IActionResult> AssignShipmentDirectly([FromBody] ShipmentAddingDto shipmentDto)
        {
            // If VehicleId is not provided from the outside, throw an error
            if (shipmentDto.VehicleId == null || shipmentDto.VehicleId <= 0)
            {
                return BadRequest("A valid Vehicle ID (VehicleId) is required for direct assignment.");
            }

            var vehicle = await _context.Vehicles.FindAsync(shipmentDto.VehicleId);
            if (vehicle == null)
            {
                return NotFound("The vehicle intended for assignment was not found.");
            }

            if (vehicle.Status != VehicleStatus.Available)
            {
                return BadRequest("This vehicle is currently not available (On Route or Under Maintenance).");
            }

            if (shipmentDto.Weight > vehicle.MaxCapacity)
            {
                return BadRequest($"Capacity exceeded! The vehicle can carry a maximum of {vehicle.MaxCapacity}, but the shipment weighs {shipmentDto.Weight}.");
            }

            Shipment newShipment = new Shipment
            {
                TrackingNumber = shipmentDto.TrackingNumber ?? Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                ReceiverAddress = shipmentDto.ReceiverAddress,
                Weight = Math.Round(shipmentDto.Weight, 2),
                VehicleId = vehicle.Id,
                CreatedAt = DateTime.UtcNow
            };

            vehicle.Status = VehicleStatus.OnRoute;

            _context.Shipments.Add(newShipment);
            await _context.SaveChangesAsync();

            return Ok(newShipment);
        }


        [HttpPut("{shipmentId}/load-to-vehicle/{vehicleId}")]
        public async Task<IActionResult> LoadFromWarehouseToVehicle(int shipmentId, int vehicleId)
        {
            var shipment = await _context.Shipments.FindAsync(shipmentId);
            if (shipment == null) return NotFound("Shipment not found.");
            if (shipment.VehicleId != null) return BadRequest("This shipment is already assigned to a vehicle.");

            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) return NotFound("Vehicle not found.");
            if (vehicle.Status != VehicleStatus.Available) return BadRequest("Vehicle is not available.");
            if (shipment.Weight > vehicle.MaxCapacity) return BadRequest("Shipment is too heavy for this vehicle.");

            shipment.VehicleId = vehicle.Id; // Assign shipment to vehicle
            vehicle.Status = VehicleStatus.OnRoute;

            await _context.SaveChangesAsync();
            return Ok(shipment);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipmentById(int id, [FromBody] ShipmentAddingDto shipmentDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid shipment ID.");
            }
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }
            shipment.TrackingNumber = shipmentDto.TrackingNumber;
            shipment.Weight = Math.Round(shipmentDto.Weight, 2);
            shipment.ReceiverAddress = shipmentDto.ReceiverAddress;
            shipment.VehicleId = shipmentDto.VehicleId;
            await _context.SaveChangesAsync();
            return Ok(shipment);
        }
    }
}
