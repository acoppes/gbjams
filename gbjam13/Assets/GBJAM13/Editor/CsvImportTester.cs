using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace GBJAM13.Editor
{
    public static class CsvImportTester
    {
        public const string DatabaseCvsPath = "Assets/Resources/events-database.csv";
        public const string DatabaseCsvUrl = "https://docs.google.com/spreadsheets/d/1ah4HiY2auJAIFUvCpX3j1OHHKYMAItzqYMnjiO7BuJc/export?format=csv";
            
        [MenuItem("GBJAM/GBJAM13/Download Events Database CSV")]
        public static void DownloadDatabaseFromSpreadsheet()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture; 
            
            // new HttpWebRequest();

            EditorCoroutineUtility.StartCoroutineOwnerless(DownloadCsv());
        }

        private static IEnumerator DownloadCsv()
        {
            var request = UnityWebRequest.Get(DatabaseCsvUrl);
            yield return request.SendWebRequest();
            
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    File.WriteAllText(DatabaseCvsPath, request.downloadHandler.text);
                    // Debug.Log(request.downloadHandler.text);
                    break;
                default:
                    Debug.LogError("HTTP Error: " + request.error);
                    break;
            }
        }
    }
}