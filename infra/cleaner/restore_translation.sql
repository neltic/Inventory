/*

DELETE [dbo].[Translation]

DELETE [dbo].[Label] 

DBCC CHECKIDENT ('Label', RESEED, 0);
GO

DBCC CHECKIDENT ('Translation', RESEED, 0);
GO

*/

SELECT 
    'export type GlobalizationKey = ' + 
    STRING_AGG(CAST('''' + Context + '.' + LabelKey + '''' AS VARCHAR(MAX)), ' | ') + ';' 
    AS TypeScriptType
FROM Label;