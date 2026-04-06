SET IDENTITY_INSERT [dbo].[Category] ON;

INSERT INTO [dbo].[Category] 
    ([CategoryId], [Name], [Icon], [Color], [Order], IncludedIn)
VALUES 
    (1, 'Accessories', 'headphones',            '#A807ED', 3, 3),
    (2, 'Batteries',   'battery_charging_full', '#3BFB2D', 6, 3),
    (3, 'Documents',   'description',           '#618494', 4, 3),
    (4, 'Electronics', 'devices',               '#4691D8', 2, 3),
    (5, 'Office',      'push_pin',              '#D1F5EE', 5, 3),
    (6, 'Safe Box',    'security',              '#0599A3', 8, 2),
    (7, 'Tools',       'home_repair_service',   '#FDBC18', 1, 2),
    (8, 'Toys',        'toys',                  '#EE7272', 7, 1);
    
SET IDENTITY_INSERT [dbo].[Category] OFF;