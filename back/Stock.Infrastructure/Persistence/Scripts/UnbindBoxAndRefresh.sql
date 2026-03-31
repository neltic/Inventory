CREATE OR ALTER PROCEDURE [dbo].[UnbindBoxAndRefresh]
    @BoxId INT
    , @ItemId INT
    , @BrandId INT
AS
BEGIN

    SET NOCOUNT ON;

    DELETE FROM [Storage] 
    WHERE [BoxId] = @BoxId AND [ItemId] = @ItemId AND [BrandId] = @BrandId;

    SELECT
    (
        SELECT 
           b.[BoxId] AS [boxId]
           , b.[Name] AS [name]
           , s.[BrandId] AS [brandId]
           , s.[Quantity] AS [quantity]
        FROM 
            [Storage] s
        INNER JOIN [Box] b 
            ON s.[BoxId] = b.[BoxId]
        WHERE 
            s.[ItemId] = @ItemId
        FOR JSON PATH 
    )
    AS [InBox]

END