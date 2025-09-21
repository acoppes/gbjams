using Game.Screens;
using Gemserk.BitmaskTypes;
using UnityEngine;

namespace GBJAM13.UI
{
    public class UIResource : MonoBehaviour
    {
        public IntTypeAsset resourceType;

        public TextView uiNumber;
        
        public void SetValue(int value)
        {
            uiNumber.SetText(value);
        }
    }
}