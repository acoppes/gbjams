using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Beatemup.Screens
{
    public class GameHud : MonoBehaviour
    {
        private List<PlayerPortrait> portraits = new List<PlayerPortrait>();
        
        public string playerPrefix = "Character_Player_";

        public Text timerText;

        // todo move to entity
        private float playTime;
        private int seconds;
        private int minutes;
        
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

            timerText.text = "00:00";
        }

        private void FixedUpdate()
        {
            playTime += Time.deltaTime;

            var time = new TimeSpan(0, 0, 0, Mathf.RoundToInt(playTime));

            if (time.Seconds != seconds || time.Minutes != minutes)
            {
                seconds = time.Seconds;
                minutes = time.Minutes;
                
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
    }
}
