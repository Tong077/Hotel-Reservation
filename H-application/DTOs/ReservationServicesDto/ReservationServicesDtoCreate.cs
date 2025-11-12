namespace H_application.DTOs.ReservationServicesDto
{
    public class ReservationServicesDtoCreate
    {

        public int? ReservationId { get; set; }


        public int? ServiceId { get; set; }

        public decimal? Quantity { get; set; }


        public decimal? TotalPrice { get; set; }
    }
}
