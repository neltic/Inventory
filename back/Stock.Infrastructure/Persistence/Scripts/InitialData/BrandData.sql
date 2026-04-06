SET IDENTITY_INSERT [dbo].[Brand] ON;

INSERT INTO [dbo].[Brand] 
    ([BrandId], [Name], [Color], [Background], [IncludedIn])
VALUES 
    (0, 'Generic',   '#FFFFFF', '#757575', 3),
    (1, 'Apple',     '#FFFFFF', '#000000', 1),
    (2, 'Logitech',  '#000000', '#18FCD2', 1),
    (3, 'UGREEN',    '#FFFFFF', '#20824E', 2),
    (4, 'Casio',     '#0A3282', '#FFFFFF', 1),
    (5, 'DEWALT',    '#000000', '#FDBC18', 2),
    (6, 'Truper',    '#000000', '#FF6720', 2),
    (7, 'Staedtler', '#FFFFFF', '#00448C', 1),
    (8, 'Amazon',    '#FFFFFF', '#FF6200', 3);    

    
SET IDENTITY_INSERT [dbo].[Brand] OFF;