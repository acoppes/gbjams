using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM10.Components;
using UnityEngine;

namespace GBJAM10.Controllers
{
    public class RoomExitRewardView : MonoBehaviour
    {
        [Serializable]
        public class RewardIcon
        {
            public string name;
            public Sprite sprite;
        }
        
        public Entity entity;

        public SpriteRenderer renderer;

        public List<RewardIcon> rewardIcons;
        
        // Update is called once per frame
        private void LateUpdate()
        {
            if (rewardIcons == null || rewardIcons.Count == 0)
                return;

            var reward = entity.roomExit.rewardType;
            if (string.IsNullOrEmpty(reward))
            {
                return;
            }

            var rewardIcon = rewardIcons.FirstOrDefault(r => r.name.Equals(entity.roomExit.rewardType));
            if (rewardIcon != null)
            {
                renderer.sprite = rewardIcon.sprite;
            }
        }
    }
}
