using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.Payment
{
    public class PaymentDtoCreate
    {
       public int? PaymentId { get; set; }  
        public List <int>? ReservationId { get; set; }
        
        public int? PaymentMethodId { get; set; }

        public decimal? Amount { get; set; } = 00;


        public string? Currency { get; set; }


        //public string? TransactionId { get; set; }


        public string? PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;


        public decimal? RefundAmount { get; set; }
        public List<decimal>? ReservationAmount { get; set; }
        public DateTime? RefundDate { get; set; }

        //public List<int> ReservationIds { get; set; } = new();



    }
}
