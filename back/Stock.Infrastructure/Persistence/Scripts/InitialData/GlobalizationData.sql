INSERT INTO [Language] (LanguageCode, [Name], IsDefault)
VALUES 
  ('en', 'English', 1)
, ('es-MX', 'Español (México)', 0);

INSERT INTO [Label] (Context, LabelKey)
VALUES 
  ('Box', 'NOT_FOUND')
, ('Box', 'CAN_NOT_UPDATE')
, ('Box', 'CAN_NOT_DELETE')
, ('Brand', 'NOT_FOUND')
, ('Brand', 'CAN_NOT_UPDATE')
, ('Brand', 'CAN_NOT_DELETE')
, ('Category', 'NOT_FOUND')
, ('Category', 'CAN_NOT_UPDATE')
, ('Category', 'CAN_NOT_DELETE')
, ('Item', 'NOT_FOUND')
, ('Item', 'CAN_NOT_UPDATE')
, ('Item', 'CAN_NOT_DELETE')
, ('Error', 'IMAGE_ASSIGN_BOX_FAILED')
, ('Error', 'IMAGE_PROCESS_BOX_ERROR')
, ('Error', 'IMAGE_ASSIGN_ITEM_FAILED')
, ('Error', 'IMAGE_PROCESS_ITEM_ERROR')
, ('Error', 'UNHANDLED_EXCEPTION')
, ('Error', 'INTERNAL_SERVER_ERROR')
, ('Storage', 'NO_ITEMS_IN_BOX')
, ('Storage', 'CAN_NOT_UPDATE');

INSERT INTO [Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'en', 
CASE 
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'Box with ID {id} was not found.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Box with ID {id}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Box with ID {id}.'
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'Brand with ID {id} was not found.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Brand with ID {id}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Brand with ID {id}.'
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'Category with ID {id} was not found.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Category with ID {id}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Category with ID {id}.'
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'Item with ID {id} was not found.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Item with ID {id}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Item with ID {id}.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_BOX_FAILED' THEN 'Failed to assign the image to the box.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_BOX_ERROR' THEN 'Internal error processing the image: {message}'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_ITEM_FAILED' THEN 'Failed to assign the image to the item.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ITEM_ERROR' THEN 'Internal error processing the image: {message}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'An unhandled exception occurred: {message}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'An internal error occurred on the server.'
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'There are no items in that box with ID {boxId}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update storage.'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];

INSERT INTO [Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'es-MX', 
CASE 
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la caja con el ID {id}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la caja con el ID {id}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la caja con el ID {id}.'
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la marca con el ID {id}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la marca con el ID {id}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la marca con el ID {id}.'
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la categoría con el ID {id}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la categoría con el ID {id}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la categoría con el ID {id}.'
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró el artículo con el ID {id}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el artículo con el ID {id}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar el artículo con el ID {id}.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_BOX_FAILED' THEN 'Error al asignar la imagen a la caja.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_BOX_ERROR' THEN 'Error interno al procesar la imagen: {message}'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_ITEM_FAILED' THEN 'Error al asignar la imagen al artículo.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ITEM_ERROR' THEN 'Error interno al procesar la imagen: {message}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'Ocurrió una excepción no controlada: {message}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'Ocurrió un error interno en el servidor.'
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'No hay artículos en la caja con el ID {boxId}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el almacenamiento.'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];
