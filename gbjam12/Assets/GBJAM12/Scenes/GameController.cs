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

                for (var i = 0; i < lanes.Count; i++)
                {
                    danceMoves.incomingNotes[i] = new IncomingNote()
                    {
                        hasIncomingNote = lanes[i].hasNotePlaying,
                        distanceToEndInTicks = lanes[i].distanceToEndNoteInTicks
                        // durationInTicks = 960
                    };
                }
            }
        }
    }
}