#!/bin/bash

# Start SQL Server in the background
/opt/mssql/bin/sqlservr &

# Wait until SQL Server is ready
echo "Waiting for SQL Server to start..."
until /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
    sleep 1
done

echo "SQL Server is ready. Running initialization script..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -i /init.sql

# Backup the database
echo "Backing up the database..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "$SA_PASSWORD" -Q "BACKUP DATABASE Dash TO DISK = '/var/opt/mssql/data/Dash.bak' WITH FORMAT;"

echo "Backup completed. Keeping the container running..."
wait
