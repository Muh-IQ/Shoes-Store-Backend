using Azure.Core;
using Interfaces.DTOs;
using Interfaces.DTOs.ProductDTOs;
using Interfaces.Repository;
using Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using ShoesStore_Project.RequestDTO;

namespace ShoesStore_Project.Controllers.Login.Admine
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service) : ControllerBase
    {

        [HttpPost("AddNewUserAsAdmine")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddNewUserAsAdmine([FromForm] UserRequestDTO requestDTO)
        {
            if (requestDTO == null || requestDTO.image == null)
            {
                return BadRequest("data is requierd.");
            }

            try
            {
                UserDTO dTO = new UserDTO { Email = requestDTO.Email, IsAdmine = true, Password = requestDTO.Password, UserName = requestDTO.UserName };
                 
                int? newID = await service.AddUser(dTO, requestDTO.image); ;

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



        [HttpGet("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Login(string Email, string password)
        {
            if (string.IsNullOrEmpty (Email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("data is requierd.");
            }
            try

            {

                (string AccessToken, string RefreshToken) Result = await service.Login(Email, password); ;

                if (string.IsNullOrEmpty(Result.AccessToken) || string.IsNullOrEmpty(Result.RefreshToken))
                {
                    return Unauthorized();
                }



                return Ok(new { token = Result.AccessToken, refreshToken= Result.RefreshToken });
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

        [HttpGet("GetAccessToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> GetAccessToken(string RefreshToken)
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                return BadRequest("RefreshToken is requierd.");
            }

            try
            {

                string AccessToken = await service.GetAccessToken(RefreshToken); ;

                if (string.IsNullOrEmpty(AccessToken))
                {
                    return Unauthorized("Refresh token expired or revoked");
                }



                return Ok(new { token = AccessToken });
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

        [Authorize]
        [HttpGet("GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> GetUser()
        {
            var userId = User.FindFirst("userID")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }
          

            try
            {

                UserDTO user = await service.GetUser(int.Parse(userId)); ;

                if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.ImagePath))
                {
                    return NotFound("Not found this user");
                }



                return Ok(user);
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
