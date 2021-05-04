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
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrenciesRepository _CurrenciesRepository;
        private readonly InternationalizationServiceContext _context;
        public CurrenciesController(ICurrenciesRepository CurrenciesRepository, InternationalizationServiceContext context)
        {
            _CurrenciesRepository = CurrenciesRepository;
            _context = context;
        }

        [HttpGet]
        [Route("currencies/{currencyId?}")]
        public IActionResult GetCurrencies(string currencyId, [FromQuery] Pagination pageInfo)
        {
            GetResponse response = new GetResponse();
            try
            {
                response = _CurrenciesRepository.GetCurrencies(currencyId, pageInfo);
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
        [Route("currencies")]
        public async Task<IActionResult> PostCurrencies(CurrenciesDto currenciesDto)
        {
            try
            {
                Currencies currency = _CurrenciesRepository.PostCurrencies(currenciesDto);
                await _context.Currencies.AddAsync(currency);
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
            return StatusCode(StatusCodes.Status201Created, new SuccessResponse{ message = CommonMessage.CurrencyInserted});
        }

        [HttpDelete]
        [Route("currencies/{currencyId}")]
        public async Task<IActionResult> DeleteCurrencies(string currencyId)
        {
            try
            {
                Currencies currency = _CurrenciesRepository.DeleteCurrencies(currencyId);
                _context.Currencies.Remove(currency);
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
