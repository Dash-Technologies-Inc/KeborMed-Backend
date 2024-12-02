using kebormed.grpcservice.Protos;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;
using static kebormed.grpcservice.Protos.QueryOrganizations.Types;

namespace kerbormed_httpservice.Service
{
    public class UserAssociateService : IUserAssociateService
    {
        private readonly kebormed.grpcservice.Protos.UserService.UserServiceClient _grpcClient;

        public UserAssociateService(kebormed.grpcservice.Protos.UserService.UserServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<AssociateUserToOrganization.Types.Response> AssociateUserToOrganization(UserOrganizations userOrganization)
        {
            var request = new AssociateUserToOrganization.Types.Request
            {
                OrganizationId = userOrganization.OrganizationId,
                UserId = userOrganization.UserId,
            };
            return await _grpcClient.AssociateUserToOrganizationAsync(request);
        }
        public async Task<DisassociateUserFromOrganization.Types.Response> DisassociateUserFromOrganization(UserOrganizations userOrganization)
        {
            var request = new DisassociateUserFromOrganization.Types.Request
            {
                OrganizationId = userOrganization.OrganizationId,
                UserId = userOrganization.UserId,
            };
            return await _grpcClient.DisassociateUserFromOrganizationAsync(request);
        }

        public async Task<QueryUsersForOrganization.Types.Response> QueryUsersForOrganization(Pagination pagination, int organizationId)
        {
            var request = new QueryUsersForOrganization.Types.Request
            {
                Page = pagination.Page,
                Direction = pagination.Direction,
                PageSize = pagination.PageSize,
                OrderBy = pagination.OrderBy,
                QueryString = pagination.QueryString,
                OrganizationId = organizationId
            };

            return await _grpcClient.QueryUsersForOrganizationAsync(request);
        }
    }
}
