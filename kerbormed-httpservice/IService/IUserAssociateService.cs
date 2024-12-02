using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IUserAssociateService
    {
        Task<AssociateUserToOrganization.Types.Response> AssociateUserToOrganization(UserOrganizations userOrganization);
        Task<DisassociateUserFromOrganization.Types.Response> DisassociateUserFromOrganization(UserOrganizations userOrganization);
        Task<QueryUsersForOrganization.Types.Response> QueryUsersForOrganization(Pagination pagination, int organizationId);
    }
}
