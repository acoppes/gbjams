using System;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM13
{
    public class GalaxyGeneratorController : MonoBehaviour
    {
        public WorldReference worldReference;
        
        public GalaxyGenerator.GalaxyGeneratorData data;
        public int totalJumps;
        
        [EntityDefinition]
        public Object wormHoleDefinition;

        [EntityDefinition] 
        public Object mapPlanetDefinition;

        public Vector2 separation;
        
        public void GenerateGalaxy()
        {
            var galaxyGenerator = new GalaxyGenerator();
            var generatedGalaxy = galaxyGenerator.GenerateGalaxy(data, totalJumps);

            var world = worldReference.GetReference(gameObject);
            var nodePosition = new Vector2();
            
            foreach (var column in generatedGalaxy.columns)
            {
                foreach (var node in column.nodes)
                {
                    if (node != null)
                    {
                        if (node.type.Equals("wormhole", StringComparison.OrdinalIgnoreCase))
                        {
                            var nodeEntity = world.CreateEntity(wormHoleDefinition);
                            nodeEntity.Get<PositionComponent>().value = transform.position.ToVector2() + nodePosition;
                        }
                        
                        if (node.type.Equals("planet", StringComparison.OrdinalIgnoreCase))
                        {
                            var nodeEntity = world.CreateEntity(mapPlanetDefinition);
                            nodeEntity.Get<PositionComponent>().value = transform.position.ToVector2() + nodePosition;
                        }
                    }

                    nodePosition.y += separation.y;
                }

                nodePosition.x += separation.x;
                nodePosition.y = 0;
            }
        }
    }
}