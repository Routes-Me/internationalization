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
    public class NationalitiesController : ControllerBase
    {
        private readonly INationaitiesRepository _NationalitiesRepository;
        private readonly InternationalizationServiceContext _context;
        public NationalitiesController(INationaitiesRepository NationalitiesRepository, InternationalizationServiceContext context)
        {
            _NationalitiesRepository = NationalitiesRepository;
            _context = context;
        }

        [HttpGet]
        [Route("nationalities/{nationalityId?}")]
        public IActionResult GetNationalities(string nationalityId, [FromQuery] Pagination pageInfo)
        {
            GetResponse response = new GetResponse();
            try
            {
                response = _NationalitiesRepository.GetNationalities(nationalityId, pageInfo);
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
        [Route("nationalities")]
        public async Task<IActionResult> PostNationalities(NationalitiesDto nationalitiesDto)
        {
            try
            {
                Nationalities nationality = _NationalitiesRepository.PostNationalities(nationalitiesDto);
                await _context.Nationalities.AddAsync(nationality);
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
            return StatusCode(StatusCodes.Status201Created, new SuccessResponse{ message = CommonMessage.NationalityInserted});
        }

        [HttpDelete]
        [Route("nationalities/{nationalityId}")]
        public async Task<IActionResult> DeleteNationalities(string nationalityId)
        {
            try
            {
                Nationalities nationality = _NationalitiesRepository.DeleteNationalities(nationalityId);
                _context.Nationalities.Remove(nationality);
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
