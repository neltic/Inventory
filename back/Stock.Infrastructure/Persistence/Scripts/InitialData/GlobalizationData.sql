INSERT INTO [Language] (LanguageCode, [Name], IsDefault)
VALUES 
  ('en', 'English', 1)
, ('es-MX', 'Español (México)', 0);

INSERT INTO [Label] (Context, LabelKey)
VALUES 
  ('Box', 'NOT_FOUND')
, ('Box', 'CAN_NOT_UPDATE')
, ('Box', 'CAN_NOT_DELETE')
, ('Box', 'ALREADY_EXISTS')
, ('Brand', 'NOT_FOUND')
, ('Brand', 'CAN_NOT_UPDATE')
, ('Brand', 'CAN_NOT_DELETE')
, ('Brand', 'ALREADY_EXISTS')
, ('Category', 'NOT_FOUND')
, ('Category', 'CAN_NOT_UPDATE')
, ('Category', 'CAN_NOT_DELETE')
, ('Category', 'ALREADY_EXISTS')
, ('Item', 'NOT_FOUND')
, ('Item', 'CAN_NOT_UPDATE')
, ('Item', 'CAN_NOT_DELETE')
, ('Item', 'ALREADY_EXISTS')
, ('Error', 'IMAGE_ASSIGN_FAILED')
, ('Error', 'IMAGE_PROCESS_ERROR')
, ('Error', 'UNHANDLED_EXCEPTION')
, ('Error', 'INTERNAL_SERVER_ERROR')
, ('Storage', 'NO_ITEMS_IN_BOX')
, ('Storage', 'CAN_NOT_UPDATE');

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'en', 
CASE 
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'Box with ID {0} was not found.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Box with ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Box with ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'ALREADY_EXISTS' THEN 'A box with this name already exists in this location.'
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'Brand with ID {0} was not found.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Brand with ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Brand with ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'ALREADY_EXISTS' THEN 'A Brand with this name already exists.'
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'Category with ID {0} was not found.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Category with ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Category with ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'ALREADY_EXISTS' THEN 'A category with this name already exists.'
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'Item with ID {0} was not found.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Item with ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Item with ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'ALREADY_EXISTS' THEN 'An item with this name already exists.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_FAILED' THEN 'Failed to assign the image.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ERROR' THEN 'Internal error processing the image: {0}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'An unhandled exception occurred: {0}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'An internal error occurred on the server.'
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'There are no items in that box with ID {0}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update storage.'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'es-MX', 
CASE 
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una caja con este nombre en esta ubicación.'
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una marca con este nombre.'
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una categoría con este nombre.'
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe un artículo con este nombre.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_FAILED' THEN 'Error al asignar la imagen.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ERROR' THEN 'Error interno al procesar la imagen: {0}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'Ocurrió una excepción no controlada: {0}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'Ocurrió un error interno en el servidor.'
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'No hay artículos en la caja con el ID {0}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el almacenamiento.'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];