using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;

namespace Beatemup.Editor
{
    public static class DevelopmentTools
    {
        [MenuItem("Tools/Create Folder for Current Scene")]
        public static void CreateFolderForCurrentScene()
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            if (activeScene == null)
            {
                return;
            }

            var sceneDirectory = Path.Combine(Path.GetDirectoryName(activeScene.path), 
                activeScene.name);

            if (!Directory.Exists(sceneDirectory))
            {
                // Debug.Log($"Creating {sceneDirectory}");
                Directory.CreateDirectory(sceneDirectory);
                AssetDatabase.Refresh();
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(sceneDirectory));
            }
            
            // AssetDatabase.GetAssetPath(activeScene.path);
            
        }
    }
}