using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

public class PluginManagerWindow : EditorWindow {
    private static string DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm";
  private static string PLUGINS_FOLDER = Path.DirectorySeparatorChar + "Assets";// + Path.DirectorySeparatorChar + "Plugins";

    private static Dictionary<string, Dictionary<string, string>> knownPlugins = new Dictionary<string, Dictionary<string, string>>();

    private GUIStyle horizontalLine = new GUIStyle();

    public void FindAllPlugins() {
        knownPlugins = new Dictionary<string, Dictionary<string, string>>();
        SearchDirectory(Directory.GetCurrentDirectory() + PLUGINS_FOLDER);

        foreach (string name in knownPlugins.Keys) {
            ReadOnlineVersion(name);
        }
    }

    private void ReadProperties(string file, Dictionary<string, string> data) {
        foreach (string row in File.ReadAllLines(file)) {
            string line = row.Trim();
            if (!line.StartsWith("#")) {
                int index = line.IndexOf("=");
                if (index != -1) {
                    data.Add(line.Substring(0, index), line.Substring(index + 1).Trim());
                }
            }
        }
    }

    private Dictionary<string, string> ReadProperties(string file) {
        Dictionary<string, string> data = new Dictionary<string, string>();
        ReadProperties(file, data);
        return data;
    }

    private bool SearchDirectory(string directory) {
        string[] files = Directory.GetFiles(directory, "PluginManager.properties");

        if (files.Length == 1) {
            Dictionary<string, string> pluginProperties = ReadProperties(files[0]);
            if (pluginProperties.ContainsKey("name")) {
                knownPlugins.Add(pluginProperties["name"], pluginProperties);
                Debug.Log("PlunginManager: Found pluging '" + pluginProperties["name"] + "'");
                pluginProperties.Add("directory", directory);
            } else {
                Debug.LogWarning("PlunginManager: Unable to understand pluging '" + files[0] + "'");
            }

            string[] ftpFiles = Directory.GetFiles(directory, "PluginManagerFTP.properties");
            if (ftpFiles.Length == 1) {
                ReadProperties(ftpFiles[0], pluginProperties);
            }

            string[] versionFiles = Directory.GetFiles(directory, FixNameForFileName(pluginProperties["name"]) +".version");
            if (versionFiles.Length == 1) {
                pluginProperties.Add("version", System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(versionFiles[0])));
            }

            return true;
        }

