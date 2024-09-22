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

        private void Update()
        {
            var world = worldReference.GetReference(gameObject);

            if (world.TryGetSingletonEntity<DanceMovesComponent>(out var danceMovesEntity))
            {
                ref var danceMoves = ref danceMovesEntity.Get<DanceMovesComponent>();
                danceMoves.d1 = lanes[0].hasNoteInSkull;
                danceMoves.d2 = lanes[1].hasNoteInSkull;
                danceMoves.d3 = lanes[2].hasNoteInSkull;
            }
        }
    }
}