 -- =============================================
-- Setup Test Database for TaranSoft.MyGarage Integration Tests
-- =============================================

-- Create SQL Login for test user
IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = 'GarageSpaceAPITestsUser')
BEGIN
    CREATE LOGIN GarageSpaceAPITestsUser WITH PASSWORD = 'Passw0rd123';
    PRINT 'Login GarageSpaceAPITestsUser created successfully.';
END
ELSE
BEGIN
    PRINT 'Login GarageSpaceAPITestsUser already exists.';
END

-- Create test database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'GarageSpaceAPITests')
BEGIN
    CREATE DATABASE GarageSpaceAPITests;
    PRINT 'Database GarageSpaceAPITests created successfully.';
END
ELSE
BEGIN
    PRINT 'Database GarageSpaceAPITests already exists.';
END

GO

-- Switch to the test database
USE GarageSpaceAPITests;

-- Create database user
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = 'GarageSpaceAPITestsUser')
BEGIN
    CREATE USER GarageSpaceAPITestsUser FOR LOGIN GarageSpaceAPITestsUser;
    PRINT 'User GarageSpaceAPITestsUser created successfully in GarageSpaceAPITests.';
END
ELSE
BEGIN
    PRINT 'User GarageSpaceAPITestsUser already exists in GarageSpaceAPITests.';
END

-- Grant necessary permissions to the test user
-- db_owner role for full access during testing
ALTER ROLE db_owner ADD MEMBER GarageSpaceAPITestsUser;
PRINT 'Granted db_owner permissions to GarageSpaceAPITestsUser in GarageSpaceAPITests.';

-- Additional specific permissions if needed
GRANT CREATE TABLE TO GarageSpaceAPITestsUser;
GRANT ALTER ON SCHEMA::dbo TO GarageSpaceAPITestsUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO GarageSpaceAPITestsUser;
PRINT 'Granted additional permissions to GarageSpaceAPITestsUser in GarageSpaceAPITests.';

-- Verify the setup
SELECT 
    'Login Status' as Check_Type,
    CASE 
        WHEN EXISTS (SELECT * FROM sys.server_principals WHERE name = 'GarageSpaceAPITestsUser') 
        THEN 'GarageSpaceAPITestsUser login exists' 
        ELSE 'GarageSpaceAPITestsUser login missing' 
    END as Status
UNION ALL
SELECT 
    'Database Status',
    CASE 
        WHEN EXISTS (SELECT * FROM sys.databases WHERE name = 'GarageSpaceAPITests') 
        THEN 'GarageSpaceAPITests database exists' 
        ELSE 'GarageSpaceAPITests database missing' 
    END
UNION ALL
SELECT 
    'User Status',
    CASE 
        WHEN EXISTS (SELECT * FROM sys.database_principals WHERE name = 'GarageSpaceAPITestsUser') 
        THEN 'GarageSpaceAPITestsUser user exists in GarageSpaceAPITests' 
        ELSE 'GarageSpaceAPITestsUser user missing in GarageSpaceAPITests' 
    END;

PRINT 'Test database setup completed successfully!';
PRINT 'Connection string: Server=(localdb)\mssqllocaldb;Database=GarageSpaceAPITests;User Id=GarageSpaceAPITestsUser;Password=Passw0rd123;MultipleActiveResultSets=true';