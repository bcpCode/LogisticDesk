using LogisticDesk.Enums;

namespace LogisticDesk.Models
{
    public class Vehicle : BaseEntity
    {
        public string PlateNumber { get; set; }
        public decimal MaxCapacity { get; set; }
        public VehicleStatus Status { get; set; }
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    }
}
