using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_Domain.Models
{
    public class Currency
    {
        public int? CurrencyId { get; set; }

        public string? FromCurrency { get; set; }
        public string? ToCurrency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsBaseRate { get; set; }
       
    }
}
