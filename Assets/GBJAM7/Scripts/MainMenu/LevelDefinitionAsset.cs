using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    [CreateAssetMenu(menuName="Level Definition")]
    public class LevelDefinitionAsset : ScriptableObject
    {
        public string name;
        // don't touch it unless we want another scene
        // public string sceneName = "GameScene";
        [Tooltip("GameObject with only the level")]
        public GameObject levelPrefab;
        
        [Tooltip("GameObject with unit positions, players, etc.")]
        public GameObject balancePrefab;
    }
}