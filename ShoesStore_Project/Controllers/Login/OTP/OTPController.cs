using Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ShoesStore_Project.Controllers.Login.OTP
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController(IOTPService service) : ControllerBase
    {
        [HttpPost("AddOTP")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> AddOTP(string Email)
        {
            if (string.IsNullOrEmpty(Email) )
            {
                return BadRequest("Email is requierd.");
            }
            try
            {
                bool Result = await service.AddOTP(Email); ;

                if (!Result)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred in add otp");
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

        [HttpGet("VerifyOTP")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> VerifyOTP(string Email, string otp)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(otp))
            {
                return BadRequest("Email and otp are requierd.");
            }
            try
            {
                bool Result = await service.VerifyOTP(Email, otp); ;

                if (!Result)
                {
                    return NotFound("OTP is not correct");
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
