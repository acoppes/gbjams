using System;
using UnityEngine;

namespace Beatemup.Definitions
{
    [Serializable]
    public class CameraShake
    {
        public float duration;
        public Vector2 magnitude;
        public AnimationCurve decay = AnimationCurve.Linear(0, 1, 1, 0);
    }
    
    [CreateAssetMenu(menuName = "Tools/Create Camera Shake", fileName = "CameraShake", order = 0)]
    public class CameraShakeAsset : ScriptableObject
    {
        public CameraShake shake;
    }
}