using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IUserService
    {
        // Service Create User
        Task<CreateUser.Types.Response> CreateUser(User user);

        // Service Get User By User Id
        Task<GetUser.Types.Response> GetUserAsync(int userId);

        // Service Update User By User Id
        Task<UpdateUser.Types.Response> UpdateUserAsync(int userId, User user);

        // Service Delete User By User Id
        Task<DeleteUser.Types.Response> DeleteUserAsync(int userId);
        // Service Query User By User Id
        Task<QueryUsers.Types.Response> QueryUserAsync(Pagination pagination);

    }
}
