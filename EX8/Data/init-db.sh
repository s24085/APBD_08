#!/bin/bash

# Ścieżka do pliku SQL na Twoim systemie plików
sqlScriptPath="/home/bart/Downloads/S7/APBD/APBD_08/EX8/Data/cw7_create.sql"

# Nazwa bazy danych
databaseName="TripsDB"

# Tworzenie bazy danych
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'StrongPass123' -Q "CREATE DATABASE [$databaseName]"

# Wykonanie skryptu SQL przy użyciu sqlcmd
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'StrongPass123' -d $databaseName -i $sqlScriptPath
