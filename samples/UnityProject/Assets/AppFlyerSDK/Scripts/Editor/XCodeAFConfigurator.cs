#if UNITY_IPHONE
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class XCodeAFConfigurator
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            BuildForiOS(path);
        }
    }
    private static void BuildForiOS(string path)
    {
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        Debug.Log("Build iOS. path: " + projPath);

        PBXProject proj = new PBXProject();
        var file = File.ReadAllText(projPath);
        proj.ReadFromString(file);

        string target = proj.TargetGuidByName("Unity-iPhone");

        proj.AddFrameworkToProject(target, "AdSupport.framework", false);
        proj.AddFrameworkToProject(target, "iAd.framework", false);

        File.WriteAllText(projPath, proj.WriteToString());
    }
}
#endif
