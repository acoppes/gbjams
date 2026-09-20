using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using GBJAM14.Systems;
using UnityEngine;
using UnityEngine.Networking;
using yutokun;

namespace GBJAM14
{
    public class DialogsFromCsvLoader : MonoBehaviour
    {
        public static bool DebugParseData = true;
        
        public CharacterDialogsDB dialogsDB;
        public string dialogsDatabasePath;
        
        private void Awake()
        {
            var databaseFilePath = Path.Combine(Application.streamingAssetsPath, dialogsDatabasePath);
            StartCoroutine(LoadDatabaseFile($"file://{databaseFilePath}"));
        }
    
        private IEnumerator LoadDatabaseFile(string path)
        {
            var request = UnityWebRequest.Get(path);
        
            yield return request.SendWebRequest();
        
            if (request.result == UnityWebRequest.Result.Success)
            {
                LoadDataFromCsv(request.downloadHandler.text);
            }
            else
            {
                Debug.LogError($"FAILED TO LOAD EVENTS DATABASE: {request.responseCode} {request.error}");
            }
        }
        
        private void LoadDataFromCsv(string csvText)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            
            var time = Time.realtimeSinceStartupAsDouble;
            
            var results = CSVParser.LoadFromString(csvText);
            
            // ignore first row which are normally the headers...
            for (var i = 1; i < results.Count; i++)
            {
                var row = results[i];

                var id = row[0];
                var type = row[1];
                var characterId = row[2];
                var option = row[3];
                var texts = row[4].Split('\n');
                var requirements = row[5];
                var outputs = row[6];
                var extraCharacters = row[7];
                
                if (string.IsNullOrEmpty(id))
                    continue;

                // var extraCharacters = row[5];

                var dialogData = new CharacterDialogsDB.DialogData()
                {
                    id = id,
                    type = int.Parse(type),
                    option = option,
                    characterId = characterId,
                    texts = texts.ToList(),
                    requirements = requirements.Split(','),
                    output = outputs.Split(','),
                    extraCharacters = extraCharacters.Split(',')
                };
                
                dialogsDB.AddDialogData(dialogData);
                
                if (DebugParseData)
                {
                    Debug.Log(JsonUtility.ToJson(dialogData));
                }
            }
            
            var completeTime = Time.realtimeSinceStartupAsDouble - time;
            
            Debug.Log($"CSV PARSE AND BUILD EVENTS: {completeTime} seconds");
        }
    }
}