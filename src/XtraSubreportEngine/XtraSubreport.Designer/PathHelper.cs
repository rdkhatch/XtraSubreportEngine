
namespace XtraSubreport.Designer
{
    //public static class PathHelper
    //{

    //    public static string GetBasePath()
    //    {
    //        return GetDirectoryName(_fullBasePath);
    //    }

    //    private static string MakeIntoFolderPath(string path)
    //    {
    //        // Always end a folder path with a slash
    //        // Important for telling files & folders apart
    //        if (!path.EndsWith(@"\"))
    //            path = path + @"\";

    //        return path;
    //    }

    //    public static string GetDirectoryName(string filepath)
    //    {
    //        string path = Path.GetDirectoryName(filepath);
    //        path = MakeIntoFolderPath(path);
    //        return path;
    //    }

    //    public IEnumerable<string> GetAllFoldersWithinBasePathContainingDLLs()
    //    {
    //        var basePath = GetBasePath();

    //        return (from filePath in Directory.GetFiles(basePath, "*.dll", SearchOption.AllDirectories)
    //                let folderName = GetDirectoryName(filePath)
    //                select MakeRelativePath(folderName)
    //               ).Distinct();
    //    }

    //    public string FormatRelativePath(string relativePath)
    //    {
    //        var fullPath = MakeFullPath(relativePath);
    //        var result = MakeRelativePath(fullPath);
    //        return result;
    //    }

    //    public string MakeFullPath(string relativePath)
    //    {
    //        return Path.Combine(GetBasePath(), relativePath);
    //    }

    //    public string MakeRelativePath(string fullPath)
    //    {
    //        return MakeRelativePath(fullPath, GetBasePath());
    //    }

    //    private static String MakeRelativePath(string fullPath, string relativetoPath)
    //    {
    //        if (String.IsNullOrEmpty(relativetoPath)) throw new ArgumentNullException("fullPath");
    //        if (String.IsNullOrEmpty(fullPath)) throw new ArgumentNullException("relativetoPath");

    //        relativetoPath = MakeIntoFolderPath(relativetoPath);

    //        bool dontEscape = true;

    //        // Change Windows Slashes into URI slashes
    //        fullPath = fullPath.Replace(@"\", "/");
    //        relativetoPath = relativetoPath.Replace(@"\", "/");

    //        Uri fromUri = new Uri(relativetoPath, dontEscape);
    //        Uri toUri = new Uri(fullPath, dontEscape);

    //        Uri relativeUri = fromUri.MakeRelativeUri(toUri);

    //        // Change URI slahes back into Windows Slashes
    //        return relativeUri.ToString().Replace("/", @"\");
    //    }
    //}
}
