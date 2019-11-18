#if UNITY_IPHONE
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class XCodeTDConfigurator
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
        proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
        proj.AddFrameworkToProject(target, "CoreMotion.framework", false);
        proj.AddFrameworkToProject(target, "Security.framework", false);
        proj.AddFrameworkToProject(target, "SystemConfiguration.framework", false);

        AddUsrLib(proj, target, "libc++.tbd");
        AddUsrLib(proj, target, "libz.tbd");

        File.WriteAllText(projPath, proj.WriteToString());
    }

    private static void AddUsrLib(PBXProject proj, string targetGuid, string framework)
    {
        string fileGuid = proj.AddFile("usr/lib/" + framework, "Frameworks/" + framework, PBXSourceTree.Sdk);
        proj.AddFileToBuild(targetGuid, fileGuid);
    }
}
#endif
