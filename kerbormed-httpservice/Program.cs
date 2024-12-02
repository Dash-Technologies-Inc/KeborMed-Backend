using kerbormed_httpservice.IService;
using kerbormed_httpservice.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<kebormed.grpcservice.Protos.OrganizationService.OrganizationServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5106/"); // Register the gRPC service
});
builder.Services.AddGrpcClient<kebormed.grpcservice.Protos.UserService.UserServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5106/"); // Register the gRPC service
});

builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IUserAssociateService,UserAssociateService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
