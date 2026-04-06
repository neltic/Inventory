CREATE OR ALTER PROCEDURE [dbo].[MoveBox]
    @BoxId INT
    , @NewParentId INT = NULL
AS
BEGIN
    
    SET XACT_ABORT ON; 

    BEGIN TRY
        BEGIN TRANSACTION;
        
        IF NOT EXISTS (SELECT 1 FROM [dbo].[Box] WHERE [BoxId] = @BoxId)
        BEGIN
            ;THROW 50001, 'The original box does not exist.', 1;
        END

        IF @BoxId = @NewParentId
        BEGIN
            ;THROW 50002, 'A box cannot be placed inside itself.', 1;
        END
        
        IF @NewParentId IS NOT NULL
        BEGIN
            
            IF NOT EXISTS (SELECT 1 FROM [dbo].[Box] WHERE [BoxId] = @NewParentId)
            BEGIN
                ;THROW 50003, 'The destination box does not exist.', 1;
            END
            
            DECLARE @IsDescendant BIT = 0;

            ;WITH [Descendants] AS (
                SELECT [BoxId] FROM [dbo].[Box] WHERE [ParentBoxId] = @BoxId
                UNION ALL
                SELECT b.[BoxId] 
                FROM [dbo].[Box] b
                INNER JOIN [Descendants] d ON b.[ParentBoxId] = d.[BoxId]
            )
            SELECT @IsDescendant = 1 
            FROM [Descendants] 
            WHERE [BoxId] = @NewParentId;

            IF @IsDescendant = 1
            BEGIN
                ;THROW 50004, 'Invalid move: The destination is a descendant of the source box.', 1;
            END
        END

        UPDATE 
            [dbo].[Box]
        SET 
            [ParentBoxId] = @NewParentId
            , [UpdatedAt] = GETUTCDATE()
        WHERE 
            [BoxId] = @BoxId;

        COMMIT TRANSACTION;        

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);

    END CATCH
END
