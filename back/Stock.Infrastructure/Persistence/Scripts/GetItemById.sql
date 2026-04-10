CREATE OR ALTER PROCEDURE [dbo].[GetItemById]
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        i.[ItemId]
        , i.[Name] 
        , i.[Notes]
        , i.[CategoryId]
        , i.[CreatedAt]
        , i.[UpdatedAt]        
    FROM [dbo].[Item] i
    LEFT JOIN [dbo].[Storage] s 
        ON i.[ItemId] = s.[ItemId]
    LEFT JOIN [dbo].[Box] b 
        ON s.[BoxId] = b.[BoxId]
    WHERE 
        i.[ItemId] = @ItemId
    GROUP BY 
        i.[ItemId]
        , i.[Name] 
        , i.[Notes]
        , i.[CategoryId]
        , i.[CreatedAt]
        , i.[UpdatedAt];

END
