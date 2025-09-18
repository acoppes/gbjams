using System;
using MyBox;
using UnityEngine;

namespace GBJAM13.Development
{
    public class DevGalaxyGenerator : MonoBehaviour
    {
        public GalaxyGeneratorController galaxyGeneratorController;

        public bool autoGenerateOnStart;
        
        public void Start()
        {
            if (autoGenerateOnStart)
            {
                Generate();
            }
        }

        [ButtonMethod]
        public void Generate()
        {
            galaxyGeneratorController.GenerateGalaxy();
            
            
        }
    }
}