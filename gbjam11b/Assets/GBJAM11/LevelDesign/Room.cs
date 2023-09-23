using System;
using UnityEngine;

namespace GBJAM11.LevelDesign
{
    public class Room : MonoBehaviour
    {
        public Transform enterPosition;
        public Transform exitPosition;

        public Room previousRoom;
        public Room nextRoom;
    }
}