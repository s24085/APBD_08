-- Drop foreign keys if they exist
IF OBJECT_ID('Country_Trip', 'U') IS NOT NULL
ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Country;

IF OBJECT_ID('Country_Trip', 'U') IS NOT NULL
ALTER TABLE Country_Trip DROP CONSTRAINT Country_Trip_Trip;

IF OBJECT_ID('Client_Trip', 'U') IS NOT NULL
ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Client;

IF OBJECT_ID('Client_Trip', 'U') IS NOT NULL
ALTER TABLE Client_Trip DROP CONSTRAINT Table_5_Trip;

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
