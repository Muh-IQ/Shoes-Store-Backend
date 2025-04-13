using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ShoesStore_Project.Controllers
{
    [Route("api/Brand")]
    [ApiController]
    [Authorize]
    public class BrandController(IBrandService service) : ControllerBase
    {
        [HttpGet("GetBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetBrands()
        {
            try
            {
                List<BrandDTO> brands  = await service.GetBrands();
                if (brands.Count < 1)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while get.");

                }
                return Ok(brands);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }

        }



        [HttpPost("AddNewBrand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddNewBrand([FromBody] BrandDTO brand)
        {
            if (brand == null)
            {
                return BadRequest("data is requierd.");
            }
          
           try
           {
                int newID = await service.AddNewBrand(brand.Name);
                if (newID < 1)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while post.");

                }
                return Ok(newID);
           }
           catch (SqlException ex)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex.Message}");
           }
           catch (Exception ex)
           {
               return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
           }
        }

    }
}
