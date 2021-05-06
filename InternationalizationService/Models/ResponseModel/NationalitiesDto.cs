using System;

namespace InternationalizationService.Models.ResponseModel
{
    public class NationalitiesDto
    {
        public string NationalityId { get; set; }
        public string CountryId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
