 -- =============================================
-- Setup Test Database for GarageSpace API
-- =============================================

-- Create SQL Login for test user
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'GarageSpaceAPIUser')
BEGIN
    CREATE LOGIN GarageSpaceAPIUser WITH PASSWORD = 'Passw0rd123';
    PRINT 'Login GarageSpaceAPIUser created successfully.';
END
ELSE
BEGIN
    PRINT 'Login GarageSpaceAPIUser already exists.';
END

-- Create test database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GarageSpaceAPI')
BEGIN
    CREATE DATABASE GarageSpaceAPI;
    PRINT 'Database GarageSpaceAPI created successfully.';
END
ELSE
BEGIN
    PRINT 'Database GarageSpaceAPI already exists.';
END

GO

-- Switch to the test database
USE GarageSpaceAPI;

-- Create database user
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'GarageSpaceAPIUser')
BEGIN
    CREATE USER GarageSpaceAPIUser FOR LOGIN GarageSpaceAPIUser;
    PRINT 'User GarageSpaceAPIUser created successfully in GarageSpaceAPI.';
END
ELSE
BEGIN
    PRINT 'User GarageSpaceAPIUser already exists in GarageSpaceAPI.';
END

-- Grant necessary permissions to the test user
-- db_owner role for full access during testing
ALTER ROLE db_owner ADD MEMBER GarageSpaceAPIUser;
PRINT 'Granted db_owner permissions to GarageSpaceAPIUser in GarageSpaceAPI.';

-- Additional specific permissions if needed
GRANT CREATE TABLE TO GarageSpaceAPIUser;
GRANT ALTER ON SCHEMA::dbo TO GarageSpaceAPIUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO GarageSpaceAPIUser;
PRINT 'Granted additional permissions to GarageSpaceAPIUser in GarageSpaceAPI.';

-- Verify the setup
SELECT 
    'Login Status' as Check_Type,
    CASE 
        WHEN EXISTS (SELECT * FROM sys.server_principals WHERE name = 'GarageSpaceAPIUser') 
        THEN 'GarageSpaceAPIUser login exists' 
        ELSE 'GarageSpaceAPIUser login missing' 
    END as Status
UNION ALL
SELECT 
    'Database Status',
    CASE 
        WHEN EXISTS (SELECT * FROM sys.databases WHERE name = 'GarageSpaceAPI') 
        THEN 'GarageSpaceAPI database exists' 
        ELSE 'GarageSpaceAPI database missing' 
    END
UNION ALL
SELECT 
    'User Status',
    CASE 
        WHEN EXISTS (SELECT * FROM sys.database_principals WHERE name = 'GarageSpaceAPIUser') 
        THEN 'GarageSpaceAPIUser user exists in GarageSpaceAPI' 
        ELSE 'GarageSpaceAPIUser user missing in GarageSpaceAPI' 
    END;

PRINT 'Test database setup completed successfully!';
PRINT 'Connection string: Server=(localdb)\mssqllocaldb;Database=GarageSpaceAPI;User Id=GarageSpaceAPIUser;Password=Passw0rd123;MultipleActiveResultSets=true';