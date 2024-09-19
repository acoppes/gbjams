using System;
using System.Linq;
using GBJAM12.Utilities;
using UnityEngine;

namespace GBJAM12
{
    public class GameTrackAsset : MonoBehaviour
    {
        [Serializable]
        public class GameTrackLane
        {
            public string track;
            public string notes;

            public int[] GetNotesArray()
            {
                return notes.Split(",").Select(int.Parse).ToArray();
            }
        }

        public AudioClip song;
        public MidiDataAsset midi;
        public GameObject audioSourcePrefab;

        public GameTrackLane[] lanes;
    }
}