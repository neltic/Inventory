CREATE OR ALTER PROCEDURE [dbo].[GetBoxFullPathByParent]
    @ParentBoxId INT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH BoxPath AS (
        SELECT 
            BoxId,
            ParentBoxId,
            [Name],
            1 AS [Level]
        FROM [dbo].[Box]
        WHERE BoxId = @ParentBoxId

        UNION ALL
        
        SELECT 
            b.BoxId,
            b.ParentBoxId,
            b.[Name],
            bp.[Level] + 1
        FROM [dbo].[Box] b
        INNER JOIN BoxPath bp ON b.BoxId = bp.ParentBoxId
    )
    SELECT (
        SELECT 
            BoxId as boxId
            , [Name] as [name]
        FROM BoxPath
        ORDER BY [Level] DESC
        FOR JSON PATH
    ) AS FullPath;

END;