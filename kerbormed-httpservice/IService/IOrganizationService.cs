using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IOrganizationService
    {
        Task<CreateOrganization.Types.Response> CreateOrganizationAsync(Organization organization);
        Task<GetOrganization.Types.Response> GetOrganizationAsync(int organizationId);
        Task<UpdateOrganization.Types.Response> UpdateOrganizationAsync(int organizationId, Organization organization);
        Task<DeleteOrganization.Types.Response> DeleteOrganizationAsync(int organizationId);
        Task<QueryOrganizations.Types.Response> QueryOrganizationsAsync(Pagination pagination);
    }
}
