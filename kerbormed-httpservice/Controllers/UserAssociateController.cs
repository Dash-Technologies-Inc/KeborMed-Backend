using Grpc.Core;
using kebormed.grpcservice.Models;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;
using Microsoft.AspNetCore.Mvc;

namespace kerbormed_httpservice.Controllers
{
    public class UserAssociateController : Controller
    {
        private readonly IUserAssociateService _userAssociateService;

        public UserAssociateController(IUserAssociateService userAssociateService)
        {
            _userAssociateService = userAssociateService;
        }


        [HttpPost("AssociateUserToOrganization")]
        public async Task<IActionResult> AssociateUserToOrganization(UserOrganization userOrganization)
        {
            try
            {
                var response = await _userAssociateService.AssociateUserToOrganization(userOrganization);
                return Ok(response);


            }
            catch (RpcException ex)
            {
                // Handle gRPC exceptions (e.g., if the service is unavailable)
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
        }

        [HttpPost("DisassociateUserFromOrganization")]
        public async Task<IActionResult> DisassociateUserFromOrganization(UserOrganization userOrganization)
        {
            try
            {
                var response = await _userAssociateService.DisassociateUserFromOrganization(userOrganization);
                return Ok(response);


            }
            catch (RpcException ex)
            {
                // Handle gRPC exceptions (e.g., if the service is unavailable)
                return StatusCode(500, $"gRPC Error: {ex.Message}");
            }
        }

        [HttpPost("QueryUsersForOrganization/{organizationId}")]
        public async Task<IActionResult> QueryUsersForOrganization(Pagination pagination, int organizationId)
        {
            try
            {
                var response = await _userAssociateService.QueryUsersForOrganization(pagination, organizationId);
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
