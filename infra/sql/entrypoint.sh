#!/bin/bash

/opt/mssql/bin/sqlservr &

echo "Waiting for SQL Server to start..."
sleep 30s

/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -d master -i /usr/config/init-db.sql -C

wait