CREATE OR ALTER PROCEDURE [dbo].[GetStorageByItemId]
   @ItemId INT
AS 
BEGIN

    SELECT 
       b.[BoxId]
       , b.[Name]
       , ISNULL(s.[BrandId], 0) AS [BrandId]
       , b.[UpdatedAt]
       , ISNULL(s.[Quantity], 0) AS [Quantity]
       , ISNULL(s.[Expires], 0) AS [Expires]
       , s.[ExpiresOn]
       , s.[Notes]
    FROM 
       dbo.[Box] AS b
       INNER JOIN dbo.[Storage] AS s
          ON s.[BoxId] = b.[BoxId] AND s.[ItemId] = @ItemId
    ORDER BY
       [Quantity] DESC,
       b.[Name] ASC;
   
END