using InternationalizationService.Models.DBModels;
using InternationalizationService.Models.ResponseModel;

using InternationalizationService.Models;

namespace InternationalizationService.Abstraction
{
    public interface INationaitiesRepository
    {
        dynamic GetNationalities(string nationalityId, Pagination pageInfo);
        Nationalities PostNationalities(NationalitiesDto nationalitiesDto);
        Nationalities DeleteNationalities(string nationalityId);
    }
}
