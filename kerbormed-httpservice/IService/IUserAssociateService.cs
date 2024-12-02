using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IUserAssociateService
    {
        // Service Associate User To Organization
        Task<AssociateUserToOrganization.Types.Response> AssociateUserToOrganization(UserOrganizations userOrganization);

        // Service Disassociate User From Organization
        Task<DisassociateUserFromOrganization.Types.Response> DisassociateUserFromOrganization(UserOrganizations userOrganization);

        // Service Query User From Organization
        Task<QueryUsersForOrganization.Types.Response> QueryUsersForOrganization(Pagination pagination, int organizationId);
    }
}
