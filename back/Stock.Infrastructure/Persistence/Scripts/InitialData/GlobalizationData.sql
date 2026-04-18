INSERT INTO [Language] (LanguageCode, [Name], IsDefault)
VALUES 
  ('en', 'English', 1)
, ('es-MX', 'Español (México)', 0);

/* BACKEND */

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

/* FRONTEND */

-- Menu

INSERT INTO [Label] (Context, LabelKey)
VALUES 
  ('Menu', 'BOX')
, ('Menu', 'SEARCH')
, ('Menu', 'ITEM')
, ('Menu', 'NEW')
, ('Menu', 'STORAGE')
, ('Menu', 'CATEGORY')
, ('Menu', 'SETUP')
, ('Menu', 'BRAND')
, ('Menu', 'LANG_EN')
, ('Menu', 'LANG_ES_MX');

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'en', 
CASE 
    WHEN Context = 'Menu' AND LabelKey = 'BOX' THEN 'Boxes'
    WHEN Context = 'Menu' AND LabelKey = 'SEARCH' THEN 'Search'
    WHEN Context = 'Menu' AND LabelKey = 'ITEM' THEN 'Items'
    WHEN Context = 'Menu' AND LabelKey = 'NEW' THEN 'New'
    WHEN Context = 'Menu' AND LabelKey = 'STORAGE' THEN 'Storage'
    WHEN Context = 'Menu' AND LabelKey = 'CATEGORY' THEN 'Categories'
    WHEN Context = 'Menu' AND LabelKey = 'SETUP' THEN 'Setup'
    WHEN Context = 'Menu' AND LabelKey = 'BRAND' THEN 'Brands'
    WHEN Context = 'Menu' AND LabelKey = 'LANG_EN' THEN 'English'
    WHEN Context = 'Menu' AND LabelKey = 'LANG_ES_MX' THEN 'Spanish (Mexico)'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label]
WHERE Context = 'Menu';

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'es-MX', 
CASE 
    WHEN Context = 'Menu' AND LabelKey = 'BOX' THEN 'Cajas'
    WHEN Context = 'Menu' AND LabelKey = 'SEARCH' THEN 'Buscar'
    WHEN Context = 'Menu' AND LabelKey = 'ITEM' THEN 'Artículos'
    WHEN Context = 'Menu' AND LabelKey = 'NEW' THEN 'Nuevo'
    WHEN Context = 'Menu' AND LabelKey = 'STORAGE' THEN 'Almacenamiento'
    WHEN Context = 'Menu' AND LabelKey = 'CATEGORY' THEN 'Categorías'
    WHEN Context = 'Menu' AND LabelKey = 'SETUP' THEN 'Configuración'
    WHEN Context = 'Menu' AND LabelKey = 'BRAND' THEN 'Marcas'
    WHEN Context = 'Menu' AND LabelKey = 'LANG_EN' THEN 'Inglés'
    WHEN Context = 'Menu' AND LabelKey = 'LANG_ES_MX' THEN 'Español (México)'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label]
WHERE Context = 'Menu';

-- Box & General

INSERT INTO [Label] (Context, LabelKey)
VALUES 
  ('Box', 'NO_ITEMS_FOUND')
, ('Box', 'FILTER_BY_NAME')
, ('Box', 'NO_BOXES_WITH_DESCRIPTION')
, ('Box', 'NO_SUB_BOXES')
, ('Box', 'CREATE_NEW_INFO')
, ('Box', 'VOLUME_FORMULA')
, ('Box', 'MOVE_TO')
, ('Box', 'BOX_NAME')
, ('Box', 'BOX_BRAND')
, ('Box', 'BOX_CATEGORY')
, ('Box', 'NOTES')
, ('Box', 'BOX_NOTES')
, ('Box', 'WIDTH')
, ('Box', 'BOX_WIDTH')
, ('Box', 'HEIGHT')
, ('Box', 'BOX_HEIGHT')
, ('Box', 'DEPTH')
, ('Box', 'BOX_DEPTH')
, ('Box', 'MOVE_CONFIRMATION_TITLE')
, ('Box', 'MOVE_CONFIRMATION_MSG');

INSERT INTO [Label] (Context, LabelKey)
VALUES 
  ('Global', 'CREATED')
