using kebormed.grpcservice.Protos;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.IService
{
    public interface IUserService
    {
        Task<CreateUser.Types.Response> CreateUser(User user);
        Task<GetUser.Types.Response> GetUserAsync(int userId);
        Task<UpdateUser.Types.Response> UpdateUserAsync(int userId, User user);
        Task<DeleteUser.Types.Response> DeleteUserAsync(int userId);
        Task<QueryUsers.Types.Response> QueryUserAsync(Pagination pagination);

    }
}
