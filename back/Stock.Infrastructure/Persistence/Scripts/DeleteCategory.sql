CREATE OR ALTER PROCEDURE [dbo].[DeleteCategory]
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        
        DELETE FROM [dbo].Category WHERE CategoryId = @CategoryId;
        
        EXEC [dbo].[OrganizeCategories]

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END