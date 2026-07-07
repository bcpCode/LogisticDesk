using LogisticDesk.Enums;

namespace LogisticDesk.Models
{
    public class Shipment : BaseEntity
    {
        public string TrackingNumber { get; set; }
        public decimal Weight { get; set; } = 0;
        public string ReceiverAddress { get; set; }
        public ShipmentStatus Status { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

    }
}
