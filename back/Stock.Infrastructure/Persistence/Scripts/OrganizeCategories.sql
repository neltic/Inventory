CREATE OR ALTER PROCEDURE [dbo].[OrganizeCategories]
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH SortedCategories AS (
        SELECT [Order], ROW_NUMBER() OVER (ORDER BY [Order], [Name] ASC) as NewOrder
        FROM [dbo].Category
    )
    UPDATE SortedCategories
    SET [Order] = NewOrder;

END