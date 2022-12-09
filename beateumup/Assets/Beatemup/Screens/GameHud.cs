using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Beatemup.Screens
{
    public class GameHud : MonoBehaviour
    {
        private List<PlayerPortrait> portraits = new List<PlayerPortrait>();
        
        public string playerPrefix = "Character_Player_";
        
        // Start is called before the first frame update
        void Start()
        {
            GetComponentsInChildren(portraits);

            //  name = $"Character_Player_{i}",

            var world = World.Instance;

            for (int i = 0; i < portraits.Count; i++)
            {
                portraits[i].playerEntityName = $"{playerPrefix}{i}";
                portraits[i].world = world;
            }
        }
    }
}
