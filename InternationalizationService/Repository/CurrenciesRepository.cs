using InternationalizationService.Abstraction;
using InternationalizationService.Models;
using InternationalizationService.Models.Common;
using InternationalizationService.Models.DBModels;
using InternationalizationService.Models.ResponseModel;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RoutesSecurity;
using System;
using System.Linq;
using System.Collections.Generic;

namespace InternationalizationService.Repository
{
    public class CurrenciesRepository : ICurrenciesRepository
    {
        private readonly AppSettings _appSettings;
        private readonly InternationalizationServiceContext _context;
        public CurrenciesRepository(IOptions<AppSettings> appSettings, InternationalizationServiceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public dynamic GetCurrencies(string currencyId, Pagination pageInfo)
        {
            List<Currencies> currencies = new List<Currencies>();
            int recordsCount = 1;
 
            if (!string.IsNullOrEmpty(currencyId))
                currencies = _context.Currencies.Where(r => r.CurrencyId == Obfuscation.Decode(currencyId)).ToList();
            else
            {
                currencies = _context.Currencies.Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();
                recordsCount = _context.Currencies.Count();
            }

            var page = new Pagination
            {
                offset = pageInfo.offset,
                limit = pageInfo.limit,
                total = recordsCount
            };

            dynamic currenciesData = currencies.Select(c => new CurrenciesDto {
                    CurrencyId = Obfuscation.Encode(c.CurrencyId),
                    CountryId = Obfuscation.Encode(c.CountryId),
                    Name = c.Name,
                    Code = c.Code,
                    Symbol = c.Symbol,
                    CreatedAt = c.CreatedAt,
                }).ToList();       

            return new GetResponse
            {
                data = JArray.Parse(JsonConvert.SerializeObject(currenciesData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })),
                pagination = page
            };
        }

        public Currencies PostCurrencies(CurrenciesDto currenciesDto)
        {
            if (currenciesDto == null)
                throw new ArgumentNullException(CommonMessage.InvalidData);
            
            return new Currencies
            {
                CountryId = Obfuscation.Decode(currenciesDto.CountryId),
                Name = currenciesDto.Name,
                Code = currenciesDto.Code,
                Symbol = currenciesDto.Symbol,
                CreatedAt = DateTime.Now
            };
        }

        public Currencies DeleteCurrencies(string currencyId)
        {
            if (string.IsNullOrEmpty(currencyId))
                throw new ArgumentNullException(CommonMessage.InvalidData);

            Currencies currency = _context.Currencies.Where(r => r.CurrencyId == Obfuscation.Decode(currencyId)).FirstOrDefault();
            if (currency == null)
                throw new KeyNotFoundException(CommonMessage.CurrencyNotFound);

            return currency;
        }
    }
}
