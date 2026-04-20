INSERT INTO [Label] (Context, LabelKey)
VALUES 
  -- Box
  ('Box', 'NOT_FOUND'),
  ('Box', 'CAN_NOT_UPDATE'),
  ('Box', 'CAN_NOT_DELETE'),
  ('Box', 'ALREADY_EXISTS'),
  ('Box', 'NO_ITEMS_FOUND'),
  ('Box', 'FILTER_BY_NAME'),
  ('Box', 'NO_BOXES_WITH_DESCRIPTION'),
  ('Box', 'NO_SUB_BOXES'),
  ('Box', 'CREATE_NEW_INFO'),
  ('Box', 'VOLUME_FORMULA'),
  ('Box', 'MOVE_TO'),
  ('Box', 'BOX_NAME'),
  ('Box', 'BOX_BRAND'),
  ('Box', 'BOX_CATEGORY'),
  ('Box', 'NOTES'),
  ('Box', 'BOX_NOTES'),
  ('Box', 'WIDTH'),
  ('Box', 'BOX_WIDTH'),
  ('Box', 'HEIGHT'),
  ('Box', 'BOX_HEIGHT'),
  ('Box', 'DEPTH'),
  ('Box', 'BOX_DEPTH'),
  ('Box', 'MOVE_CONFIRMATION_TITLE'),
  ('Box', 'MOVE_CONFIRMATION_MSG'),
  ('Box', 'NAME'),
  ('Box', 'BRAND'),
  ('Box', 'CATEGORY'),

  -- Item
  ('Item', 'NOT_FOUND'),
  ('Item', 'CAN_NOT_UPDATE'),
  ('Item', 'CAN_NOT_DELETE'),
  ('Item', 'ALREADY_EXISTS'),
  ('Item', 'FILTER_BY_NAME'),
  ('Item', 'FILTER_BY_CATEGORY'),
  ('Item', 'NO_ITEMS_WITH_DESCRIPTION_OR_CATEGORY'),
  ('Item', 'CREATE_NEW_INFO'),
  ('Item', 'NAME'),
  ('Item', 'ITEM_NAME'),
  ('Item', 'NOTES'),
  ('Item', 'ITEM_NOTES'),
  ('Item', 'CATEGORY'),
  ('Item', 'ITEM_CATEGORY'),
  ('Item', 'BRAND'),
  ('Item', 'ITEM_BRAND'),

  -- Brand
  ('Brand', 'NOT_FOUND'),
  ('Brand', 'CAN_NOT_UPDATE'),
  ('Brand', 'CAN_NOT_DELETE'),
  ('Brand', 'ALREADY_EXISTS'),
  ('Brand', 'FILTER_BY_NAME'),
  ('Brand', 'NAME'),
  ('Brand', 'BRAND_NAME'),
  ('Brand', 'COLOR'),
  ('Brand', 'BRAND_COLOR'),
  ('Brand', 'BACKGROUND'),
  ('Brand', 'BRAND_BACKGROUND'),
  ('Brand', 'SCOPE'),
  ('Brand', 'BRAND_SCOPE'),
  ('Brand', 'NO_BRANDS_FOUND'),

  -- Category
  ('Category', 'NOT_FOUND'),
  ('Category', 'CAN_NOT_UPDATE'),
  ('Category', 'CAN_NOT_DELETE'),
  ('Category', 'ALREADY_EXISTS'),
  ('Category', 'FILTER_BY_NAME'),
  ('Category', 'NAME'),
  ('Category', 'CATEGORY_NAME'),
  ('Category', 'ICON'),
  ('Category', 'CATEGORY_ICON'),
  ('Category', 'COLOR'),
  ('Category', 'CATEGORY_COLOR'),
  ('Category', 'SCOPE'),
  ('Category', 'CATEGORY_SCOPE'),
  ('Category', 'ORDER'),
  ('Category', 'NO_CATEGORIES_FOUND'),

  -- Global
  ('Global', 'CREATED'),
  ('Global', 'UPDATED'),
  ('Global', 'DELETE'),
  ('Global', 'DELETING'),
  ('Global', 'BACK'),
  ('Global', 'CHANGE_PHOTO'),
  ('Global', 'UPLOAD_PHOTO'),
  ('Global', 'FILL_FORM_PHOTO'),
  ('Global', 'SAVING'),
  ('Global', 'UPLOADING_IMAGE'),
  ('Global', 'SAVE'),
  ('Global', 'CANCEL'),
  ('Global', 'MOVE'),
  ('Global', 'EDIT'),
  ('Global', 'NEW'),
  ('Global', 'DETAILS'),
  ('Global', 'OPEN'),
  ('Global', 'UNREGISTERED'),
  ('Global', 'ALL'),
  ('Global', 'NEXT'),
  ('Global', 'CONFIRM_SAVE'),
  ('Global', 'OK'),
  ('Global', 'YES'),
  ('Global', 'NO'),
  ('Global', 'NOT_SELECTED'),
  ('Global', 'STOCK'),

  -- Menu
  ('Menu', 'BOX'),
  ('Menu', 'SEARCH'),
  ('Menu', 'ITEM'),
  ('Menu', 'NEW'),
  ('Menu', 'STORAGE'),
  ('Menu', 'CATEGORY'),
  ('Menu', 'SETUP'),
  ('Menu', 'BRAND'),
  ('Menu', 'LANG_EN'),
  ('Menu', 'LANG_ES_MX'),

  -- Storage
  ('Storage', 'NO_ITEMS_IN_BOX'),
  ('Storage', 'CAN_NOT_UPDATE'),
  ('Storage', 'SELECT_BOX'),
  ('Storage', 'SELECT_ITEM'),
  ('Storage', 'QUANTITY'),
  ('Storage', 'QUANTITY_ABBREVIATION'),
  ('Storage', 'STORAGE_QUANTITY'),
  ('Storage', 'EXPIRES'),
  ('Storage', 'EXPIRES_ON'),
  ('Storage', 'EXPIRATION_DATE'),
  ('Storage', 'REVIEW_CONFIRM'),
  ('Storage', 'FILL_DETAILS'),
  ('Storage', 'SUMMARY'),

  -- Message
  ('Message', 'CONFIRM_DELETE_BOX'),
  ('Message', 'BOX_DELETED'),
  ('Message', 'WAIT_IMAGE_UPLOAD'),
  ('Message', 'BOX_SAVED'),
  ('Message', 'IMAGE_UPDATED'),
  ('Message', 'IMAGE_READY_TO_SAVE'),
  ('Message', 'CONFIRM_DELETE_BRAND'),
  ('Message', 'BRAND_DELETED'),
  ('Message', 'CONFIRM_DELETE_CATEGORY'),
  ('Message', 'CATEGORY_DELETED'),
  ('Message', 'MOVED_TO_POSITION'),
  ('Message', 'CONFIRM_DELETE_ITEM'),
  ('Message', 'ITEM_DELETED'),
  ('Message', 'CONFIRM_REMOVE_ITEM_FROM_BOX'),
  ('Message', 'ITEM_REMOVED_FROM_BOX'),
  ('Message', 'ITEM_SAVED'),
  ('Message', 'STORAGE_UPDATED'),
  ('Message', 'PENDING_CHANGES'),

  -- Error
  ('Error', 'IMAGE_ASSIGN_FAILED'),
  ('Error', 'IMAGE_PROCESS_ERROR'),
  ('Error', 'UNHANDLED_EXCEPTION'),
  ('Error', 'INTERNAL_SERVER_ERROR'),
  ('Error', 'REQUIRED'),
  ('Error', 'MIN_LENGTH'),
  ('Error', 'MAX_LENGTH'),
  ('Error', 'MIN'),
  ('Error', 'BRAND_NOT_FOUND'),
  ('Error', 'CATEGORY_NOT_FOUND'),
  ('Error', 'ENTITY_NOT_FOUND'),
  ('Error', 'INVALID_DESTINATION'),
  ('Error', 'REQUIRED_TRUE'),
  ('Error', 'IMAGE_PROCESSING'),
  ('Error', 'EXECUTION_ERROR'),
  ('Error', 'REMOVE_RELATIONSHIP_ERROR'),
  ('Error', 'STORAGE_UPDATE_ERROR'),

  -- Scope
  ('Scope', 'ALL'),
  ('Scope', 'ITEM'),
  ('Scope', 'BOX'),
  ('Scope', 'NONE'),

  -- Welcome
  ('Welcome', 'TITLE'),
  ('Welcome', 'SUBTITLE'),
  ('Welcome', 'STORAGE_CARD_TITLE'),
  ('Welcome', 'STORAGE_CARD_SUBTITLE'),
  ('Welcome', 'STORAGE_CARD_CONTENT'),
  ('Welcome', 'BOX_CARD_TITLE'),
  ('Welcome', 'BOX_CARD_SUBTITLE'),
  ('Welcome', 'BOX_CARD_CONTENT'),
  ('Welcome', 'ITEM_CARD_TITLE'),
  ('Welcome', 'ITEM_CARD_SUBTITLE'),
  ('Welcome', 'ITEM_CARD_CONTENT'),
  ('Welcome', 'CATEGORY_CARD_TITLE'),
  ('Welcome', 'CATEGORY_CARD_SUBTITLE'),
  ('Welcome', 'CATEGORY_CARD_CONTENT'),
  ('Welcome', 'BRAND_CARD_TITLE'),
  ('Welcome', 'BRAND_CARD_SUBTITLE'),
  ('Welcome', 'BRAND_CARD_CONTENT');


INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'en', 
CASE 
    -- Box
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'Box with ID {0} was not found.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Box with ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Box with ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'ALREADY_EXISTS' THEN 'A box with this name already exists in this location.'
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
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_TITLE' THEN 'Move "{0}" To...'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_MSG' THEN 'I understand that moving "{0}" will also move all its internal items and sub-boxes.'
    WHEN Context = 'Box' AND LabelKey = 'NAME' THEN 'Name'
    WHEN Context = 'Box' AND LabelKey = 'BRAND' THEN 'Brand'
    WHEN Context = 'Box' AND LabelKey = 'CATEGORY' THEN 'Category'

    -- Item
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'Item with ID {0} was not found.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Item with ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Item with ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'ALREADY_EXISTS' THEN 'An item with this name already exists.'
    WHEN Context = 'Item' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filter by item name'
    WHEN Context = 'Item' AND LabelKey = 'FILTER_BY_CATEGORY' THEN 'Filter by item category'
    WHEN Context = 'Item' AND LabelKey = 'NO_ITEMS_WITH_DESCRIPTION_OR_CATEGORY' THEN 'There are no items with that description or category.'
    WHEN Context = 'Item' AND LabelKey = 'CREATE_NEW_INFO' THEN 'You can create a new item from the menu:'
    WHEN Context = 'Item' AND LabelKey = 'NAME' THEN 'Name'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_NAME' THEN 'Item name'
    WHEN Context = 'Item' AND LabelKey = 'NOTES' THEN 'Notes'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_NOTES' THEN 'Item notes'
    WHEN Context = 'Item' AND LabelKey = 'CATEGORY' THEN 'Category'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_CATEGORY' THEN 'Item category'
    WHEN Context = 'Item' AND LabelKey = 'BRAND' THEN 'Brand'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_BRAND' THEN 'Item brand'

    -- Brand
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'Brand with ID {0} was not found.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Brand with ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Brand with ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'ALREADY_EXISTS' THEN 'A Brand with this name already exists.'
    WHEN Context = 'Brand' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filter by brand name'
    WHEN Context = 'Brand' AND LabelKey = 'NAME' THEN 'Name'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_NAME' THEN 'Brand name'
    WHEN Context = 'Brand' AND LabelKey = 'COLOR' THEN 'Color'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_COLOR' THEN 'Brand color'
    WHEN Context = 'Brand' AND LabelKey = 'BACKGROUND' THEN 'Background'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_BACKGROUND' THEN 'Brand background'
    WHEN Context = 'Brand' AND LabelKey = 'SCOPE' THEN 'Scope'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_SCOPE' THEN 'Brand scope'
    WHEN Context = 'Brand' AND LabelKey = 'NO_BRANDS_FOUND' THEN 'No brands found matching your search.'

    -- Category
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'Category with ID {0} was not found.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update Category with ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'Can not delete Category with ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'ALREADY_EXISTS' THEN 'A category with this name already exists.'
    WHEN Context = 'Category' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filter by category name'
    WHEN Context = 'Category' AND LabelKey = 'NAME' THEN 'Name'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_NAME' THEN 'Category name'
    WHEN Context = 'Category' AND LabelKey = 'ICON' THEN 'Icon'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_ICON' THEN 'Category icon'
    WHEN Context = 'Category' AND LabelKey = 'COLOR' THEN 'Color'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_COLOR' THEN 'Category color'
    WHEN Context = 'Category' AND LabelKey = 'SCOPE' THEN 'Scope'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_SCOPE' THEN 'Category scope'
    WHEN Context = 'Category' AND LabelKey = 'ORDER' THEN 'Order'
    WHEN Context = 'Category' AND LabelKey = 'NO_CATEGORIES_FOUND' THEN 'No categories found matching your search.'

    -- Menu
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
    WHEN Context = 'Global' AND LabelKey = 'NEW' THEN 'New'
    WHEN Context = 'Global' AND LabelKey = 'DETAILS' THEN 'Details'
    WHEN Context = 'Global' AND LabelKey = 'OPEN' THEN 'Open'
    WHEN Context = 'Global' AND LabelKey = 'UNREGISTERED' THEN 'Unregistered'
    WHEN Context = 'Global' AND LabelKey = 'ALL' THEN 'All'
    WHEN Context = 'Global' AND LabelKey = 'NEXT' THEN 'Next'
    WHEN Context = 'Global' AND LabelKey = 'CONFIRM_SAVE' THEN 'Confirm & Save'
    WHEN Context = 'Global' AND LabelKey = 'YES' THEN 'Yes'
    WHEN Context = 'Global' AND LabelKey = 'NO' THEN 'No'
    WHEN Context = 'Global' AND LabelKey = 'OK' THEN 'Ok'
    WHEN Context = 'Global' AND LabelKey = 'NOT_SELECTED' THEN 'Not selected'
    WHEN Context = 'Global' AND LabelKey = 'STOCK' THEN 'Stock'

    -- Error
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_FAILED' THEN 'Failed to assign the image.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ERROR' THEN 'Internal error processing the image: {0}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'An unhandled exception occurred: {0}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'An internal error occurred on the server.'
    WHEN Context = 'Error' AND LabelKey = 'REQUIRED' THEN 'You must enter: {0}'
    WHEN Context = 'Error' AND LabelKey = 'MIN_LENGTH' THEN 'The {0} is too short'
    WHEN Context = 'Error' AND LabelKey = 'MAX_LENGTH' THEN 'The {0} is too long'
    WHEN Context = 'Error' AND LabelKey = 'MIN' THEN 'The value must be greater than 0'
    WHEN Context = 'Error' AND LabelKey = 'BRAND_NOT_FOUND' THEN 'The selected {0} is invalid'
    WHEN Context = 'Error' AND LabelKey = 'CATEGORY_NOT_FOUND' THEN 'The selected {0} is invalid'
    WHEN Context = 'Error' AND LabelKey = 'ENTITY_NOT_FOUND' THEN 'The selected {0} is invalid'
    WHEN Context = 'Error' AND LabelKey = 'INVALID_DESTINATION' THEN 'Please select a valid destination'
    WHEN Context = 'Error' AND LabelKey = 'REQUIRED_TRUE' THEN 'You must accept the change to continue'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESSING' THEN 'Image processing error'
    WHEN Context = 'Error' AND LabelKey = 'EXECUTION_ERROR' THEN 'Error executing action'
    WHEN Context = 'Error' AND LabelKey = 'REMOVE_RELATIONSHIP_ERROR' THEN 'An error occurred while trying to remove item relationship'
    WHEN Context = 'Error' AND LabelKey = 'STORAGE_UPDATE_ERROR' THEN 'Error updating storage'

    -- Storage
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'There are no items in that box with ID {0}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'Can not update storage.'
    WHEN Context = 'Storage' AND LabelKey = 'SELECT_BOX' THEN 'Select a Box'
    WHEN Context = 'Storage' AND LabelKey = 'SELECT_ITEM' THEN 'Select an Item'
    WHEN Context = 'Storage' AND LabelKey = 'QUANTITY' THEN 'Quantity'
    WHEN Context = 'Storage' AND LabelKey = 'QUANTITY_ABBREVIATION' THEN 'Qty'
    WHEN Context = 'Storage' AND LabelKey = 'STORAGE_QUANTITY' THEN 'Storage quantity'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRES' THEN 'Expires?'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRES_ON' THEN 'Expires on'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRATION_DATE' THEN 'Expiration Date'
    WHEN Context = 'Storage' AND LabelKey = 'REVIEW_CONFIRM' THEN 'Review & Confirm'
    WHEN Context = 'Storage' AND LabelKey = 'FILL_DETAILS' THEN 'Fill out details'
    WHEN Context = 'Storage' AND LabelKey = 'SUMMARY' THEN 'Summary'

    -- Scope
    WHEN Context = 'Scope' AND LabelKey = 'ALL' THEN 'All'
    WHEN Context = 'Scope' AND LabelKey = 'ITEM' THEN 'Item'
    WHEN Context = 'Scope' AND LabelKey = 'BOX' THEN 'Box'
    WHEN Context = 'Scope' AND LabelKey = 'NONE' THEN 'None'

    -- Message
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_BOX' THEN 'Are you sure you want to delete this box?'
    WHEN Context = 'Message' AND LabelKey = 'BOX_DELETED' THEN '¡Box deleted successfully!'
    WHEN Context = 'Message' AND LabelKey = 'WAIT_IMAGE_UPLOAD' THEN 'Please wait until the image upload finishes.'
    WHEN Context = 'Message' AND LabelKey = 'BOX_SAVED' THEN '¡Box saved!'
    WHEN Context = 'Message' AND LabelKey = 'IMAGE_UPDATED' THEN 'Image updated!'
    WHEN Context = 'Message' AND LabelKey = 'IMAGE_READY_TO_SAVE' THEN 'Image uploaded, you can now save it!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_BRAND' THEN 'Are you sure you want to delete this Brand ({0})?'
    WHEN Context = 'Message' AND LabelKey = 'BRAND_DELETED' THEN 'Brand deleted successfully!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_CATEGORY' THEN 'Are you sure you want to delete this Category ({0})?'
    WHEN Context = 'Message' AND LabelKey = 'CATEGORY_DELETED' THEN 'Category deleted successfully!'
    WHEN Context = 'Message' AND LabelKey = 'MOVED_TO_POSITION' THEN 'Moved to position {0}'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_ITEM' THEN 'Are you sure you want to delete this Item?'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_DELETED' THEN 'Item deleted successfully!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_REMOVE_ITEM_FROM_BOX' THEN 'Are you sure you want to remove this "{0}" branded item from the box "{1}"?'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_REMOVED_FROM_BOX' THEN 'Item removed from the box successfully'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_SAVED' THEN '¡Item saved!'
    WHEN Context = 'Message' AND LabelKey = 'STORAGE_UPDATED' THEN 'Storage updated successfully'
    WHEN Context = 'Message' AND LabelKey = 'PENDING_CHANGES' THEN 'You have unsaved changes. Are you sure you want to exit?'

    -- Welcome
    WHEN Context = 'Welcome' AND LabelKey = 'TITLE' THEN 'Inventory Management'
    WHEN Context = 'Welcome' AND LabelKey = 'SUBTITLE' THEN 'Select a module to start'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_TITLE' THEN 'Storage'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_SUBTITLE' THEN 'Direct stock entry'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_CONTENT' THEN 'Efficiently register new items into existing boxes with step-by-step validation.'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_TITLE' THEN 'Boxes'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_SUBTITLE' THEN 'Manage your storage'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_CONTENT' THEN 'Create new containers or search through your current setup.'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_TITLE' THEN 'Items'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_SUBTITLE' THEN 'Manage your items'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_CONTENT' THEN 'Browse your complete catalog of items, define new products, and manage descriptions.'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_TITLE' THEN 'Categories'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_SUBTITLE' THEN 'Classification'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_CONTENT' THEN 'Organize your inventory by types and groups for better searching.'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_TITLE' THEN 'Brands'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_SUBTITLE' THEN 'Manufacturers'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_CONTENT' THEN 'Manage item and brands suppliers in your system.'

