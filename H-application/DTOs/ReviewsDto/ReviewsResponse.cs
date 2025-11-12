namespace H_application.DTOs.ReviewsDto
{
    public class ReviewsResponse
    {
        public int ReviewId { get; set; }


        public int? GuestId { get; set; }


        public int? ReservationId { get; set; }


        public int? Rating { get; set; }


        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
