namespace H_application.DTOs.ReservationDto
{
    public class MonthlyRevenueDto
    {
        public string? Period { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal GrowthPercentage { get; set; }
        public int Week { get; set; }
        public int TotalReservations { get; set; }
    }

}
