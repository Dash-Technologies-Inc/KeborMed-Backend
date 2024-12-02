using Google.Protobuf;
using Grpc.Core;
using kebormed.grpcservice.Data;
using kebormed.grpcservice.Models;
using kebormed.grpcservice.Protos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static kebormed.grpcservice.Protos.QueryOrganizations.Types;
using static kebormed.grpcservice.Protos.QueryUsers.Types;

namespace kebormed.grpcservice.Services
{
    public class UserService : kebormed.grpcservice.Protos.UserService.UserServiceBase
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task<CreateUser.Types.Response> CreateUser(CreateUser.Types.Request request, ServerCallContext context)
        {
            try
            {

                // If organization name not entered
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return new CreateUser.Types.Response
                    {
                        Message = "Please enter name"
                    };
                }

                // If name container special character
                var specialCharacterRegex = new Regex(@"^[a-zA-Z0-9\s]+$");
                if (!specialCharacterRegex.IsMatch(request.Name))
                {
                    return new CreateUser.Types.Response
                    {
                        Message = "Name should not contain special characters. Only letters, numbers, and spaces are allowed."
                    };
                }

                // If organization name not entered
                if (string.IsNullOrWhiteSpace(request.Username))
                {
                    return new CreateUser.Types.Response
                    {
                        Message = "Please enter username"
                    };
                }

                // Check if a user with the same username or email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

                // Check email is valid or not
                if (!Regex.IsMatch(request.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return new CreateUser.Types.Response
                    {
                        Message = "Please enter valid email address."
                    };
                }

                // Return if user is exist
                if (existingUser != null)
                {
                    return new CreateUser.Types.Response
                    {
                        Message = "Username or email already exists."
                    };
                }

                // Create a new user entity
                var user = new Models.User
                {
                    Name = request.Name,
                    Username = request.Username,
                    Email = request.Email,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                // Add the user to the database
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Return the user ID and messsage in the response
                return new CreateUser.Types.Response
                {
                    UserId = user.Id,
                    Message = "User added sucessfully."
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new CreateUser.Types.Response
                {
                    UserId = 0,
                    Message = ex.Message
                };
            }
        }

        public override async Task<GetUser.Types.Response> GetUser(GetUser.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Find the user by ID
                var user = await _context.Users
                  .Where(o => o.Id == request.UserId && o.IsDeleted == false)
                  .FirstOrDefaultAsync();

                // Return if user not found
                if (user == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
                }

                return new GetUser.Types.Response
                {
                    Name = user.Name,
                    Username = user.Username,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    UpdatedAt = user.UpdatedAt != DateTime.MinValue
                    ? user.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                    : null  // Handle nullable UpdatedAt
                };

            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<UpdateUser.Types.Response> UpdateUser(UpdateUser.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Find the user by ID
                var user = await _context.Users.FindAsync(request.UserId);

                if (user == null)
                {
                    return new UpdateUser.Types.Response
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                user.Name = request.Name;
                user.Username = request.Username;
                user.Email = request.Email;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new UpdateUser.Types.Response { Success = true, Message = "User updated sucessfully." };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new UpdateUser.Types.Response { Success = false, Message = ex.Message };
            }
        }

        public override async Task<DeleteUser.Types.Response> DeleteUser(DeleteUser.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Find the user by ID
                var user = await _context.Users.Where(o => o.Id == request.UserId && o.IsDeleted == false)
                   .FirstOrDefaultAsync();
                if (user == null)
                {
                    return new DeleteUser.Types.Response
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }
                user.IsDeleted = true;
                user.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                // Return success response
                return new DeleteUser.Types.Response
                {
                    Success = true,
                    Message = $"User with ID {request.UserId} successfully deleted."
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new DeleteUser.Types.Response
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public override async Task<QueryUsers.Types.Response> QueryUsers(QueryUsers.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Get all users
                var query = _context.Users.Where(o => o.IsDeleted == false).AsQueryable();

                // Set order by name, username or createdat
                if (!string.IsNullOrEmpty(request.OrderBy))
                {
                    switch (request.OrderBy.ToLower())
                    {
                        case "name":
                            query = request.Direction == "asc" ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                            break;
                        case "username":
                            query = request.Direction == "asc" ? query.OrderBy(o => o.Username) : query.OrderByDescending(o => o.Username);
                            break;
                        case "createdat":
                            query = request.Direction == "asc" ? query.OrderBy(o => o.CreatedAt) : query.OrderByDescending(o => o.CreatedAt);
                            break;
                        default:
                            break;
                    }
                }

                var total = await query.CountAsync();

                // Get the list of user as per pagination
                var users = await query.Skip((request.Page - 1) * request.PageSize)
                                        .Take(request.PageSize)
                                        .Select(u => new QueryUsers.Types.User
                                        {
                                            UserId = u.Id,
                                            Name = u.Name,
                                            Username = u.Username,
                                            Email = u.Email,
                                            CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                                            UpdatedAt = u.UpdatedAt.ToString()
                                        }).ToListAsync();

                // Search user by name, email or email
                if (!string.IsNullOrEmpty(request.QueryString))
                {
                    users = users
                        .Where(o => o.Name.ToLower().Contains(request.QueryString.ToLower())
                                 || o.Username.ToLower().Contains(request.QueryString.ToLower())
                                 || o.Email.ToLower().Contains(request.QueryString.ToLower()))
                        .ToList();
                }

                return new QueryUsers.Types.Response
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    Users = { users }
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<AssociateUserToOrganization.Types.Response> AssociateUserToOrganization(AssociateUserToOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Check user is already associated with organization or not 
                var existingUser = await _context.UserOrganizations.Where(o => o.UserId == request.UserId && o.OrganizationId == request.OrganizationId).FirstOrDefaultAsync();
                if (existingUser != null && existingUser?.IsAssociate == true)
                {
                    return new AssociateUserToOrganization.Types.Response
                    {
                        Success = false,
                        Message = $"User already associated with Organization Id {request.OrganizationId}."
                    };
                }

                var userOrg = new UserOrganizations
                {
                    Id = 0,
                    UserId = request.UserId,
                    OrganizationId = request.OrganizationId,
                    CreatedAt = DateTime.UtcNow,
                    IsAssociate = true,
                    IsDeleted = false
                };

                _context.UserOrganizations.Add(userOrg);
                await _context.SaveChangesAsync();

                // Return the user ID in the response
                return new AssociateUserToOrganization.Types.Response
                {
                    Success = true,
                    Message = $"User associate successfully with Organization Id {request.OrganizationId}."
                };

            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new AssociateUserToOrganization.Types.Response
                {
                    Success = false,
                    Message = ex.Message
                }; ;
            }
        }

        public override async Task<DisassociateUserFromOrganization.Types.Response> DisassociateUserFromOrganization(DisassociateUserFromOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                var userOrganization = await _context.UserOrganizations
                            .Where(org => org.UserId == request.UserId && org.OrganizationId == request.OrganizationId)
                            .FirstOrDefaultAsync();

                // If no association exists, return a response indicating this
                if (userOrganization == null)
                {
                    return new DisassociateUserFromOrganization.Types.Response
                    {
                        Success = false,
                        Message = "Association between the user and organization does not exist."
                    };
                }

                // Remove the association from the table
                _context.UserOrganizations.Remove(userOrganization);
                await _context.SaveChangesAsync();

                // Return the user ID in the response
                return new DisassociateUserFromOrganization.Types.Response
                {
                    Success = true,
                    Message = $"User disassociate successfully with Organization Id {request.OrganizationId}."
                };

            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new DisassociateUserFromOrganization.Types.Response
                {
                    Success = false,
                    Message = ex.Message
                }; ;
            }
        }


        public override async Task<QueryUsersForOrganization.Types.Response> QueryUsersForOrganization(
        QueryUsersForOrganization.Types.Request request,
        ServerCallContext context)
        {
            try
            {
                // Get base query

                var query = from post in _context.UserOrganizations
                            join meta in _context.Users on post.UserId equals meta.Id
                            where post.OrganizationId == request.OrganizationId
                            select new { Name = meta.Name, Username = meta.Username, Email = meta.Email, CreatedAt = meta.CreatedAt };

                // Apply search filter
                if (!string.IsNullOrEmpty(request.QueryString))
                {
                    query = query.Where(u =>
                        u.Name.Contains(request.QueryString) ||
                        u.Username.Contains(request.QueryString) ||
                        u.Email.Contains(request.QueryString));
                }

                // Apply sorting
                query = request.OrderBy?.ToLower() switch
                {
                    "name" => request.Direction?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Name)
                        : query.OrderBy(u => u.Name),
                    "username" => request.Direction?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Username)
                        : query.OrderBy(u => u.Username),
                    "email" => request.Direction?.ToLower() == "desc"
                        ? query.OrderByDescending(u => u.Email)
                        : query.OrderBy(u => u.Email),
                    _ => query.OrderBy(u => u.Name) // Default sorting
                };

                // Get total count
                var total = await query.CountAsync();

                // Apply pagination
                var users = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                // Map to gRPC response
                var response = new QueryUsersForOrganization.Types.Response
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                };

                response.Users.AddRange(users.Select(u => new QueryUsersForOrganization.Types.User
                {
                    Name = u.Name,
                    Username = u.Username,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                }));

                return response;
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

    }
}
