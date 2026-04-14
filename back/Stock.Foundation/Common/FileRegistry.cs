namespace Stock.Foundation.Common;

public static class FileRegistry
{
    public static string GetRandomImageName(out string guid)
    {
        guid = Guid.NewGuid().ToString();
        return GetImageName(guid);
    }

    public static string GetImageName(object? baseName)
    {
        return $"{baseName ?? "unknown"}{Extension.Image}";
    }

    public static string GetSyncFileName(string origin, int id)
    {
        return $"{Prefix.Sync}{origin}_{id}{Extension.Sync}";
    }

    public static string GetDeletedImageName(string origin, int id, string folder)
    {
        return $"{Prefix.Deleted}{origin}_{id}_{folder}_{Guid.NewGuid()}{Extension.Image}";
    }

    public static class Folder
    {
        public const string Image = "img";
        public const string ImageSlashed = "img/";
        public const string Temp = "temp";
        public const string TempSlashed = "temp/";

        public const string Box = "box";
        public const string Item = "item";

        public static class SubFolder
        {
            public const string Original = "original";
            public const string Thumbnails = "thumbnails";
            public const string Icons = "icons";

            public static readonly string[] All = [Original, Thumbnails, Icons];
        }
    }

    public static class Path
    {
        public const string HostMount = "/host_mnt";

        public static string GetLocal(string hostedPath)
        {
            if (string.IsNullOrEmpty(hostedPath)) return string.Empty;

            if (System.IO.Path.DirectorySeparatorChar == '/')
            {
                return hostedPath;
            }

            return hostedPath.Replace(HostMount, "").Replace("/c/", "C:\\").Replace("/", "\\");
        }

        public static string Normalize(string path)
        {
            return path.Replace(System.IO.Path.DirectorySeparatorChar, '/').TrimStart('/');
        }
    }

    public static class Extension
    {
        public const string Image = ".png";
        public const string Sync = ".txt";
    }

    public static class Prefix
    {
        public const string Deleted = "deleted_";
        public const string Sync = "sync_";
    }
}
