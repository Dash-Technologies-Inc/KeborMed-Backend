using Grpc.Net.Client;
using kebormed.grpcservice;
using kebormed.grpcservice.Data;
using kebormed.grpcservice.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Logging.AddConsole();

builder.Services.AddSingleton(GrpcChannel.ForAddress("http://localhost:5106"));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<OrganizationService>();
app.MapGrpcService<UserService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
