using Grpc.Core;
using kebormed.grpcservice.Protos;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;
using Microsoft.AspNetCore.Mvc;

namespace kerbormed_httpservice.Controllers
{
 
    public class OrganizationController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpPost("CreateOrganization")]
        public async Task<IActionResult> CreateOrganization([FromBody] Organization request)
        {
            try
            {
                var response = await _organizationService.CreateOrganizationAsync(request);
                return Ok(response);


            }
            catch (RpcException ex)
            {
                // Handle gRPC exceptions (e.g., if the service is unavailable)
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
        }

        [HttpGet("GetOrganization/{organizationId}")]
        public async Task<IActionResult> GetOrganization(int organizationId)
        {
            try
            {
                var response = await _organizationService.GetOrganizationAsync(organizationId);
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


        [HttpPost("UpdateOrganization/{organizationId}")]
        [ProducesResponseType(typeof(Organization), 200)]
        public async Task<IActionResult> UpdateOrganization(int organizationId, [FromBody] Organization request)
        {
            var response = await _organizationService.UpdateOrganizationAsync(organizationId, request);
            return Ok(response);
        }


        [HttpDelete("DeleteOrganization/{organizationId}")]
        public async Task<IActionResult> DeleteOrganization(int organizationId)
        {
            try
            {
                var response = await _organizationService.DeleteOrganizationAsync(organizationId);
                return Ok(response); // Return 200 OK with the response
            }
            catch (RpcException ex)
            {
                // Handle gRPC errors (e.g., if the service is unavailable)
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other errors
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpPost("QueryOrganizations")]
        public async Task<IActionResult> QueryOrganizations(Pagination pagination)
        {
            try
            {
                var response = await _organizationService.QueryOrganizationsAsync(pagination);
                return Ok(response);
            }
            catch (RpcException ex)
            {
                // Handle gRPC exceptions (e.g., if the service is unavailable)
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other general exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
