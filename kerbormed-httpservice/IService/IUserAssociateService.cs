using kebormed.grpcservice.Models;
using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IUserAssociateService
    {
        Task<AssociateUserToOrganization.Types.Response> AssociateUserToOrganization(UserOrganization userOrganization);
        Task<DisassociateUserFromOrganization.Types.Response> DisassociateUserFromOrganization(UserOrganization userOrganization);
        Task<QueryUsersForOrganization.Types.Response> QueryUsersForOrganization(Pagination pagination, int organizationId);
    }
}