END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];


INSERT INTO [dbo].[Translation] (LabelId, LanguageCode, [Text], CreatedAt, UpdatedAt)
SELECT LabelId, 'es-MX', 
CASE 
    -- Box
    WHEN Context = 'Box' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la caja con el ID {0}.'
    WHEN Context = 'Box' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una caja con este nombre en esta ubicación.'
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
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_TITLE' THEN 'Mover "{0}" a...'
    WHEN Context = 'Box' AND LabelKey = 'MOVE_CONFIRMATION_MSG' THEN 'Entiendo que mover "{0}" también moverá todos sus artículos internos y sub-cajas.'
    WHEN Context = 'Box' AND LabelKey = 'NAME' THEN 'Nombre'
    WHEN Context = 'Box' AND LabelKey = 'BRAND' THEN 'Marca'
    WHEN Context = 'Box' AND LabelKey = 'CATEGORY' THEN 'Categoría'

    -- Item
    WHEN Context = 'Item' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar el artículo con el ID {0}.'
    WHEN Context = 'Item' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe un artículo con este nombre.'
    WHEN Context = 'Item' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filtrar por nombre del artículo'
    WHEN Context = 'Item' AND LabelKey = 'FILTER_BY_CATEGORY' THEN 'Filtrar por categoría del artículo'
    WHEN Context = 'Item' AND LabelKey = 'NO_ITEMS_WITH_DESCRIPTION_OR_CATEGORY' THEN 'No hay artículos con esa descripción o categoría.'
    WHEN Context = 'Item' AND LabelKey = 'CREATE_NEW_INFO' THEN 'Puedes crear un nuevo artículo desde el menú:'
    WHEN Context = 'Item' AND LabelKey = 'NAME' THEN 'Nombre'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_NAME' THEN 'Nombre del artículo'
    WHEN Context = 'Item' AND LabelKey = 'NOTES' THEN 'Notas'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_NOTES' THEN 'Notas del artículo'
    WHEN Context = 'Item' AND LabelKey = 'CATEGORY' THEN 'Categoría'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_CATEGORY' THEN 'Categoría del artículo'
    WHEN Context = 'Item' AND LabelKey = 'BRAND' THEN 'Marca'
    WHEN Context = 'Item' AND LabelKey = 'ITEM_BRAND' THEN 'Marca del artículo'

    -- Brand
    WHEN Context = 'Brand' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la marca con el ID {0}.'
    WHEN Context = 'Brand' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una marca con este nombre.'
    WHEN Context = 'Brand' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filtrar por nombre de marca'
    WHEN Context = 'Brand' AND LabelKey = 'NAME' THEN 'Nombre'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_NAME' THEN 'Nombre de la marca'
    WHEN Context = 'Brand' AND LabelKey = 'COLOR' THEN 'Color'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_COLOR' THEN 'Color de la marca'
    WHEN Context = 'Brand' AND LabelKey = 'BACKGROUND' THEN 'Fondo'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_BACKGROUND' THEN 'Fondo de la marca'
    WHEN Context = 'Brand' AND LabelKey = 'SCOPE' THEN 'Alcance'
    WHEN Context = 'Brand' AND LabelKey = 'BRAND_SCOPE' THEN 'Alcance de la marca'
    WHEN Context = 'Brand' AND LabelKey = 'NO_BRANDS_FOUND' THEN 'No se encontraron marcas que coincidan con tu búsqueda.'

    -- Category
    WHEN Context = 'Category' AND LabelKey = 'NOT_FOUND' THEN 'No se encontró la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'CAN_NOT_DELETE' THEN 'No se pudo eliminar la categoría con el ID {0}.'
    WHEN Context = 'Category' AND LabelKey = 'ALREADY_EXISTS' THEN 'Ya existe una categoría con este nombre.'
    WHEN Context = 'Category' AND LabelKey = 'FILTER_BY_NAME' THEN 'Filtrar por nombre de categoría'
    WHEN Context = 'Category' AND LabelKey = 'NAME' THEN 'Nombre'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_NAME' THEN 'Nombre de la categoría'
    WHEN Context = 'Category' AND LabelKey = 'ICON' THEN 'Icono'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_ICON' THEN 'Icono de la categoría'
    WHEN Context = 'Category' AND LabelKey = 'COLOR' THEN 'Color'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_COLOR' THEN 'Color de la categoría'
    WHEN Context = 'Category' AND LabelKey = 'SCOPE' THEN 'Alcance'
    WHEN Context = 'Category' AND LabelKey = 'CATEGORY_SCOPE' THEN 'Alcance de la categoría'
    WHEN Context = 'Category' AND LabelKey = 'ORDER' THEN 'Orden'
    WHEN Context = 'Category' AND LabelKey = 'NO_CATEGORIES_FOUND' THEN 'No se encontraron categorías que coincidan con tu búsqueda.'

    -- Menu
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

    -- Global
    WHEN Context = 'Global' AND LabelKey = 'CREATED' THEN 'Creado'
    WHEN Context = 'Global' AND LabelKey = 'UPDATED' THEN 'Actualizado'
    WHEN Context = 'Global' AND LabelKey = 'DELETE' THEN 'Eliminar'
    WHEN Context = 'Global' AND LabelKey = 'DELETING' THEN 'Eliminando...'
    WHEN Context = 'Global' AND LabelKey = 'BACK' THEN 'Volver'
    WHEN Context = 'Global' AND LabelKey = 'CHANGE_PHOTO' THEN 'Cambiar Foto'
    WHEN Context = 'Global' AND LabelKey = 'UPLOAD_PHOTO' THEN 'Subir Foto'
    WHEN Context = 'Global' AND LabelKey = 'FILL_FORM_PHOTO' THEN 'Por favor complete el formulario, luego podrás subir la foto'
    WHEN Context = 'Global' AND LabelKey = 'SAVING' THEN 'Guardando...'
    WHEN Context = 'Global' AND LabelKey = 'UPLOADING_IMAGE' THEN 'Subiendo imagen...'
    WHEN Context = 'Global' AND LabelKey = 'SAVE' THEN 'Guardar'
    WHEN Context = 'Global' AND LabelKey = 'CANCEL' THEN 'Cancelar'
    WHEN Context = 'Global' AND LabelKey = 'MOVE' THEN 'Mover'
    WHEN Context = 'Global' AND LabelKey = 'EDIT' THEN 'Editar'
    WHEN Context = 'Global' AND LabelKey = 'NEW' THEN 'Nuevo'
    WHEN Context = 'Global' AND LabelKey = 'DETAILS' THEN 'Detalles'
    WHEN Context = 'Global' AND LabelKey = 'OPEN' THEN 'Abrir'
    WHEN Context = 'Global' AND LabelKey = 'UNREGISTERED' THEN 'Sin registrar'
    WHEN Context = 'Global' AND LabelKey = 'ALL' THEN 'Todo'
    WHEN Context = 'Global' AND LabelKey = 'NEXT' THEN 'Siguiente'
    WHEN Context = 'Global' AND LabelKey = 'CONFIRM_SAVE' THEN 'Confirmar y Guardar'
    WHEN Context = 'Global' AND LabelKey = 'YES' THEN 'Sí'
    WHEN Context = 'Global' AND LabelKey = 'NO' THEN 'No'
    WHEN Context = 'Global' AND LabelKey = 'OK' THEN 'Aceptar'    
    WHEN Context = 'Global' AND LabelKey = 'NOT_SELECTED' THEN 'No seleccionado'
    WHEN Context = 'Global' AND LabelKey = 'STOCK' THEN 'Inventario'

    -- Error
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_ASSIGN_FAILED' THEN 'Error al asignar la imagen.'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESS_ERROR' THEN 'Error interno al procesar la imagen: {0}'
    WHEN Context = 'Error' AND LabelKey = 'UNHANDLED_EXCEPTION' THEN 'Ocurrió una excepción no controlada: {0}'
    WHEN Context = 'Error' AND LabelKey = 'INTERNAL_SERVER_ERROR' THEN 'Ocurrió un error interno en el servidor.'
    WHEN Context = 'Error' AND LabelKey = 'REQUIRED' THEN 'Debes ingresar: {0}'
    WHEN Context = 'Error' AND LabelKey = 'MIN_LENGTH' THEN 'El campo {0} es demasiado corto'
    WHEN Context = 'Error' AND LabelKey = 'MAX_LENGTH' THEN 'El campo {0} es demasiado largo'
    WHEN Context = 'Error' AND LabelKey = 'MIN' THEN 'El valor debe ser mayor a 0'
    WHEN Context = 'Error' AND LabelKey = 'BRAND_NOT_FOUND' THEN 'La {0} seleccionada no es válida'
    WHEN Context = 'Error' AND LabelKey = 'CATEGORY_NOT_FOUND' THEN 'La {0} seleccionada no es válida'
    WHEN Context = 'Error' AND LabelKey = 'ENTITY_NOT_FOUND' THEN 'El valor seleccionado no es válido para: {0}'
    WHEN Context = 'Error' AND LabelKey = 'INVALID_DESTINATION' THEN 'Por favor selecciona un destino válido'
    WHEN Context = 'Error' AND LabelKey = 'REQUIRED_TRUE' THEN 'Debes aceptar el cambio para continuar'
    WHEN Context = 'Error' AND LabelKey = 'IMAGE_PROCESSING' THEN 'Error al procesar la imagen'
    WHEN Context = 'Error' AND LabelKey = 'EXECUTION_ERROR' THEN 'Error al ejecutar la acción'
    WHEN Context = 'Error' AND LabelKey = 'REMOVE_RELATIONSHIP_ERROR' THEN 'Ocurrió un error al intentar eliminar la relación del artículo'
    WHEN Context = 'Error' AND LabelKey = 'STORAGE_UPDATE_ERROR' THEN 'Error al actualizar el almacenamiento'

    -- Storage
    WHEN Context = 'Storage' AND LabelKey = 'NO_ITEMS_IN_BOX' THEN 'No hay artículos en la caja con el ID {0}.'
    WHEN Context = 'Storage' AND LabelKey = 'CAN_NOT_UPDATE' THEN 'No se pudo actualizar el almacenamiento.'
    WHEN Context = 'Storage' AND LabelKey = 'SELECT_BOX' THEN 'Selecciona una Caja'
    WHEN Context = 'Storage' AND LabelKey = 'SELECT_ITEM' THEN 'Selecciona un Artículo'
    WHEN Context = 'Storage' AND LabelKey = 'QUANTITY' THEN 'Cantidad'
    WHEN Context = 'Storage' AND LabelKey = 'QUANTITY_ABBREVIATION' THEN 'Cant'
    WHEN Context = 'Storage' AND LabelKey = 'STORAGE_QUANTITY' THEN 'Cantidad de almacenamiento'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRES' THEN '¿Expira?'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRES_ON' THEN 'Expira el'
    WHEN Context = 'Storage' AND LabelKey = 'EXPIRATION_DATE' THEN 'Fecha de expiración'
    WHEN Context = 'Storage' AND LabelKey = 'REVIEW_CONFIRM' THEN 'Revisar y Confirmar'
    WHEN Context = 'Storage' AND LabelKey = 'FILL_DETAILS' THEN 'Completa los detalles'
    WHEN Context = 'Storage' AND LabelKey = 'SUMMARY' THEN 'Resumen'

    -- Scope
    WHEN Context = 'Scope' AND LabelKey = 'ALL' THEN 'Todo'
    WHEN Context = 'Scope' AND LabelKey = 'ITEM' THEN 'Artículo'
    WHEN Context = 'Scope' AND LabelKey = 'BOX' THEN 'Caja'
    WHEN Context = 'Scope' AND LabelKey = 'NONE' THEN 'Ninguno'

    -- Message
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_BOX' THEN '¿Estás seguro de que quieres eliminar esta caja?'
    WHEN Context = 'Message' AND LabelKey = 'BOX_DELETED' THEN '¡Caja eliminada con éxito!'
    WHEN Context = 'Message' AND LabelKey = 'WAIT_IMAGE_UPLOAD' THEN 'Por favor, espera a que termine la carga de la imagen.'
    WHEN Context = 'Message' AND LabelKey = 'BOX_SAVED' THEN '¡Caja guardada!'
    WHEN Context = 'Message' AND LabelKey = 'IMAGE_UPDATED' THEN '¡Imagen actualizada!'
    WHEN Context = 'Message' AND LabelKey = 'IMAGE_READY_TO_SAVE' THEN '¡Imagen subida, ahora puedes guardar!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_BRAND' THEN '¿Estás seguro de que quieres eliminar esta marca ({0})?'
    WHEN Context = 'Message' AND LabelKey = 'BRAND_DELETED' THEN '¡Marca eliminada con éxito!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_CATEGORY' THEN '¿Estás seguro de que quieres eliminar esta categoría ({0})?'
    WHEN Context = 'Message' AND LabelKey = 'CATEGORY_DELETED' THEN '¡Categoría eliminada con éxito!'
    WHEN Context = 'Message' AND LabelKey = 'MOVED_TO_POSITION' THEN 'Movido a la posición {0}'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_DELETE_ITEM' THEN '¿Estás seguro de que quieres eliminar este artículo?'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_DELETED' THEN '¡Artículo eliminado con éxito!'
    WHEN Context = 'Message' AND LabelKey = 'CONFIRM_REMOVE_ITEM_FROM_BOX' THEN '¿Estás seguro de que quieres quitar el artículo "{0}" de la caja "{1}"?'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_REMOVED_FROM_BOX' THEN 'Artículo quitado de la caja con éxito'
    WHEN Context = 'Message' AND LabelKey = 'ITEM_SAVED' THEN '¡Artículo guardado!'
    WHEN Context = 'Message' AND LabelKey = 'STORAGE_UPDATED' THEN 'Almacenamiento actualizado con éxito'
    WHEN Context = 'Message' AND LabelKey = 'PENDING_CHANGES' THEN 'Tienes cambios sin guardar. ¿Estás seguro de que quieres salir?'

    -- Welcome
    WHEN Context = 'Welcome' AND LabelKey = 'TITLE' THEN 'Gestión de Inventario'
    WHEN Context = 'Welcome' AND LabelKey = 'SUBTITLE' THEN 'Selecciona un módulo para comenzar'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_TITLE' THEN 'Almacenamiento'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_SUBTITLE' THEN 'Entrada directa de stock'
    WHEN Context = 'Welcome' AND LabelKey = 'STORAGE_CARD_CONTENT' THEN 'Registra eficientemente nuevos artículos en cajas existentes con validación paso a paso.'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_TITLE' THEN 'Cajas'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_SUBTITLE' THEN 'Gestiona tu almacenamiento'
    WHEN Context = 'Welcome' AND LabelKey = 'BOX_CARD_CONTENT' THEN 'Crea nuevos contenedores o busca en tu configuración actual.'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_TITLE' THEN 'Artículos'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_SUBTITLE' THEN 'Gestiona tus artículos'
    WHEN Context = 'Welcome' AND LabelKey = 'ITEM_CARD_CONTENT' THEN 'Explora tu catálogo completo, define nuevos productos y gestiona descripciones.'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_TITLE' THEN 'Categorías'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_SUBTITLE' THEN 'Clasificación'
    WHEN Context = 'Welcome' AND LabelKey = 'CATEGORY_CARD_CONTENT' THEN 'Organiza tu inventario por tipos y grupos para una mejor búsqueda.'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_TITLE' THEN 'Marcas'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_SUBTITLE' THEN 'Fabricantes'
    WHEN Context = 'Welcome' AND LabelKey = 'BRAND_CARD_CONTENT' THEN 'Gestiona marcas y proveedores en tu sistema.'

END, SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET()
FROM [Label];