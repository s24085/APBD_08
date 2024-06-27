USE TripsDB;
GO

-- Drop foreign keys if they exist
IF OBJECT_ID('Client_Trip', 'U') IS NOT NULL
    BEGIN
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Table_5_Client]') AND parent_object_id = OBJECT_ID(N'[Client_Trip]'))
        ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Client;

        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Table_5_Trip]') AND parent_object_id = OBJECT_ID(N'[Client_Trip]'))
        ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Trip;
    END

IF OBJECT_ID('Country_Trip', 'U') IS NOT NULL
    BEGIN
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Country_Trip_Country]') AND parent_object_id = OBJECT_ID(N'[Country_Trip]'))
        ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Country;

        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[Country_Trip_Trip]') AND parent_object_id = OBJECT_ID(N'[Country_Trip]'))
        ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Trip;
    END

-- Drop tables if they exist
IF OBJECT_ID('Client', 'U') IS NOT NULL
    DROP TABLE Client;

IF OBJECT_ID('Client_Trip', 'U') IS NOT NULL
    DROP TABLE Client_Trip;

IF OBJECT_ID('Country', 'U') IS NOT NULL
    DROP TABLE Country;

IF OBJECT_ID('Country_Trip', 'U') IS NOT NULL
    DROP TABLE Country_Trip;

IF OBJECT_ID('Trip', 'U') IS NOT NULL
    DROP TABLE Trip;
GO
