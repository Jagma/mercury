using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class TaleRealmsMenu {
	private static string TABLEREALMS_FOLDER=Path.DirectorySeparatorChar+ "Assets" + Path.DirectorySeparatorChar + "Plugins" + Path.DirectorySeparatorChar+"Editor";
    private static string DEFAULT_WORK_FILE = Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar + "TableRealms.tr.bytes";

	private static string WORK_FILE_KEY="WorkingFile";

	private static Dictionary<string,string> settings=new Dictionary<string, string>();
	
	private static bool CheckFolders(){
#if UNITY_STANDALONE_WIN
		string file = "TableRealms.exe";
#else
		string file = "TableRealms.jar";
#endif
		string directory = Directory.GetCurrentDirectory()+TABLEREALMS_FOLDER;
		
		// Looks like it was moved lets serch for it
		if (!File.Exists(directory+Path.DirectorySeparatorChar+file)){
			string foundIn=SearchDirectory(Directory.GetCurrentDirectory(),file);
			if (foundIn!=null){
				TABLEREALMS_FOLDER=foundIn.Substring(Directory.GetCurrentDirectory().Length);
			}
		}
		
		if (!File.Exists(Directory.GetCurrentDirectory()+TABLEREALMS_FOLDER+Path.DirectorySeparatorChar+file)){
			Debug.LogWarning("TableRealms not found in expected folder, searching project.");
			ShowCanNotFindFilesMessage();
			return false;
		}
		
		return true;
	}
	
	private static string SearchDirectory(string directory,string file){
		string[] files=Directory.GetFiles(directory,file);
		//Debug.LogWarning(directory+"="+files.Length);
		if (files.Length==1){
			return directory;
		}
		
		string[] directories=Directory.GetDirectories(directory);
		foreach (string d in directories){
			string found=SearchDirectory(d,file);
			if (found!=null){
				return found;
			}
		}
		return null;
	}
	
	private static string GetConfigFile(){
		return Directory.GetCurrentDirectory()+TABLEREALMS_FOLDER+Path.DirectorySeparatorChar+"Config.properties";
	}
	
	private static void SaveConfig(){
		if (!CheckFolders()){
            Debug.LogWarning("Could not verify TableRealms folder not saving.");
			return;
		}
		string[] values=new string[settings.Count];
		
		int count=0;
		foreach (string k in settings.Keys){
			values[count++]=k+"="+settings[k];
			Debug.Log(k+"="+settings[k]);
		}
		
		File.WriteAllLines(GetConfigFile(),values);
	}
	
	private static void LoadConfig(){
		if (!CheckFolders()){
			return;
		}
		string configFile=GetConfigFile();
		if (File.Exists(configFile)){
			// Load config
			string[] lines=File.ReadAllLines(configFile);
			foreach (string line in lines){
				string l=line.Trim();
				if (!l.StartsWith("#")){
					int i=l.IndexOf("=");
					if (i!=-1){
						string n=l.Substring(0,i).Trim();
						string v=l.Substring(i+1).Trim();
						settings[n]=v;
					}
				}
			}
		}
	}
	
	private static string GetWorkingFile(){
		string workingFolder= DEFAULT_WORK_FILE;
		if (settings.ContainsKey(WORK_FILE_KEY)){
			workingFolder=settings[WORK_FILE_KEY];
			workingFolder=workingFolder.Replace(Path.AltDirectorySeparatorChar,Path.DirectorySeparatorChar);
		}
		return Directory.GetCurrentDirectory()+workingFolder+"\\TableRealms.tr.bytes";
	}
	
	[MenuItem("Plugins/TableRealms/Start Designer #&t")]
    public static void StartEditor () { 
		if (!CheckFolders()){
			return;
		}
		System.Diagnostics.Process proc=new System.Diagnostics.Process();
		proc.EnableRaisingEvents=false;
		
		LoadConfig();
		string workingFile=GetWorkingFile();

#if UNITY_EDITOR_WIN
		proc.StartInfo.FileName = Directory.GetCurrentDirectory()+TABLEREALMS_FOLDER+Path.DirectorySeparatorChar+"TableRealms.exe";
#else
		proc.StartInfo.FileName = Directory.GetCurrentDirectory()+TABLEREALMS_FOLDER+Path.DirectorySeparatorChar+"TableRealms.jar";
#endif
        proc.StartInfo.Arguments = "\""+ workingFile + "\"";
		
		Debug.Log(proc.StartInfo.FileName);
		Debug.Log(proc.StartInfo.Arguments);
		
		// Identify is Java is installed. If not then offer to download it.
		try {
			if (!proc.Start()){
				Debug.LogError("Unable to execute '"+proc.StartInfo.FileName+" "+proc.StartInfo.Arguments+"'");
				ShowNeedJavaMessage();
			}
		} catch (Exception e){
			Debug.LogException(e);
			ShowNeedJavaMessage();
		}
	}
	
	private static void ShowNeedJavaMessage(){
		if (EditorUtility.DisplayDialog("Unable to start TableRealms. Java Required.","TableRealms requires you have Java installed on your system to run. Would you like to open your web browser to the download page?","Yes Please","No Thanks")){
			Application.OpenURL("http://java.com/en/download/index.jsp");
		}
	}
	
	private static void ShowCanNotFindFilesMessage(){
#if UNITY_STANDALONE_WIN
		string file = "TableRealms.exe";
#else
		string file = "TableRealms.jar";
#endif
		EditorUtility.DisplayDialog("Unable to start TableRealms.","The TableRealms '"+file+"' file can not be found in your project. Did you import it correctly.","Ok");
	}

    [MenuItem("Plugins/TableRealms/Pick Working Folder")]
    public static void PickWorkingFolder () { 
		LoadConfig();
		string workingFile=GetWorkingFile();
		
		//string folderName=workingFolder.Substring(workingFolder.LastIndexOf(""+Path.DirectorySeparatorChar)+1);
		
		string newfolder=EditorUtility.SaveFolderPanel("Working file", workingFile, null);
		
		if (newfolder!=null && newfolder.Length>0){
			newfolder=newfolder.Replace(Path.AltDirectorySeparatorChar,Path.DirectorySeparatorChar);
			Debug.Log(newfolder);
			
			// Test that this is still inside the untiy working structure
			if (newfolder.StartsWith(Directory.GetCurrentDirectory())){
				newfolder=newfolder.Substring(Directory.GetCurrentDirectory().Length);
				
				// Save this folder
				settings.Remove(WORK_FILE_KEY);
				settings.Add(WORK_FILE_KEY, newfolder);
				SaveConfig();
			}else{
				EditorUtility.DisplayDialog("Invalid Working Folder.","The folder you pick must be inside the project space of your Unity project.","Ok");
			}
		}
		
	}
	
}
