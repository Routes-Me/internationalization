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
    public class NationalitiesRepository : INationaitiesRepository
    {
        private readonly AppSettings _appSettings;
        private readonly InternationalizationServiceContext _context;
        public NationalitiesRepository(IOptions<AppSettings> appSettings, InternationalizationServiceContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }

        public dynamic GetNationalities(string nationalityId, Pagination pageInfo)
        {
            List<Nationalities> nationalities = new List<Nationalities>();
            int recordsCount = 1;
 
            if (!string.IsNullOrEmpty(nationalityId))
                nationalities = _context.Nationalities.Where(r => r.NationalityId == Obfuscation.Decode(nationalityId)).ToList();
            else
            {
                nationalities = _context.Nationalities.Skip((pageInfo.offset - 1) * pageInfo.limit).Take(pageInfo.limit).ToList();
                recordsCount = _context.Nationalities.Count();
            }

            var page = new Pagination
            {
                offset = pageInfo.offset,
                limit = pageInfo.limit,
                total = recordsCount
            };

            dynamic nationalitiesData = nationalities.Select(n => new NationalitiesDto {
                    NationalityId = Obfuscation.Encode(n.NationalityId),
                    CountryId = Obfuscation.Encode(n.CountryId),
                    Name = n.Name,
                    CreatedAt = n.CreatedAt,
                }).ToList();       

            return new GetResponse
            {
                data = JArray.Parse(JsonConvert.SerializeObject(nationalitiesData, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })),
                pagination = page
            };
        }

        public Nationalities PostNationalities(NationalitiesDto nationalitiesDto)
        {
            if (nationalitiesDto == null)
                throw new ArgumentNullException(CommonMessage.InvalidData);
            
            return new Nationalities
            {
                CountryId = Obfuscation.Decode(nationalitiesDto.CountryId),
                Name = nationalitiesDto.Name,
                CreatedAt = DateTime.Now
            };
        }

        public Nationalities DeleteNationalities(string nationalityId)
        {
            if (string.IsNullOrEmpty(nationalityId))
                throw new ArgumentNullException(CommonMessage.InvalidData);

            Nationalities nationality = _context.Nationalities.Where(r => r.NationalityId == Obfuscation.Decode(nationalityId)).FirstOrDefault();
            if (nationality == null)
                throw new KeyNotFoundException(CommonMessage.NationalityNotFound);

            return nationality;
        }
    }
}
