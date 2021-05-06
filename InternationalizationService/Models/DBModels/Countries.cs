using System;

namespace InternationalizationService.Models.DBModels
{
    public partial class Countries
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Currencies Currency { get; set; }
        public virtual Nationalities Nationality { get; set; }
    }
}
