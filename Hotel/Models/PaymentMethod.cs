using System.ComponentModel.DataAnnotations;

namespace H_Domain.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }


        public string? Name { get; set; }


        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        
    }
}
