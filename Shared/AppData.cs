using System;
using System.Web;

public static class AppData
{
    private static readonly string baseDataPath = HttpContext.Current.Server.MapPath(@"~\App_Data\");
    public static string GetFullFilename(string dataFilename)
    {
        var fullpath = baseDataPath + dataFilename;
        if (!fullpath.StartsWith(baseDataPath, StringComparison.OrdinalIgnoreCase))
            throw new Exception("Full filename exceeds data path: " + dataFilename);
        return fullpath;
    }
}
