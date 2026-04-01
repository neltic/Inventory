CREATE OR ALTER PROCEDURE [dbo].[GetAvailableParentBoxes]
    @TargetBoxId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;    
    
    DECLARE @Forbidden TABLE (BoxId INT PRIMARY KEY);

    IF @TargetBoxId IS NOT NULL
    BEGIN
        ;WITH [Descendants] AS (
            SELECT BoxId FROM [dbo].[Box] WHERE BoxId = @TargetBoxId
            UNION ALL
            SELECT b.BoxId FROM [dbo].[Box] b
            INNER JOIN [Descendants] d ON b.ParentBoxId = d.BoxId
        )
        INSERT INTO @Forbidden SELECT BoxId FROM [Descendants];
    END

    -- 2. Identificar el padre actual
    DECLARE @CurrentParentId INT;
    SELECT @CurrentParentId = ParentBoxId FROM [dbo].[Box] WHERE BoxId = @TargetBoxId;

    -- 3. Generar jerarquía con la opción ROOT
    ;WITH [BoxHierarchy] AS (
        -- REGISTRO VIRTUAL: La Raíz del Sistema
        SELECT 
            NULL AS [BoxId]
            , NULL AS [ParentBoxId]
            , '[root]' AS [Name]
            , GETUTCDATE() AS [UpdatedAt]
            , '0' AS [SortPath]
            , 0 AS [Indent]
            , CAST(CASE WHEN @CurrentParentId IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS [IsSelectable]

        UNION ALL

        -- Raíces Reales (Cajas nivel 0)
        SELECT 
            b.[BoxId]
            , b.[ParentBoxId]
            , b.[Name]
            , b.[UpdatedAt]
            , CAST('1.' + CAST(ROW_NUMBER() OVER (ORDER BY b.[Name]) AS VARCHAR(MAX)) AS VARCHAR(MAX))
            , 0 AS [Indent]
            , CAST(CASE WHEN b.[BoxId] <> ISNULL(@CurrentParentId, -1) THEN 1 ELSE 0 END AS BIT)
        FROM 
            [dbo].[Box] b
        WHERE 
            b.[ParentBoxId] IS NULL 
            AND NOT EXISTS (SELECT 1 FROM @Forbidden f WHERE f.BoxId = b.BoxId)

        UNION ALL
        
        -- Hijos recursivos
        SELECT 
            b.[BoxId]
            , b.[ParentBoxId]
            , b.[Name]
            , b.[UpdatedAt]
            , CAST(bh.[SortPath] + '.' + CAST(ROW_NUMBER() OVER (PARTITION BY b.[ParentBoxId] ORDER BY b.[Name]) AS VARCHAR(10)) AS VARCHAR(MAX))
            , bh.[Indent] + 1 AS [Indent]
            , CAST(CASE WHEN b.[BoxId] <> ISNULL(@CurrentParentId, -1) THEN 1 ELSE 0 END AS BIT)
        FROM 
            [dbo].[Box] b
            INNER JOIN [BoxHierarchy] bh 
                ON b.[ParentBoxId] = bh.[BoxId]
        WHERE 
            bh.[BoxId] IS NOT NULL 
            AND NOT EXISTS (SELECT 1 FROM @Forbidden f WHERE f.BoxId = b.BoxId)
    )
    SELECT 
        [BoxId]
        , [Name]
        , [UpdatedAt]
        , [Indent]
        , [IsSelectable]
    FROM 
        [BoxHierarchy]
    ORDER BY 
        [SortPath];
        
END