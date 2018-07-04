using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;




using System.Diagnostics;
 
class BuildGame : EditorWindow {

    [MenuItem("Window/Building")]

    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(BuildGame));
    }

    public string[] scenes = { "Assets/Scenes/Campus.unity", "Assets/Scenes/RobotShop.unity" };

    bool withUpload = true;

    void OnGUI() {
        // The actual window code goes here
        GUILayout.Label("Build the Binary");

        EditorGUILayout.Toggle("upload after build", withUpload);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Build for Windows")) {
            PerformBuildWin();
        }
        if (GUILayout.Button("Build for Linux")) {
            PerformBuildLinux();
        }
        if (GUILayout.Button("Build for Mac")) {
            PerformBuildMac();
        }
        if (GUILayout.Button("Build for all")) {
            PerformBuildWin();
            PerformBuildLinux();
            PerformBuildMac();
        }
        
    }

    string workingDir = "E:/Unity Projects/RobotAnimalTycoon/builds/current";

    void PerformBuildWin() {

        BuildPipeline.BuildPlayer(scenes, "builds/current/win/RobotierTyccon.exe", BuildTarget.StandaloneWindows,BuildOptions.None);
        if (withUpload) {
            ProcessStartInfo proc = new ProcessStartInfo("uploadwin.bat");
            proc.WorkingDirectory = workingDir;
            Process.Start(proc);
        }
    }
    void PerformBuildLinux() {

        BuildPipeline.BuildPlayer(scenes, "builds/current/linux/RobotierTyccon", BuildTarget.StandaloneLinuxUniversal, BuildOptions.None);
        if (withUpload) {
            ProcessStartInfo proc = new ProcessStartInfo("uploadlinux.bat");
            proc.WorkingDirectory = workingDir;
            Process.Start(proc);
        }
    }
    void PerformBuildMac() {

        BuildPipeline.BuildPlayer(scenes, "builds/current/mac/RobotierTyccon", BuildTarget.StandaloneOSX, BuildOptions.None);
        if (withUpload) {
            ProcessStartInfo proc = new ProcessStartInfo("uploadmac.bat");
            proc.WorkingDirectory = workingDir;
            Process.Start(proc);
        }
    }
}