using Interfaces.DTOs.ProductDTOs;
using Interfaces.Utiltiy.IMisc;
using Interfaces.Utiltiy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Interfaces.Services;
using Interfaces.DTOs.Basket_DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ShoesStore_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController(IBasketService service) : ControllerBase
    {
        //  يجب ان يكون user id from token

        [HttpPost("AddToBasket")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddToBasket(Add_ChangeToBasketDTO basketDTO)
        {
            if (basketDTO.ProductID < 1 || basketDTO.Quantity < 1)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                var userId = User.FindFirst("userID")?.Value;
                //basketDTO.UserID = int.Parse(userId);

                bool res = await service.Add(basketDTO);
                if (!res)
                {
                    return StatusCode(500, "Something went wrong while adding the item.");
                }

                return NoContent();
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


        [HttpGet("GetBasketPaged")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BasketItemDTO>>> GetBasketPaged(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                var userId = User.FindFirst("userID")?.Value;
                int UserID = int.Parse(userId);

                List<BasketItemDTO> data = (await service.GetBasketPaged(UserID,pageNumber, pageSize)).ToList();
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

        [HttpPut ("ChangeBasketItemQuantity")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ChangeBasketItemQuantity(Add_ChangeToBasketDTO basketDTO)
        {
            if (basketDTO.ProductID < 1 || basketDTO.Quantity < 1)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                var userId = User.FindFirst("userID")?.Value;
                //basketDTO.UserID = int.Parse(userId);

                bool res = await service.ChangeBasketItemQuantity(basketDTO);
                if (!res)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while changing the Quantity.");
                }

                return NoContent();
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


        [HttpDelete("DeleteAllBasketItem")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAllBasketItem()
        {
     

            try
            {
                var userId = User.FindFirst("userID")?.Value;
                //int UserID = int.Parse(userId);

                bool res = await service.DeleteAllBasketItem(15);
                if (!res)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while Deleting the items.");
                }

                return NoContent();
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
