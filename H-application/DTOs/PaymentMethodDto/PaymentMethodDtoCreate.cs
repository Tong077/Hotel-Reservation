namespace H_application.DTOs.PaymentMethodDto
{
    public class PaymentMethodDtoCreate
    {



        public string? Name { get; set; }


        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
