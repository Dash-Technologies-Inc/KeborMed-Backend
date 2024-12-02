Install SQL 2019 Server and SSMS in windows
Install Docker desktop

execute docker commands:

docker build -t mssql_demo .

excute first one command only if first not working then and only execute second
1. docker run -p 1433:1433 --name mssql_demo_container -v "$(pwd):/backup" -d mssql_demo
2. docker run -p 1433:1433 --name mssql_demo_container -d mssql_demo

Once the container is created, we can make sure it is running in the docker desktop software. 

sql servername : localhost, 1433;
username : sa;
password : AdminStrong@Passw0rd