, ('Global', 'UPDATED')
, ('Global', 'DELETE')
, ('Global', 'DELETING')
, ('Global', 'BACK')
, ('Global', 'CHANGE_PHOTO')
, ('Global', 'UPLOAD_PHOTO')
, ('Global', 'FILL_FORM_PHOTO')
, ('Global', 'SAVING')
, ('Global', 'UPLOADING_IMAGE')
, ('Global', 'SAVE')     
, ('Global', 'CANCEL')
, ('Global', 'MOVE')
, ('Global', 'EDIT')
, ('Global', 'DETAILS')
, ('Global', 'OPEN');

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'en', 
CASE 
    -- Box
    WHEN Context = 'Box' AND LabelKey = 'NO_ITEMS_FOUND' THEN 'There are no items in this box.'
    WHEN Context = 'Box' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filter by box name'
    WHEN Context = 'Box' AND LabelKey = 'NO_BOXES_WITH_DESCRIPTION' THEN 'There are no boxes with that description: {0}'
    WHEN Context = 'Box' AND LabelKey = 'NO_SUB_BOXES' THEN 'This box contains no other boxes.'
    WHEN Context = 'Box' AND LabelKey = 'CREATE_NEW_INFO' THEN 'You can create a new box within this one from the menu:'
    WHEN Context = 'Box' AND LabelKey = 'VOLUME_FORMULA' THEN '(width x height x depth = volume)'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_TO' THEN 'Move To'
    WHEN Context = 'Box' AND LabelKey = 'BOX_NAME' THEN 'Box name'
    WHEN Context = 'Box' AND LabelKey = 'BOX_BRAND' THEN 'Box brand'
    WHEN Context = 'Box' AND LabelKey = 'BOX_CATEGORY' THEN 'Box category'
    WHEN Context = 'Box' AND LabelKey = 'NOTES' THEN 'Notes'
    WHEN Context = 'Box' AND LabelKey = 'BOX_NOTES' THEN 'Box notes'
    WHEN Context = 'Box' AND LabelKey = 'WIDTH' THEN 'Width'
    WHEN Context = 'Box' AND LabelKey = 'BOX_WIDTH' THEN 'Box width'
    WHEN Context = 'Box' AND LabelKey = 'HEIGHT' THEN 'Height'
    WHEN Context = 'Box' AND LabelKey = 'BOX_HEIGHT' THEN 'Box height'
    WHEN Context = 'Box' AND LabelKey = 'DEPTH' THEN 'Depth'
    WHEN Context = 'Box' AND LabelKey = 'BOX_DEPTH' THEN 'Box depth'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_TITLE' THEN 'Move {0} To...'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_MSG' THEN 'I understand that moving {0} will also move all its internal items and sub-boxes.'
    
    -- Global
    WHEN Context = 'Global' AND LabelKey = 'CREATED' THEN 'Created'
    WHEN Context = 'Global' AND LabelKey = 'UPDATED' THEN 'Updated'
    WHEN Context = 'Global' AND LabelKey = 'DELETE' THEN 'Delete'
    WHEN Context = 'Global' AND LabelKey = 'DELETING' THEN 'Deleting...'
    WHEN Context = 'Global' AND LabelKey = 'BACK' THEN 'Back'
    WHEN Context = 'Global' AND LabelKey = 'CHANGE_PHOTO' THEN 'Change Photo'
    WHEN Context = 'Global' AND LabelKey = 'UPLOAD_PHOTO' THEN 'Upload Photo'
    WHEN Context = 'Global' AND LabelKey = 'FILL_FORM_PHOTO' THEN 'Please fill out the form, then you can upload the photo'
    WHEN Context = 'Global' AND LabelKey = 'SAVING' THEN 'Saving...'
    WHEN Context = 'Global' AND LabelKey = 'UPLOADING_IMAGE' THEN 'Uploading image...'
    WHEN Context = 'Global' AND LabelKey = 'SAVE' THEN 'Save'
    WHEN Context = 'Global' AND LabelKey = 'CANCEL' THEN 'Cancel'
    WHEN Context = 'Global' AND LabelKey = 'MOVE' THEN 'Move'
    WHEN Context = 'Global' AND LabelKey = 'EDIT' THEN 'Edit'
    WHEN Context = 'Global' AND LabelKey = 'DETAILS' THEN 'Details'
    WHEN Context = 'Global' AND LabelKey = 'OPEN' THEN 'Open'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label]
WHERE 
  (Context = 'Box' AND LabelKey IN ('NO_ITEMS_FOUND', 'FILTER_BY_NAME', 'NO_BOXES_WITH_DESCRIPTION', 'NO_SUB_BOXES', 'CREATE_NEW_INFO', 'VOLUME_FORMULA', 'MOVE_TO', 'BOX_NAME', 'BOX_BRAND', 'BOX_CATEGORY', 'NOTES', 'BOX_NOTES', 'WIDTH', 'BOX_WIDTH', 'HEIGHT', 'BOX_HEIGHT', 'DEPTH', 'BOX_DEPTH', 'MOVE_CONFIRMATION_TITLE', 'MOVE_CONFIRMATION_MSG'))
  OR 
  (Context = 'Global' AND LabelKey IN ('CREATED', 'UPDATED', 'DELETE', 'DELETING', 'BACK', 'CHANGE_PHOTO', 'UPLOAD_PHOTO', 'FILL_FORM_PHOTO', 'SAVING', 'UPLOADING_IMAGE', 'SAVE', 'CANCEL', 'MOVE', 'EDIT', 'DETAILS', 'OPEN'));

INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'es-MX', 
CASE 
    -- Box
    WHEN Context = 'Box' AND LabelKey = 'NO_ITEMS_FOUND' THEN 'No hay artículos en esta caja.'
    WHEN Context = 'Box' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filtrar por nombre de caja'
    WHEN Context = 'Box' AND LabelKey = 'NO_BOXES_WITH_DESCRIPTION' THEN 'No hay cajas con esa descripción: {0}'
    WHEN Context = 'Box' AND LabelKey = 'NO_SUB_BOXES' THEN 'Esta caja no contiene otras cajas.'
    WHEN Context = 'Box' AND LabelKey = 'CREATE_NEW_INFO' THEN 'Puedes crear una nueva caja dentro de esta desde el menú:'
    WHEN Context = 'Box' AND LabelKey = 'VOLUME_FORMULA' THEN '(ancho x alto x profundidad = volumen)'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_TO' THEN 'Mover a'
    WHEN Context = 'Box' AND LabelKey = 'BOX_NAME' THEN 'Nombre de la caja'
    WHEN Context = 'Box' AND LabelKey = 'BOX_BRAND' THEN 'Marca de la caja'
    WHEN Context = 'Box' AND LabelKey = 'BOX_CATEGORY' THEN 'Categoría de la caja'
    WHEN Context = 'Box' AND LabelKey = 'NOTES' THEN 'Notas'
    WHEN Context = 'Box' AND LabelKey = 'BOX_NOTES' THEN 'Notas de la caja'
    WHEN Context = 'Box' AND LabelKey = 'WIDTH' THEN 'Ancho'
    WHEN Context = 'Box' AND LabelKey = 'BOX_WIDTH' THEN 'Ancho de la caja'
    WHEN Context = 'Box' AND LabelKey = 'HEIGHT' THEN 'Alto'
    WHEN Context = 'Box' AND LabelKey = 'BOX_HEIGHT' THEN 'Alto de la caja'
    WHEN Context = 'Box' AND LabelKey = 'DEPTH' THEN 'Profundidad'
    WHEN Context = 'Box' AND LabelKey = 'BOX_DEPTH' THEN 'Profundidad de la caja'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_TITLE' THEN 'Mover {0} a...'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_MSG' THEN 'Entiendo que mover {0} también moverá todos sus artículos internos y sub-cajas.'
    
    -- Global
    WHEN Context = 'Global' AND LabelKey = 'CREATED' THEN 'Creado'
    WHEN Context = 'Global' AND LabelKey = 'UPDATED' THEN 'Actualizado'
    WHEN Context = 'Global' AND LabelKey = 'DELETE' THEN 'Eliminar'
    WHEN Context = 'Global' AND LabelKey = 'DELETING' THEN 'Eliminando...'
    WHEN Context = 'Global' AND LabelKey = 'BACK' THEN 'Volver'
    WHEN Context = 'Global' AND LabelKey = 'CHANGE_PHOTO' THEN 'Cambiar Foto'
    WHEN Context = 'Global' AND LabelKey = 'UPLOAD_PHOTO' THEN 'Subir Foto'
    WHEN Context = 'Global' AND LabelKey = 'FILL_FORM_PHOTO' THEN 'Por favor complete el formulario, luego podrá subir la foto'
    WHEN Context = 'Global' AND LabelKey = 'SAVING' THEN 'Guardando...'
    WHEN Context = 'Global' AND LabelKey = 'UPLOADING_IMAGE' THEN 'Subiendo imagen...'
    WHEN Context = 'Global' AND LabelKey = 'SAVE' THEN 'Guardar'
    WHEN Context = 'Global' AND LabelKey = 'CANCEL' THEN 'Cancelar'
    WHEN Context = 'Global' AND LabelKey = 'MOVE' THEN 'Mover'
    WHEN Context = 'Global' AND LabelKey = 'EDIT' THEN 'Editar'
    WHEN Context = 'Global' AND LabelKey = 'DETAILS' THEN 'Detalles'
    WHEN Context = 'Global' AND LabelKey = 'OPEN' THEN 'Abrir'
END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label]
WHERE 
  (Context = 'Box' AND LabelKey IN ('NO_ITEMS_FOUND', 'FILTER_BY_NAME', 'NO_BOXES_WITH_DESCRIPTION', 'NO_SUB_BOXES', 'CREATE_NEW_INFO', 'VOLUME_FORMULA', 'MOVE_TO', 'BOX_NAME', 'BOX_BRAND', 'BOX_CATEGORY', 'NOTES', 'BOX_NOTES', 'WIDTH', 'BOX_WIDTH', 'HEIGHT', 'BOX_HEIGHT', 'DEPTH', 'BOX_DEPTH', 'MOVE_CONFIRMATION_TITLE', 'MOVE_CONFIRMATION_MSG'))
  OR 
  (Context = 'Global' AND LabelKey IN ('CREATED', 'UPDATED', 'DELETE', 'DELETING', 'BACK', 'CHANGE_PHOTO', 'UPLOAD_PHOTO', 'FILL_FORM_PHOTO', 'SAVING', 'UPLOADING_IMAGE', 'SAVE', 'CANCEL', 'MOVE', 'EDIT', 'DETAILS', 'OPEN'));
  
