using InternationalizationService.Models.DBModels;
using InternationalizationService.Models.ResponseModel;

using InternationalizationService.Models;

namespace InternationalizationService.Abstraction
{
    public interface ICurrenciesRepository
    {
        dynamic GetCurrencies(string currencyId, Pagination pageInfo);
        Currencies PostCurrencies(CurrenciesDto currenciesDto);
        Currencies DeleteCurrencies(string currencyId);
    }
}
