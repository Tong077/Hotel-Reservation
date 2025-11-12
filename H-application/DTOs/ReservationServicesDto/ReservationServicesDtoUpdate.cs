namespace H_application.DTOs.ReservationServicesDto
{
    public class ReservationServicesDtoUpdate
    {
        public int ReservationServiceId { get; set; }


        public int? ReservationId { get; set; }


        public int? ServiceId { get; set; }

        public decimal? Quantity { get; set; }


        public decimal? TotalPrice { get; set; }
    }
}
