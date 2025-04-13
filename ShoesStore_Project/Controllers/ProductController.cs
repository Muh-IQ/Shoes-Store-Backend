using Interfaces.DTOs;
using Interfaces.DTOs.InventoryDTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using ShoesStore_Project.Extention;
using ShoesStore_Project.RequestDTO;
using System.Diagnostics;

namespace ShoesStore_Project.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpPost("AddNewProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddNewProduct([FromForm] ProductRequestDTO requestDTO)
        {
            if (requestDTO == null || requestDTO.images == null)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                Dictionary<int, int> sizeQuantities = Utility.ConvertSizeQuantityToDictionary(requestDTO.SizeQuantities);

                ProductDTO ProductdTO = new ProductDTO(null, requestDTO.Title, requestDTO.CategoryID, requestDTO.BrandID, null, requestDTO.Price);
                int newID = await service.AddNewProduct(sizeQuantities, ProductdTO, requestDTO.images);
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

        [HttpGet("GetProductsPaged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductDetailsDTO>>> GetProductsPaged(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                List<ProductDetailsDTO> data = await service.GetProductsPaged(pageNumber, pageSize);
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

        [HttpGet("GetCountProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetCountProducts()
        {
            try
            {
           
                return Ok(await service.GetCountProducts());
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


        [HttpGet("GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductSummaryDTO>> GetProduct(int ProductID)
        {
            try
            {
                ProductSummaryDTO product = await service.GetProduct(ProductID);
                if (product == null)
                {
                    return NotFound($"not found product that has ID {ProductID}");
                }
                return Ok(product);
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

        [HttpGet("GetProductImages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<string>>> GetProductImages(int ProductID)
        {
            try
            {
                List<string> productImages = await service.GetProductImages(ProductID);
                if (productImages == null || productImages.Count < 1)
                {
                    return NotFound($"not found product images that has ID {ProductID}");
                }
                return Ok(productImages);
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



        [HttpDelete("DeleteProduct/{ProductID}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProduct(int ProductID)
        {
            try
            {
                bool res = await service.DeleteProduct(ProductID);

                if (res)
                {
                    return NoContent(); 
                }

                return NotFound($"Product with ID {ProductID} not found."); 
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


        [HttpGet("GetAllSizeQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductInventoryDTO>>> GetAllSizeQuantity(int ProductID)
        {
            try
            {
                List<ProductInventoryDTO> SizeQuantityList = await service.GetAllSizeQuantity(ProductID);

                if (SizeQuantityList.Count <= 0)
                {
                    return NotFound($"not found product that has ID {ProductID}");
                }
                return Ok(SizeQuantityList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while post. excrption message is {ex}");
            }
        }

        [HttpPost("AddNewSizeQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> AddNewSizeQuantity(ProductSizeQuantityCreateDTO SQ_Data)
        {
            try
            {
                int NewStorageID = await service.AddNewSizeQuantity(SQ_Data);
                if (NewStorageID <= 0)
                {
                    return NotFound($"not found product that has ID {SQ_Data.ProductID}");
                }

                return Ok(NewStorageID);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while post. excrption message is {ex}");
            }
        }


        [HttpPatch("UpdateProductQuantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateProductQuantity(int StorageID, byte Quantity)
        {
            if (StorageID <= 0)
            {
                return BadRequest("StorageID is Not Correct");
            }
            try
            {
                bool UpdateResault = await service.UpdateProductQuantity(StorageID, Quantity);
                if (!UpdateResault)
                {
                    return NotFound($"Not Found result with StorageId :{StorageID}");
                }
                return Ok(UpdateResault);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex}");

            }
        }



        [HttpPatch("UpdateProductPrice")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateProductPrice(int ProductID, decimal Price)
        {
            if (ProductID <= 0)
            {
                return BadRequest("ProductID is Not Correct");
            }
            try
            {
                bool UpdateResault = await service.UpdateProductPrice(ProductID, Price);
                if (!UpdateResault)
                {
                    return NotFound($"Not Found result with ProductID :{ProductID}");
                }
                return Ok(UpdateResault);
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Database error: {ex}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex}");

            }
        }


    }
}
