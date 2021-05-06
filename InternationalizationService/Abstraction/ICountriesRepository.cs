using InternationalizationService.Models.DBModels;
using InternationalizationService.Models.ResponseModel;

using InternationalizationService.Models;

namespace InternationalizationService.Abstraction
{
    public interface ICountriesRepository
    {
        dynamic GetCountries(string countryId, Pagination pageInfo);
        Countries PostCountries(CountriesDto countriesDto);
        Countries UpdateCountries(string countryId, CountriesDto countriesDto);
        Countries DeleteCountries(string countryId);
    }
}
