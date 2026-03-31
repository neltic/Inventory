CREATE OR ALTER PROCEDURE [dbo].[GetBoxesLookup]
AS
BEGIN
    SET NOCOUNT ON;    

    ;WITH [BoxHierarchy] AS (
        
        SELECT 
            [BoxId]
            , [ParentBoxId]
            , [Name]
            , [UpdatedAt]
            , CAST(ROW_NUMBER() OVER (ORDER BY [BoxId]) AS VARCHAR(100)) AS [Level]
            , 0 AS [Indent] 
        FROM 
            [dbo].[Box]
        WHERE 
            [ParentBoxId] IS NULL

        UNION ALL
        
        SELECT 
            b.[BoxId]
            , b.[ParentBoxId]
            , b.[Name]
            , b.[UpdatedAt]
            , CAST(bh.[Level] + '.' + CAST(ROW_NUMBER() OVER (PARTITION BY b.[ParentBoxId] ORDER BY b.[BoxId]) AS VARCHAR(10)) AS VARCHAR(100)) AS [Level]
            , bh.[Indent] + 1 AS [Indent] 
        FROM 
            [dbo].[Box] b
            INNER JOIN [BoxHierarchy] bh 
                ON b.[ParentBoxId] = bh.[BoxId]
    )
    SELECT 
        [BoxId]
        , [Name]
        , [UpdatedAt]
        , [Indent] 
    FROM
        [BoxHierarchy]
    ORDER BY 
        [Level];
        
END
GO