        string[] directories = Directory.GetDirectories(directory);
        foreach (string d in directories) {
            SearchDirectory(d);
        }
        return false;
    }

    [MenuItem("Plugins/PluginManager/Show Window")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(PluginManagerWindow));
    }

    public void OnEnable() {
        FindAllPlugins();

        horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
        horizontalLine.margin = new RectOffset(0, 0, 4, 4);
        horizontalLine.fixedHeight = 1;
    }

    private void HorizontalLine(Color color) {
        var c = GUI.color;
        GUI.color = color;
        GUILayout.Box(GUIContent.none, horizontalLine);
        GUI.color = c;
    }

    private void ReadOnlineVersion(string name) {
        Dictionary<string, string> pluginProperties = knownPlugins[name];

        string url = pluginProperties["url"] + "/"+ FixNameForFileName(name) + ".version";
        WWW w = new WWW(url);
        while (!w.isDone) ;
        if (w.error != null) {
            if (pluginProperties.ContainsKey("latestVersion")) {
                pluginProperties.Remove("latestVersion");
            }
            Debug.LogWarning("PluginManager: Unable to get version for plugin '" + name + "@" + url + "' error " + w.error);
        } else {
            if (pluginProperties.ContainsKey("latestVersion")) {
                pluginProperties.Remove("latestVersion");
            }
            pluginProperties.Add("latestVersion", w.text);
        }
    }

    private string FixDateTimeForFileName(string datatimeString) {
        return datatimeString.Replace(' ', '_').Replace(':', 'h').Replace('/', '_');
    }

    private string FixNameForFileName(string name) {
        return name.Replace(' ', '_');
    }

    private void UpdatePlugin(string name) {
        Dictionary<string, string> pluginProperties = knownPlugins[name];
        string url = pluginProperties["url"] + "/" + FixNameForFileName(name) + "_" + FixDateTimeForFileName(pluginProperties["latestVersion"]) + ".zip";
        Debug.Log("PluginManager: Downloading " + url);
        WWW w = new WWW(url);
        while (!w.isDone) ;
        if (w.error != null) {
            Debug.LogError("PluginManager: Unable to update plugin '" + name + "@" + url + "' error " + w.error);
        } else {
            IoUtils.ExtractZipFile(w.bytes, pluginProperties["directory"]);
            string versionFileName = FixNameForFileName(name) + ".version";
            if (pluginProperties.ContainsKey("version")) {
                pluginProperties.Remove("version");
            }
            pluginProperties.Add("version", pluginProperties["latestVersion"]);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(pluginProperties["latestVersion"]);
            File.WriteAllBytes(pluginProperties["directory"] + Path.DirectorySeparatorChar + versionFileName, buffer);
        }
    }

    private void PushPlugin(string name) {
        Dictionary<string, string> pluginProperties = knownPlugins[name];
        byte[] bytes = IoUtils.CompressZipFile(pluginProperties["directory"], new String[] { ".*\\.meta\\Z", "\\APluginManager\\.properties\\Z", "\\APluginManagerFTP\\.properties\\Z", ".*\\.version\\Z" }, null);

        string version = DateTime.Now.ToString(DATE_TIME_FORMAT);
        string filename = FixNameForFileName(name) + "_" + FixDateTimeForFileName(version) + ".zip";
        string directory = pluginProperties["ftp_directory"];
        if (directory.Length > 0 && !directory.EndsWith("/")) {
            directory = directory + "/";
        }

        if (IoUtils.FTPUploadFile(pluginProperties["ftp_host"], directory + filename, pluginProperties["ftp_username"], pluginProperties["ftp_password"], bytes)) {

            string versionFileName = FixNameForFileName(name) + ".version";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(version);
            if (IoUtils.FTPUploadFile(pluginProperties["ftp_host"], directory + versionFileName, pluginProperties["ftp_username"], pluginProperties["ftp_password"], buffer)) {
                File.WriteAllBytes(pluginProperties["directory"] + Path.DirectorySeparatorChar + versionFileName, buffer);
                if (pluginProperties.ContainsKey("version")) {
                    pluginProperties.Remove("version");
                }
                pluginProperties.Add("version", version);
                if (pluginProperties.ContainsKey("latestVersion")) {
                    pluginProperties.Remove("latestVersion");
                }
                pluginProperties.Add("latestVersion", version);
            }
        }
    }

    public void OnGUI() {
        if (GUILayout.Button("Refresh All")) {
            FindAllPlugins();
        }
        foreach (string name in knownPlugins.Keys) {
            Dictionary<string, string> pluginProperties = knownPlugins[name];

            HorizontalLine(Color.grey);
            GUILayout.Label(name, EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Url: ", pluginProperties["url"]);
            DateTime version = DateTime.MinValue;
            DateTime newVersion = DateTime.MinValue;
            if (pluginProperties.ContainsKey("version") && pluginProperties["version"].Length>0) {
                EditorGUILayout.LabelField("Version: ", pluginProperties["version"]);
                version = DateTime.ParseExact(pluginProperties["version"], DATE_TIME_FORMAT, null);
            } else {
                EditorGUILayout.LabelField("Version: ", "");
            }
            if (pluginProperties.ContainsKey("latestVersion") && pluginProperties["latestVersion"].Length > 0) {
                EditorGUILayout.LabelField("Latest Version: ", pluginProperties["latestVersion"]);
                newVersion = DateTime.ParseExact(pluginProperties["latestVersion"], DATE_TIME_FORMAT, null);
            } else {
                EditorGUILayout.LabelField("Latest Version: ", "Unknown");
            }

            if (newVersion > version) {
                if (GUILayout.Button("Update")) {
                    UpdatePlugin(name);
                }
            }

            if (pluginProperties.ContainsKey("ftp_username")) {
                if (GUILayout.Button("Push New Version")) {
                    PushPlugin(name);
                }
            }

        }
        HorizontalLine(Color.grey);
    }
}
