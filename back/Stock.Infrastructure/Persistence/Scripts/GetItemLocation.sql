CREATE OR ALTER PROCEDURE [dbo].[GetItemLocation]
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.[BoxId]
        , b.[Name]
        , s.[BrandId]
        , s.[Quantity]
    FROM 
        [Storage] s
    INNER JOIN [Box] b 
        ON s.[BoxId] = b.[BoxId]
    WHERE 
        s.[ItemId] = @ItemId;

END
