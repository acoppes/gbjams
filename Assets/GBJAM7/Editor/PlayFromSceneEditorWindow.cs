using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GBJAM7.Editor
{
    [InitializeOnLoad]
    public static class PlayFromSceneHandler
    {
        static PlayFromSceneHandler()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange playMode)
        {
            if (playMode == PlayModeStateChange.EnteredPlayMode)
            {
                var data = GameObject.FindObjectOfType<PlayFromSceneData>();
                if (data == null)
                    return;
                EditorSceneManager.LoadSceneInPlayMode(data.scenePath, new LoadSceneParameters(LoadSceneMode.Single));
            }
            
            if (playMode == PlayModeStateChange.EnteredEditMode)
            {
                var data = GameObject.FindObjectOfType<PlayFromSceneData>();
                if (data == null)
                    return;
                GameObject.DestroyImmediate(data.gameObject);
            }
        }
    }
    
    public class PlayFromSceneEditorWindow : EditorWindow
    {
        [MenuItem("Window/Play From Scene Window")]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(PlayFromSceneEditorWindow), 
                false, "Play From Scene", true);
            window.minSize = new Vector2(100, 100);
        }
        
        private void OnGUI()
        {
            var guids = AssetDatabase.FindAssets("t:scene").ToList();
            var ignoreGuids = AssetDatabase.FindAssets("t:scene l:Ignore").ToList();

            guids.RemoveAll(g => ignoreGuids.Contains(g));
            
            foreach (var sceneGuid in guids)
            {
                // TODO: could search using a tag to avoid scenes
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var name = Path.GetFileName(scenePath);
                
                if (GUILayout.Button(name))
                {
                    var playFromSceneObject = new GameObject("~PlayFromSceneData");
                    var playFromSceneData = playFromSceneObject.AddComponent<PlayFromSceneData>();
                    playFromSceneData.scenePath = scenePath;
                    EditorApplication.EnterPlaymode();
                }
            }

        }
    }
}
