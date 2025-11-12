namespace H_application.DTOs.ServicesDto
{
    public class ServicesDtoCreate
    {

        public string? ServiceName { get; set; }


        public string? Category { get; set; }


        public decimal? Price { get; set; }

        public string? Currency { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
