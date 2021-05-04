using System;

namespace InternationalizationService.Models.ResponseModel
{
    public class CountriesDto
    {
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
