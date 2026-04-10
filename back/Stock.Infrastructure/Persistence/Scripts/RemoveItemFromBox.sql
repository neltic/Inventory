CREATE OR ALTER PROCEDURE [dbo].[RemoveItemFromBox]
    @BoxId INT
    , @ItemId INT
    , @BrandId INT
AS
BEGIN

    DELETE FROM [Storage] 
    WHERE [BoxId] = @BoxId AND [ItemId] = @ItemId AND [BrandId] = @BrandId;

END