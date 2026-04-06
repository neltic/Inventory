CREATE OR ALTER PROCEDURE [dbo].[ReorderCategory]
    @CategoryId INT,
    @NewOrder INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRANSACTION;
    BEGIN TRY
        
        DECLARE @OldOrder INT;
        SELECT @OldOrder = [Order] FROM [dbo].Category WHERE CategoryId = @CategoryId;
        
        IF (@OldOrder > @NewOrder)
        BEGIN
            UPDATE [dbo].Category 
            SET [Order] = [Order] + 1 
            WHERE [Order] >= @NewOrder AND [Order] < @OldOrder AND CategoryId <> @CategoryId;
        END
        
        ELSE IF (@OldOrder < @NewOrder)
        BEGIN
            UPDATE [dbo].Category 
            SET [Order] = [Order] - 1 
            WHERE [Order] > @OldOrder AND [Order] <= @NewOrder AND CategoryId <> @CategoryId;
        END
        
        UPDATE [dbo].Category SET [Order] = @NewOrder WHERE CategoryId = @CategoryId;
        
        EXEC [dbo].[OrganizeCategories];

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END