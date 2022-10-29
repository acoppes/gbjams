using System.IO;
using System.Linq;
using GBJAM.Commons;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
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

        private Vector2 scroll;
        
        private void OnGUI()
        {
            var guids = AssetDatabase.FindAssets("t:scene").ToList();
            var ignoreGuids = AssetDatabase.FindAssets("t:scene l:Ignore").ToList();

            guids.RemoveAll(g => ignoreGuids.Contains(g));

            scroll = EditorGUILayout.BeginScrollView(scroll);
            
            foreach (var sceneGuid in guids)
            {
                // TODO: could search using a tag to avoid scenes
                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);
                var name = Path.GetFileName(scenePath);

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name);
                
                if (GUILayout.Button("Open"))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                }
                
                if (GUILayout.Button("Add"))
                {
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
                
                if (GUILayout.Button("Play"))
                {
                    var playFromSceneObject = new GameObject("~PlayFromSceneData");
                    var playFromSceneData = playFromSceneObject.AddComponent<PlayFromSceneData>();
                    playFromSceneData.scenePath = scenePath;
                    EditorApplication.EnterPlaymode();
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndScrollView();

        }
    }
}
