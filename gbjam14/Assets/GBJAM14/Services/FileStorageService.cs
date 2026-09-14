using System.IO;
using UnityEngine;

namespace GBJAM14.Services
{
    public class FileStorageService : MonoBehaviour, IStorageService
    {
        private bool autoCreateFile = true;
        
        public void SaveTextToFile(string path, string contents)
        {
            var finalPath = Path.Combine(Application.persistentDataPath, path);
            File.WriteAllText(finalPath, contents);
        }

        public string LoadFileAsText(string path)
        {
            var finalPath = Path.Combine(Application.persistentDataPath, path);
            if (!File.Exists(finalPath))
            {
                if (autoCreateFile)
                {
                    File.WriteAllText(finalPath, "");
                }
                else
                {
                    throw new FileNotFoundException(path);
                }
            }
            var contents = File.ReadAllText(finalPath);
            return contents;
        }

        public void DeleteFile(string path)
        {
            var finalPath = Path.Combine(Application.persistentDataPath, path);
            File.Delete(finalPath);
        }
    }
}