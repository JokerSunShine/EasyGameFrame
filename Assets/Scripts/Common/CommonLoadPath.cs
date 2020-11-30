using UnityEngine;

public class CommonLoadPath {
    private static string localProjectName;
    public static string LocalProjectName {
        get {
            if (string.IsNullOrEmpty(localProjectName))
            {
                localProjectName = Application.dataPath.Replace("/Assets", "");
                localProjectName = localProjectName.Substring(localProjectName.LastIndexOf("/") + 1);
            }
            return localProjectName;
        }
    }

    private static string loadRootPath;
    public static string LoadRootPath {
        get {
            if (string.IsNullOrEmpty(loadRootPath))
            {
                loadRootPath = System.Text.RegularExpressions.Regex.Replace(Application.dataPath, "Client/(\\w+)/" + LocalProjectName + "/Assets",
            "Data/$1/CurrentUseData/Normal/");
            }
            return loadRootPath;
        }
    }
}