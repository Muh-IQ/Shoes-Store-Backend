using Interfaces.DTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ShoesStore_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController(IStorageService service) : ControllerBase
    {
        [HttpGet("GetSizeQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<StorageDTO>>> GetSizeQuantity(int ProductID)
        {
            try
            {
                List<StorageDTO> storages = await service.GetSizeQuantity(ProductID);
                if (storages == null || storages.Count < 1)
                {
                    return NotFound($"not found storages that has ID {ProductID}");
                }
                return Ok(storages);
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
