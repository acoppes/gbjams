using System.Collections.Generic;
using Game;
using GBJAM12.Components;
using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace GBJAM12.Scenes
{
    public class GameController : MonoBehaviour
    {
        public WorldReference worldReference;
        public List<MusicLane> lanes;

        public int distanceToPlayInTicks = 240;

        private void Update()
        {
            var world = worldReference.GetReference(gameObject);

            if (world.TryGetSingletonEntity<DanceMovesComponent>(out var danceMovesEntity))
            {
                ref var danceMoves = ref danceMovesEntity.Get<DanceMovesComponent>();
                danceMoves.d1 = lanes[0].distanceToClosestIncomingNote < distanceToPlayInTicks;
                danceMoves.d2 = lanes[1].distanceToClosestIncomingNote < distanceToPlayInTicks;
                danceMoves.d3 = lanes[2].distanceToClosestIncomingNote < distanceToPlayInTicks;
            }
        }
    }
}