using System;

namespace InternationalizationService.Models.ResponseModel
{
    public class CurrenciesDto
    {
        public string CurrencyId { get; set; }
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
