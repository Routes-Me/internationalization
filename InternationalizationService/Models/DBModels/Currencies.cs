using System;

namespace InternationalizationService.Models.DBModels
{
    public partial class Currencies
    {
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Countries Country { get; set; }
    }
}
