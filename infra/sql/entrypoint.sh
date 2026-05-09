#!/bin/bash

/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to be ready..."

for i in {1..60}; do
    
    /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -C -Q "SELECT 1" &> /dev/null
    if [ $? -eq 0 ]; then
        echo "SQL Server is UP. Creating databases..."
        break
    fi
    echo "Still waiting for SQL Server... ($i/60)"
    sleep 2
done

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$MSSQL_SA_PASSWORD" -d master -i /usr/config/init-db.sql -C

echo "Databases initialized."

wait