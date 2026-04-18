namespace Stock.Foundation.Common;

public static class LabelRegistry
{
    public static class Context
    {
        public const string Box = "Box";
        public const string Brand = "Brand";
        public const string Category = "Category";
        public const string Item = "Item";
        public const string Error = "Error";
        public const string Storage = "Storage";
    }

    public static class Key
    {
        public const string NotFound = "NOT_FOUND";
        public const string CanNotUpdate = "CAN_NOT_UPDATE";
        public const string CanNotDelete = "CAN_NOT_DELETE";
        public const string AlreadyExists = "ALREADY_EXISTS";
        public const string NoItemsInBox = "NO_ITEMS_IN_BOX";
        public const string ImageAssignFailed = "IMAGE_ASSIGN_FAILED";
        public const string ImageProcessError = "IMAGE_PROCESS_ERROR";
        public const string UnhandledException = "UNHANDLED_EXCEPTION";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
    }
}
