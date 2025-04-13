using Interfaces.DTOs.ProductDTOs;
using Interfaces.Services;
using Interfaces.Utiltiy;
using Interfaces.Utiltiy.IMisc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ShoesStore_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController(IFilterService service) : ControllerBase
    {
        [HttpGet("GetPagedFilteredProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductCardDTO>>>
            GetPagedFilteredProducts(PropertyType property = PropertyType.Rate, DirectionType type= DirectionType.DESC, int pageNumber = 1 , int pageSize = 5)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                Direction dir =  new Direction() { Select = type };
                Property prop = new Property() { Select = property };
                List<ProductCardDTO> data = (await service.GetPagedFilteredProducts(dir, prop, pageNumber, pageSize)).ToList();
                if (data == null || data.Count == 0)
                {
                    return NotFound("No data available.");
                }

                return Ok(data);
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
