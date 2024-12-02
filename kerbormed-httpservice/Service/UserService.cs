using kebormed.grpcservice.Protos;
using kerbormed_httpservice.IService;
using kerbormed_httpservice.Model;

namespace kerbormed_httpservice.Service
{
    public class UserService : IUserService
    {
        private readonly kebormed.grpcservice.Protos.UserService.UserServiceClient _grpcClient;

        public UserService(kebormed.grpcservice.Protos.UserService.UserServiceClient grpcClient)
        {
            _grpcClient = grpcClient;
        }
        // Service Create User
        public async Task<CreateUser.Types.Response> CreateUser(User user)
        {
            var request = new CreateUser.Types.Request
            {
                Username = user.Username,
                Name = user.Name,
                Email = user.Email
            };
            return await _grpcClient.CreateUserAsync(request);
        }

        // Service Get User By User Id
        public async Task<GetUser.Types.Response> GetUserAsync(int userId)
        {
            var request = new GetUser.Types.Request { UserId = userId };
            return await _grpcClient.GetUserAsync(request);
        }

        // Service Update User By User Id
        public async Task<UpdateUser.Types.Response> UpdateUserAsync(int userId, User user)
        {
            if (userId <= 0)
            {
                return new UpdateUser.Types.Response
                {
                    Success = false,
                    Message = "Please enter user Id"
                };
            }

            var request = new UpdateUser.Types.Request
            {
                UserId = userId,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email
            };

            return await _grpcClient.UpdateUserAsync(request);
        }

        // Service Delete User By User Id
        public async Task<DeleteUser.Types.Response> DeleteUserAsync(int userId)
        {
            var request = new DeleteUser.Types.Request
            {
                UserId = userId
            };

            return await _grpcClient.DeleteUserAsync(request);
        }

        // Service Query User By User Id
        public async Task<QueryUsers.Types.Response> QueryUserAsync(Pagination pagination)
        {
            var request = new QueryUsers.Types.Request
            {
                Page = pagination.Page,
                Direction = pagination.Direction,
                PageSize = pagination.PageSize,
                OrderBy = pagination.OrderBy,
                QueryString = pagination.QueryString
            };

            return await _grpcClient.QueryUsersAsync(request);
        }
    }
}
