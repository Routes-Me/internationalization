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
    public class CountriesRepository : ICountriesRepository
    {
        private readonly AppSettings _appSettings;
        private readonly InternationalizationServiceContext _context;
        public CountriesRepository(IOptions<AppSettings> appSettings, InternationalizationServiceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public dynamic GetCountries(string countryId, Pagination pageInfo)
        {
            List<Countries> countries = new List<Countries>();
            int recordsCount = 1;
 
            if (!string.IsNullOrEmpty(countryId))
                countries = _context.Countries.Where(r => r.CountryId == Obfuscation.Decode(countryId)).ToList();
            else
            {
                countries = _context.Countries.Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();
                recordsCount = _context.Countries.Count();
            }

            var page = new Pagination
            {
                offset = pageInfo.offset,
                limit = pageInfo.limit,
                total = recordsCount
            };

            dynamic countriesData = countries.Select(c => new CountriesDto {
                    CountryId = Obfuscation.Encode(c.CountryId),
                    Name = c.Name,
                    Code = c.Code,
                    CreatedAt = c.CreatedAt,
                }).ToList();       

            return new GetResponse
            {
                data = JArray.Parse(JsonConvert.SerializeObject(countriesData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })),
                pagination = page
            };
        }

        public Countries PostCountries(CountriesDto countriesDto)
        {
            if (countriesDto == null)
                throw new ArgumentNullException(CommonMessage.InvalidData);
            
            return new Countries
            {
                Name = countriesDto.Name,
                Code = countriesDto.Code,
                CreatedAt = DateTime.Now
            };
        }

        public Countries UpdateCountries(string countryId, CountriesDto countriesDto)
        {
            if (countriesDto == null || string.IsNullOrEmpty(countryId))
                throw new ArgumentNullException(CommonMessage.InvalidData);

            Countries country = _context.Countries.Where(c => c.CountryId == Obfuscation.Decode(countryId)).FirstOrDefault();
            if (country == null)
                throw new ArgumentException(CommonMessage.CountryNotFound);

            country.Name = countriesDto.Name ?? country.Name;
            country.Code = countriesDto.Code ?? country.Code;

            return country;
        }

        public Countries DeleteCountries(string countryId)
        {
            if (string.IsNullOrEmpty(countryId))
                throw new ArgumentNullException(CommonMessage.InvalidData);

            Countries country = _context.Countries.Where(r => r.CountryId == Obfuscation.Decode(countryId)).FirstOrDefault();
            if (country == null)
                throw new KeyNotFoundException(CommonMessage.CountryNotFound);

            return country;
        }
    }
}
