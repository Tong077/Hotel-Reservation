using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H_Domain.Models
{
    public class PaymentMethod
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentMethodId { get; set; }


        public string? Name { get; set; }


        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;


    }
}
