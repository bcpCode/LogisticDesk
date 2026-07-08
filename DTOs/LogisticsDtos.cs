namespace LogisticDesk.DTOs
{
    public class LogisticsDtos
    {
        public string TrackingNumber { get; set; }
        public decimal Weight { get; set; }
        public string ReceiverAddress { get; set; }
        public int? VehicleId { get; set; }

    }
}
namespace LogisticDesk.DTOs
{
    public class VehicleAddingDto
    {
        public string PlateNumber { get; set; }
        public decimal MaxCapacity { get; set; }
    }
}
