CREATE OR ALTER PROCEDURE [dbo].[GetBoxesByParent]
    @ParentBoxId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        b.[BoxId]
        , b.[ParentBoxId]
        , b.[Name]
        , b.CategoryId
        , b.BrandId
        , b.[UpdatedAt]
        , CAST(CASE 
            WHEN EXISTS (SELECT 1 FROM [dbo].[Box] sub WHERE sub.[ParentBoxId] = b.[BoxId]) 
            THEN 1 
            ELSE 0 
        END AS BIT) AS [HasChildren]
        , CAST(CASE 
            WHEN EXISTS (SELECT 1 FROM [dbo].[Storage] sub WHERE sub.[BoxId] = b.[BoxId]) 
            THEN 1 
            ELSE 0 
        END AS BIT) AS [HasItems]
    FROM 
        [dbo].[Box] b
    WHERE 
        (b.[ParentBoxId] = @ParentBoxId) OR (b.[ParentBoxId] IS NULL AND @ParentBoxId IS NULL);
END