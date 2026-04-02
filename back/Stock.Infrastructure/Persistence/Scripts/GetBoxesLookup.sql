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
            , CAST(RIGHT('00000' + CAST(ROW_NUMBER() OVER (ORDER BY [Name] ASC) AS VARCHAR(10)), 5) AS VARCHAR(MAX)) AS [SortPath]
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
            , CAST(bh.[SortPath] + '.' + RIGHT('00000' + CAST(ROW_NUMBER() OVER (PARTITION BY b.[ParentBoxId] ORDER BY b.[Name] ASC) AS VARCHAR(10)), 5) AS VARCHAR(MAX)) AS [SortPath]
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
        [SortPath]; 

END
GO