using GBJAM14.UI;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Serialization;

namespace GBJAM14.Scenes
{
    public class GameSceneController : MonoBehaviour
    {
        public WorldReference worldReference;

        [FormerlySerializedAs("dialog")] 
        public UIDialog uiDialog;
        public UIOptions uiOptions;
      
        public void StartGame()
        {
           
        }
    }
}