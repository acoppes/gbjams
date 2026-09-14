using System;
using System.Collections.Generic;

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

        public static SaveGame saveGame = new SaveGame();

        public SavegameData data = new SavegameData();

        public void Save()
        {
            
        }

        public void Load()
        {
            
        }
    }
}