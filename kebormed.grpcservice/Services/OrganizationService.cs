using Grpc.Core;
using kebormed.grpcservice.Data;
using kebormed.grpcservice.Models;
using kebormed.grpcservice.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text.RegularExpressions;
using static kebormed.grpcservice.Protos.QueryOrganizations.Types;

namespace kebormed.grpcservice.Services
{

    public class OrganizationService : kebormed.grpcservice.Protos.OrganizationService.OrganizationServiceBase
    {
        private readonly ApplicationDbContext _context;

        public OrganizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task<CreateOrganization.Types.Response> CreateOrganization(CreateOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    // If organization name not entered
                    return new CreateOrganization.Types.Response
                    {
                        Message = "Please enter organization name"
                    };
                }
                var specialCharacterRegex = new Regex(@"^[a-zA-Z0-9\s]+$");
                if (!specialCharacterRegex.IsMatch(request.Name))
                {
                    return new CreateOrganization.Types.Response
                    {
                        // If organization name container special character
                        Message = "Organization name should not contain special characters. Only letters, numbers, and spaces are allowed."
                    };
                }

                var existingOrganization = await _context.Organizations.FirstOrDefaultAsync(o => o.Name == request.Name);

                if (existingOrganization != null)
                {
                    // If an organization with the same name exists, throw an exception or return a specific error response
                    return new CreateOrganization.Types.Response
                    {
                        Message = "An organization with this name already exists."
                    };
                }


                var organization = new Models.Organization
                {
                    Name = request.Name,
                    Address = request.Address,
                    CreatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.Organizations.Add(organization);
                await _context.SaveChangesAsync();

                return new CreateOrganization.Types.Response
                {
                    OrganizationId = organization.Id,
                    Message = "Organization created sucessfully."
                };

            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new CreateOrganization.Types.Response
                {
                    OrganizationId = 0,
                    Message = $"gRPC Error Service: {ex.Message}"
                };
            }
        }

        public override async Task<GetOrganization.Types.Response> GetOrganization(GetOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Fetch the organization from the database by ID
                var organization = await _context.Organizations
                    .Where(o => o.Id == request.OrganizationId && o.IsDeleted == false)
                    .FirstOrDefaultAsync();

                if (organization == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Organization not found"));
                }

                return new GetOrganization.Types.Response
                {
                    Name = organization.Name,
                    Address = organization.Address,
                    CreatedAt = organization.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),  // Convert to string format
                    UpdatedAt = organization.UpdatedAt != DateTime.MinValue
                    ? organization.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                    : null  // Handle nullable UpdatedAt
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        public override async Task<UpdateOrganization.Types.Response> UpdateOrganization(UpdateOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Find the organization by ID
                var organization = await _context.Organizations.FindAsync(request.OrganizationId);
                if (organization == null)
                {
                    return new UpdateOrganization.Types.Response
                    {
                        Success = false,
                        Message = "Organization not found"
                    };
                }

                // Update the organization properties
                organization.Name = request.Name ?? organization.Name; // Only update if the new value is not null
                organization.Address = request.Address ?? organization.Address; // Only update if the new value is not null
                organization.UpdatedAt = DateTime.UtcNow;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the updated organization details
                return new UpdateOrganization.Types.Response
                {
                    Success = true,
                    Message = "Organization updated sucessfully."
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new UpdateOrganization.Types.Response
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public override async Task<DeleteOrganization.Types.Response> DeleteOrganization(DeleteOrganization.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Find the organization by ID
                var organization = await _context.Organizations.Where(o => o.Id == request.OrganizationId && o.IsDeleted == false)
                    .FirstOrDefaultAsync();
                if (organization == null)
                {
                    return new DeleteOrganization.Types.Response
                    {
                        Success = false,
                        Message = "Organization not found"
                    };
                }

                organization.IsDeleted = true;
                organization.DeletedAt = DateTime.UtcNow;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return success response
                return new DeleteOrganization.Types.Response
                {
                    Success = true,
                    Message = $"Organization with ID {request.OrganizationId} successfully deleted."
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                return new DeleteOrganization.Types.Response
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public override async Task<QueryOrganizations.Types.Response> QueryOrganizations(QueryOrganizations.Types.Request request, ServerCallContext context)
        {
            try
            {
                // Build query based on request parameters
                var query = _context.Organizations.Where(o => o.IsDeleted == false).AsQueryable();

                // Sorting functionality
                if (!string.IsNullOrEmpty(request.OrderBy))
                {
                    switch (request.OrderBy.ToLower())
                    {
                        case "name":
                            query = request.Direction == "asc" ? query.OrderBy(o => o.Name) : query.OrderByDescending(o => o.Name);
                            break;
                        case "createdat":
                            query = request.Direction == "asc" ? query.OrderBy(o => o.CreatedAt) : query.OrderByDescending(o => o.CreatedAt);
                            break;
                        default:
                            break;
                    }
                }

                // Pagination functionality
                var total = await query.CountAsync();
                var organizations = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(o => new QueryOrganizations.Types.Organization
                    {
                        OrganizationId = o.Id,
                        Name = o.Name,
                        Address = o.Address,
                        CreatedAt = o.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                    .ToListAsync();

                // Search functionality (filter by name or address)
                if (!string.IsNullOrEmpty(request.QueryString))
                {
                    organizations = organizations
                        .Where(o => o.Name.ToLower().Contains(request.QueryString.ToLower())
                                 || o.Address.ToLower().Contains(request.QueryString.ToLower()))
                        .ToList();
                }

                // Return the response
                return new QueryOrganizations.Types.Response
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    OrganizationList = { organizations }
                };
            }
            catch (Exception ex)
            {
                // Handle gRPC-specific exceptions and return with status code
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

    }
}
