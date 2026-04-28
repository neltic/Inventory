CREATE OR ALTER PROCEDURE [dbo].[GetBoxById]
    @BoxId INT
AS
BEGIN
    
    SET NOCOUNT ON;

    ;WITH BoxHierarchy AS (

        SELECT 
            p.[BoxId]
            , p.[parentBoxId]
            , p.[Name]
            , 0 AS [Level]
        FROM [Box] b
        INNER JOIN [Box] p ON b.[parentBoxId] = p.[BoxId]
        WHERE b.[BoxId] = @BoxId

        UNION ALL

        SELECT 
            p.[BoxId], 
            p.[parentBoxId], 
            p.[Name], 
            bh.[Level] + 1 AS [Level]
        FROM [Box] p
        INNER JOIN BoxHierarchy bh ON bh.[parentBoxId] = p.[BoxId]
    )
    SELECT 
        b.[BoxId]
        , b.[ParentBoxId]
        , b.[Name]
        , b.[BrandId]
        , b.[CategoryId]
        , b.[Height]
        , b.[Width]
        , b.[Depth]
        , b.[Volume]
        , b.[Notes]
        , b.[CreatedAt]
        , b.[UpdatedAt]
        , (
            SELECT 
                [BoxId] AS [boxId], 
                [Name] AS [name]
            FROM BoxHierarchy
            ORDER BY [Level] DESC 
            FOR JSON PATH
          ) AS [FullPath]
        , CAST(CASE 
            WHEN EXISTS (SELECT 1 FROM [Box] WHERE [ParentBoxId] = b.[BoxId]) THEN 0
            WHEN EXISTS (SELECT 1 FROM [dbo].[Storage] WHERE [BoxId] = b.[BoxId]) THEN 0
            ELSE 1 
          END AS BIT) AS [CanBeDeleted]
    FROM [Box] b
    WHERE b.[BoxId] = @BoxId;

END;