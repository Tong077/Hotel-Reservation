namespace H_application.DTOs.CurrencyDto
{
    public class CurrencyDtoUpdate
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
