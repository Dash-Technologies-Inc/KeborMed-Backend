using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IOrganizationService
    {
        // Service Create Organization
        Task<CreateOrganization.Types.Response> CreateOrganizationAsync(Organization organization);
        // Service Get Organization
        Task<GetOrganization.Types.Response> GetOrganizationAsync(int organizationId);
        // Service Update Organization
        Task<UpdateOrganization.Types.Response> UpdateOrganizationAsync(int organizationId, Organization organization);
        // Service Delete Organization
        Task<DeleteOrganization.Types.Response> DeleteOrganizationAsync(int organizationId);
        // Service Query Organization
        Task<QueryOrganizations.Types.Response> QueryOrganizationsAsync(Pagination pagination);
    }
}
