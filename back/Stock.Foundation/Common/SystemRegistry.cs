namespace Stock.Foundation.Common;

public static class SystemRegistry
{
    public enum Entity : byte
    {
        Box = 1,
        Brand = 2,
        Category = 3,
        Item = 4,
        Label = 5,
        Language = 6,
        Storage = 7,
        Transaction = 8,
    }

    public enum Event : byte
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Read = 4,
        Move = 5,
        Reorder = 6,
        UpdateImage = 7
    }
}
