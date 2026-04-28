CREATE OR ALTER PROCEDURE [dbo].[AddOrEditStorage]    
    @BoxId INT
    , @ItemId INT
    , @BrandId INT
    , @Quantity INT
    , @Expires BIT
    , @ExpiresOn DATE = NULL
    , @Notes NVARCHAR(512) = NULL
AS
BEGIN

    IF @Expires = 0
    BEGIN
        SET @ExpiresOn = NULL;
    END

    IF EXISTS (SELECT 1 FROM [dbo].[Storage] WHERE BoxId = @BoxId AND ItemId = @ItemId AND BrandId = @BrandId)
    BEGIN

        IF @Quantity > 0 
        BEGIN

            UPDATE 
                [dbo].[Storage]
            SET 
                Quantity = @Quantity, 
                Expires = @Expires, 
                ExpiresOn = @ExpiresOn,
                Notes = @Notes,
                UpdatedAt = SYSDATETIMEOFFSET()
            WHERE 
                BoxId = @BoxId
                AND ItemId = @ItemId                 
                AND BrandId = @BrandId;

        END
        ELSE
        BEGIN

            DELETE 
                [dbo].[Storage]
            WHERE 
                BoxId = @BoxId
                AND ItemId = @ItemId                 
                AND BrandId = @BrandId;

        END

    END
    ELSE
    BEGIN

        IF @Quantity > 0 
        BEGIN

            INSERT INTO [dbo].[Storage]
                (BoxId, ItemId, BrandId, Quantity, Expires, ExpiresOn, Notes, CreatedAt, UpdatedAt)
            VALUES 
                (@BoxId, @ItemId, @BrandId, @Quantity, @Expires, @ExpiresOn, @Notes, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET());

        END

    END
   
END