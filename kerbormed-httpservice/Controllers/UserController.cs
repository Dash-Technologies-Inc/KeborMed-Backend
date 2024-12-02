using Grpc.Core;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;
using Microsoft.AspNetCore.Mvc;

namespace kerbormed_httpservice.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            try
            {
                var response = await _userService.CreateUser(user);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                // Handle gRPC exceptions (e.g., if the service is unavailable)
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("GetUser/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var response = await _userService.GetUserAsync(userId);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("UpdateUser/{userId}")]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] User user)
        {
            var response = await _userService.UpdateUserAsync(userId, user);
            return Ok(response);
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var response = await _userService.DeleteUserAsync(userId);

                if (response.Success)
                {
                    return Ok(response); 
                }
                else
                {
                    // Return 404 Not Found if the organization was not found
                    return NotFound(response.Message);
                }
            }
            catch (RpcException ex)
            {
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("QueryUser")]
        public async Task<IActionResult> QueryUser(Pagination pagination)
        {
            try
            {
                var response = await _userService.QueryUserAsync(pagination);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
