using UnityEngine;

namespace GBJAM7.Scripts.MainMenu
{
    public class BackgroundMusicSingleton : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
