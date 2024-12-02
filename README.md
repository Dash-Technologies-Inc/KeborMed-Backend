# KeborMed-Backend

This .net core(8) application demonstrates the implementation of User and Organization CRUD operation with query string api include pagination and User can associate/disassociate from organization 

# Getting Started

Prerequisites
1. Clone the repository: git clone https://github.com/Dash-Technologies-Inc/KeborMed-Backend.git cd <project_directory>
2. Install Visual Studio 2022
3. Install SQL 2019 Server and SSMS in windows (if required)
4. Install Docker desktop


# Running the App

Make sure install all dependencies
1. kebormed.grpcservice 
	1. Frameworks
		1. Microsoft.AspNetCore.App
		2. Microsoft.NETCore.App
	2. Add required NugGet packages
		1. Google.Protobuf
		2. Grpc
		3. Grpc.AspNetCore
		4. Grpc.Net.Client
		5. Grpc.Tools
		6. Microsoft.EntityFrameworkCore.Design
		7. Microsoft.EntityFrameworkCore.SqlServer
		8. Microsoft.EntityFrameworkCore.Tools

2. kerbormed-httpservice
	1. Frameworks
		a. Microsoft.AspNetCore.App
		b. Microsoft.NETCore.App
	2. Add required NugGet packages
		a. Grpc.core.api
		b. Grpc.Net.ClientFactory

3. Run the application
	1. kebormed.grpcservice  - F5
	2. kerbormed-httpservice - F5

4. gRPC service
http://localhost:5106

5. For api test
https://localhost:44303/swagger/index.html

# Execute docker commands :
1. docker build -t mssql_demo .
2. docker run -p 1433:1433 --name mssql_demo_container -v "$(pwd):/backup" -d mssql_demo

If the above docker run command (2) doesn’t work, use: 

3. docker run -p 1433:1433 --name mssql_demo_container -d mssql_demo

Once the container is created, we can make sure it is running in the docker desktop software. 

# SQL server details
sql servername : localhost, 1433;
username : sa;
password : AdminStrong@Passw0rd


