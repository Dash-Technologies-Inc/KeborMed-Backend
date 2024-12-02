Install SQL 2022 Server and SSMS in windows
Install Docker desktop

docker commands:

docker build -t mssql_demo .

docker run -p 1433:1433 --name mssql_demo_container -d mssql_demo
docker run -p 1433:1433 --name mssql_demo_container -v "$(pwd):/backup" -d mssql_demo

sql servername : localhost, 1433;
username : sa;
password : AdminStrong@Passw0rd
