using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using Gemserk;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using AssetDatabaseExt = Gemserk.RefactorTools.Editor.AssetDatabaseExt;

namespace GBJAM14.Editor
{
    public static class DownloadDatabaseCsvUtils
    {
        [MenuItem("GBJAM/GBJAM14/Download Dialogs CSV")]
        public static void DownloadDatabaseFromSpreadsheet()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture; 

            var downloadDatabaseAssets = AssetDatabaseExt.FindAssets<DownloadDatabaseAsset>();
            foreach (var downloadDatabaseAsset in downloadDatabaseAssets)
            {
                EditorCoroutineUtility.StartCoroutineOwnerless(DownloadCsv(downloadDatabaseAsset.spreadsheetUrl, 
                    downloadDatabaseAsset.outputPath));
                AssetDatabase.Refresh();
            }
        }

        private static IEnumerator DownloadCsv(string url, string outputPath)
        {
            var request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    File.WriteAllText(outputPath, request.downloadHandler.text);
                    Debug.Log("DOWNLOADED DATABASE");
                    break;
                default:
                    Debug.LogError("HTTP Error: " + request.error);
                    break;
            }
        }
    }
}