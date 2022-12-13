using System;
using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

namespace Beatemup.Screens
{
    public class GameHud : MonoBehaviour
    {
        private List<PlayerPortrait> portraits = new List<PlayerPortrait>();
        
        public string playerPrefix = "Character_Player_";
        
        public string killCountEntityName = "Players_KillCount";

        public Text timerText;

        // todo move to entity
        private float playTime;
        private int seconds;
        private int minutes;

        public World world;

        public Text totalKillCountText;
        private int totalKillCount;
        
        // Start is called before the first frame update
        void Start()
        {
            GetComponentsInChildren(portraits);

            //  name = $"Character_Player_{i}",

            if (world == null)
            {
                world = World.Instance;
            }
            
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

            var targets = world.GetTargets(new TargetingUtils.RuntimeTargetingParameters()
            {
                player = 0,
                playerAllianceType = TargetingUtils.PlayerAllianceType.Allies
            });

            var newCount = 0;
            
            foreach (var target in targets)
            {
                if (world.Exists(target.entity) && world.HasComponent<KillCountComponent>(target.entity))
                {
                    newCount += world.GetComponent<KillCountComponent>(target.entity).count;
                }
            }

            if (newCount != totalKillCount)
            {
                totalKillCountText.text = $"{newCount}";
                totalKillCount = newCount;
            }
        }
    }
}
