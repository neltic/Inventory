CREATE OR ALTER PROCEDURE [dbo].[GetAllTranslations]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [t].[LanguageCode]
        ,  [l].[Context]
        , [l].[LabelKey]
        , [t].[Text]
    FROM 
        [Translation] AS [t]
        INNER JOIN [Label] AS [l] 
            ON [t].[LabelId] = [l].[LabelId];

END
