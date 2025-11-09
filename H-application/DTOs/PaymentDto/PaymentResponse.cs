using H_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_application.DTOs.Payment
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }


        public int? ReservationId { get; set; }

       
        public int? PaymentMethodId { get; set; }


        public decimal? Amount { get; set; }


        public string? Currency { get; set; }


        public string? TransactionId { get; set; }


        public string? PaymentStatus { get; set; }

        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;


        public decimal? RefundAmount { get; set; }

        public DateTime? RefundDate { get; set; }

        public string? PaymentMethodName { get; set; }
        public decimal? TotalPrice { get; set; }
        public string? GuestRoomInfo { get; set; }

        public decimal TotalAmount { get; set; }
    }
}
