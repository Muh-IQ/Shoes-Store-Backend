using Interfaces.DTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ShoesStore_Project.Controllers
{
    [Authorize]
    [Route("api/Categories")]
    [ApiController]
    public class CategoriesController(ICategoryService service) : ControllerBase
    {
        [HttpGet("GetCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
        {
            try
            {
                List<CategoryDTO> brands = await service.GetCategories();
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
    }
}
