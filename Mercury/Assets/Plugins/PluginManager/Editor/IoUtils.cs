using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

public class IoUtils {
    public delegate void ProgressDelegate(string sMessage);
 
    public static byte[] CompressZipFile(string targetDirectory, string[] filesToSkip, ProgressDelegate progress) {
        string[] sFiles = Directory.GetFiles(targetDirectory, "*.*", SearchOption.AllDirectories);
        int iDirLen = targetDirectory[targetDirectory.Length - 1] == Path.DirectorySeparatorChar ? targetDirectory.Length : targetDirectory.Length + 1;
        MemoryStream memoryStream = new MemoryStream();
        ZipOutputStream zipFile = new ZipOutputStream(memoryStream);
        zipFile.SetLevel(3);
        byte[] buffer = new byte[64*1024];

        foreach (string sFilePath in sFiles) {
            string sRelativePath = sFilePath.Substring(iDirLen);
            bool skipFile = false;
            if (filesToSkip != null) {
                foreach (string pattern in filesToSkip) {
                    skipFile = Regex.IsMatch(sRelativePath, pattern, RegexOptions.IgnoreCase);
                    if (skipFile) {
                        break;
                    }
                }
            }
            FileInfo fi = new FileInfo(sFilePath);
            if (!skipFile && fi.Length>0) {
                if (progress != null) {
                    progress(sRelativePath);
                }

                string entryName = sFilePath.Substring(iDirLen);
                entryName = ZipEntry.CleanName(entryName);
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime;
                newEntry.Size = fi.Length;

                zipFile.PutNextEntry(newEntry);

                using (FileStream streamReader = File.OpenRead(sFilePath)) {
                    StreamUtils.Copy(streamReader, zipFile, buffer);
                }
                zipFile.CloseEntry();

            }
        }
        zipFile.IsStreamOwner = true;
        zipFile.Close();
        memoryStream.Close();

        return memoryStream.ToArray();
    }

    public static void ExtractZipFile(byte[] zipFileData, string targetDirectory, int bufferSize = 256 * 1024) {
        Directory.CreateDirectory(targetDirectory);

        using (MemoryStream fileStream = new MemoryStream()) {
            fileStream.Write(zipFileData, 0, zipFileData.Length);
            fileStream.Flush();
            fileStream.Seek(0, SeekOrigin.Begin);

            ZipFile zipFile = new ZipFile(fileStream);

            foreach (ZipEntry entry in zipFile) {
                string targetFile = Path.Combine(targetDirectory, entry.Name);

                using (FileStream outputFile = File.Create(targetFile)) {
                    if (entry.Size > 0) {
                        Stream zippedStream = zipFile.GetInputStream(entry);
                        byte[] dataBuffer = new byte[bufferSize];

                        int readBytes;
                        while ((readBytes = zippedStream.Read(dataBuffer, 0, bufferSize)) > 0) {
                            outputFile.Write(dataBuffer, 0, readBytes);
                            outputFile.Flush();
                        }
                    }
                }
            }
        }
    }

    public static bool FTPUploadFile(string host,string remoteFile, string user,string pass,byte[] fileBytes) {
        try {
            FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("ftp://" + host + "/" + remoteFile);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            Stream ftpStream = ftpRequest.GetRequestStream();
            try {
                byte[] buffer = new byte[4096];
                using (Stream streamReader = new MemoryStream(fileBytes)) {
                    StreamUtils.Copy(streamReader, ftpStream, buffer);
                }
            } catch (Exception ex) {
                Debug.LogError("PluginManager: " + ex.ToString());
                return false;
            }
            ftpStream.Close();
            ftpRequest = null;
        } catch (Exception ex) {
            Debug.LogError("PluginManager: " + ex.ToString());
            return false;
        }
        return true;
    }

}
