CREATE OR ALTER PROCEDURE [dbo].[GetBoxFullPath]
    @BoxId INT
AS
BEGIN
    SET NOCOUNT ON;
        
    ;WITH ParentHierarchy AS (
        
        SELECT 
            p.[BoxId]
            , p.[ParentBoxId]
            , p.[Name]
            , 0 AS [Level]
        FROM 
            [Box] b
            INNER JOIN [Box] p 
                ON b.[ParentBoxId] = p.[BoxId]
        WHERE 
            b.[BoxId] = @BoxId

        UNION ALL
                
        SELECT 
            p.[BoxId]
            , p.[ParentBoxId]
            , p.[Name]
            , ph.[Level] + 1 AS [Level]
        FROM 
            [Box] p
            INNER JOIN ParentHierarchy ph 
                ON ph.[ParentBoxId] = p.[BoxId]
    )
    SELECT (
        SELECT 
            [BoxId] AS [boxId]
            , [Name] AS [name]
        FROM 
            ParentHierarchy
        ORDER BY 
            [Level] DESC 
        FOR JSON PATH
    ) AS [FullPath];

END;