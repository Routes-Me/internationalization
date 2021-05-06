using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using InternationalizationService.Abstraction;
using InternationalizationService.Models;
using InternationalizationService.Models.DBModels;
using InternationalizationService.Models.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InternationalizationService.Controllers
{
    [ApiController]
    [ApiVersion( "1.0" )]
    [Route("v{version:apiVersion}/")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _CountriesRepository;
        private readonly InternationalizationServiceContext _context;
        public CountriesController(ICountriesRepository CountriesRepository, InternationalizationServiceContext context)
        {
            _CountriesRepository = CountriesRepository;
            _context = context;
        }

        [HttpGet]
        [Route("countries/{countryId?}")]
        public IActionResult GetCountries(string countryId, [FromQuery] Pagination pageInfo)
        {
            GetResponse response = new GetResponse();
            try
            {
                response = _CountriesRepository.GetCountries(countryId, pageInfo);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorResponse{ error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse{ error = ex.Message });
            }
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [Route("countries")]
        public async Task<IActionResult> PostCountries(CountriesDto countriesDto)
        {
            try
            {
                Countries country = _CountriesRepository.PostCountries(countriesDto);
                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorResponse{ error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse{ error = ex.Message });
            }
            return StatusCode(StatusCodes.Status201Created, new SuccessResponse{ message = CommonMessage.CountryInserted});
        }

        [HttpPut]
        [Route("countries/{countryId}")]
        public async Task<IActionResult> UpdateCountries(string countryId, CountriesDto countriesDto)
        {
            try
            {
                Countries country = _CountriesRepository.UpdateCountries(countryId, countriesDto);
                _context.Countries.Update(country);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorResponse{ error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse{ error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse{ error = ex.Message });
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete]
        [Route("countries/{countryId}")]
        public async Task<IActionResult> DeleteCountries(string countryId)
        {
            try
            {
                Countries country = _CountriesRepository.DeleteCountries(countryId);
                _context.Countries.Remove(country);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse{ error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse{ error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse{ error = ex.Message });
            }
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
