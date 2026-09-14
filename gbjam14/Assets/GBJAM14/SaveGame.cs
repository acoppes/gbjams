using System;
using System.Collections.Generic;
using GBJAM14.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM14
{
    [Serializable]
    public class SavegameRoomData
    {
        public string currentRoom;
        public string currentStart;
    }
    
    [Serializable]
    public class SavegameData
    {
        public List<string> items = new List<string>();
        public SavegameRoomData roomData = new SavegameRoomData();
    }
    
    public class SaveGame
    {
        public const int Version = 1;
        public const string DefaultSavePath = "savegame.json";

        public static SaveGame saveGame = new SaveGame();
        public SavegameData data = new SavegameData();

        public void Delete()
        {
            var fileStorageService = Object.FindFirstObjectByType<FileStorageService>();
            fileStorageService.DeleteFile(DefaultSavePath);
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(data);
            var fileStorageService = Object.FindFirstObjectByType<FileStorageService>();
            fileStorageService.SaveTextToFile(DefaultSavePath, json);
        }

        public void Load()
        {
            var fileStorageService = Object.FindFirstObjectByType<FileStorageService>();
            var fileContents = fileStorageService.LoadFileAsText(DefaultSavePath);
            
            if (string.IsNullOrEmpty(fileContents))
            {
                data = new SavegameData();
                return;
            }

            try
            {
                var jObject = JObject.Parse(fileContents);
                if (jObject.ContainsKey("version"))
                {
                    var savedVersion = jObject["version"].Value<int>();
                    if (savedVersion != Version)
                    {
                        data = new SavegameData();
                        return;
                    } 
                }
                data = jObject.ToObject<SavegameData>();
            }
            catch (Exception e) 
            {
                Debug.LogWarning(e);
                data = new SavegameData();
            }
        }
    }
}