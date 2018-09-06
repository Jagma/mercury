using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class TRWebBuild {
	
	[MenuItem("Plugins/TableRealms/Build Browser")]
    public static void BuildBrowser() {
        Build("TRBrowser");
    }

/*    [MenuItem("TR Web Build/Build Embedded")]
    public static void BuildEmbedded() {
        Build("TREmbedded");
    }*/
	
	[MenuItem("Plugins/TableRealms/Remove Warnig")]
    public static void RemoveMobileWarning() {
	    //string dataPath = Application.dataPath;
	    string path = EditorUtility.OpenFolderPanel("Select Build Folder", Application.dataPath, "");
	    RemoveMobileWarning(path);
    }


	public static void Build (string template) {
		
		string path = EditorUtility.OpenFolderPanel("Select Build Folder", Application.dataPath, "");
		
		if (path.Length > 0) {
			PlayerSettings.WebGL.template = "PROJECT:" + template;
	        BuildPlayerOptions options = new BuildPlayerOptions();
			options.target = BuildTarget.WebGL;
	
	        // Create the target path of the build
			// string dataPath = Application.dataPath;
			//string buildPath = dataPath.Substring(0, dataPath.LastIndexOf("/")) + "/Build " + template;
			options.locationPathName = path;
	
	        // Get the scenes in the build
	        string[] scenesInBuild = new string[EditorBuildSettings.scenes.Length];
	        for (int i = 0; i < scenesInBuild.Length; i++) {
	            scenesInBuild[i] = EditorBuildSettings.scenes[i].path;
	        }
	        options.scenes = scenesInBuild;
	
	        // Build player.
			//FileUtil.DeleteFileOrDirectory(path);
			UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(options);

			if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded) RemoveMobileWarning(path);
			
			Debug.Log("WebGL Build: Successful");
		}
    }

    public static void RemoveMobileWarning (string buildPath) {
	    StreamReader sr = new StreamReader(buildPath + "/Build/UnityLoader.js");
        string unityloaderFile = sr.ReadToEnd();
        sr.Close();

	    FileUtil.DeleteFileOrDirectory(buildPath + "/Build/UnityLoader.js");

        unityloaderFile = unityloaderFile.Replace(@"UnityLoader.SystemInfo.hasWebGL?UnityLoader.SystemInfo.mobile?e.popup(""Please note that Unity WebGL is not currently supported on mobiles. Press OK if you wish to continue anyway."",[{text:""OK"",callback:t}]):[""Edge"",""Firefox"",""Chrome"",""Safari""].indexOf(UnityLoader.SystemInfo.browser)==-1?e.popup(""Please note that your browser is not currently supported for this Unity WebGL content. Press OK if you wish to continue anyway."",[{text:""OK"",callback:t}]):t():e.popup(""Your browser does not support WebGL"",[{text:""OK"",callback:r}])", "t();");

	    StreamWriter sw = new StreamWriter(buildPath + "/Build/UnityLoader.js");
        sw.Write(unityloaderFile);
	    sw.Close();
	    
	    Debug.Log("Remove Warning: Successful");
    }
}