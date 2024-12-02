using kebormed.grpcservice.Protos;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.Service
{
    public class OrganizationService : IOrganizationService
    {
        private readonly kebormed.grpcservice.Protos.OrganizationService.OrganizationServiceClient _grpcClient;
        public OrganizationService(kebormed.grpcservice.Protos.OrganizationService.OrganizationServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }

        public async Task<CreateOrganization.Types.Response> CreateOrganizationAsync(Organization organization)
        {
            var request = new CreateOrganization.Types.Request
            {
                OrganizationId = organization.Id,
                Address = organization.Address,
                Name = organization.Name
            };

            return await _grpcClient.CreateOrganizationAsync(request);
        }

        public async Task<GetOrganization.Types.Response> GetOrganizationAsync(int organizationId)
        {
            var request = new GetOrganization.Types.Request { OrganizationId = organizationId };
            return await _grpcClient.GetOrganizationAsync(request);
        }
        public async Task<UpdateOrganization.Types.Response> UpdateOrganizationAsync(int organizationId, Organization organization)
        {
            if (organizationId <= 0)
            {
                return new UpdateOrganization.Types.Response
                {
                    Success = false,
                    Message = "Please enter organization Id"
                };
            }
            var request = new UpdateOrganization.Types.Request
            {
                OrganizationId = organizationId,
                Address = organization.Address,
                Name = organization.Name
            };

            return await _grpcClient.UpdateOrganizationAsync(request);
        }

        public async Task<DeleteOrganization.Types.Response> DeleteOrganizationAsync(int organizationId)
        {
            var request = new DeleteOrganization.Types.Request
            {
                OrganizationId = organizationId,
                IsDeleted = true
            };

           return await _grpcClient.DeleteOrganizationAsync(request);
        }

        public async Task<QueryOrganizations.Types.Response> QueryOrganizationsAsync(Pagination pagination)
        {
            var request = new QueryOrganizations.Types.Request
            {
                Page = pagination.Page,
                Direction = pagination.Direction,
                PageSize = pagination.PageSize,
                OrderBy = pagination.OrderBy,
                QueryString = pagination.QueryString
            };

            return await _grpcClient.QueryOrganizationsAsync(request);
        }
    }
}
