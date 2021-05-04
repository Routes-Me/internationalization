using System;

namespace InternationalizationService.Models.DBModels
{
    public partial class Nationalities
    {
        public int NationalityId { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Countries Country { get; set; }
    }
}
