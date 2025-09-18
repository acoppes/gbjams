using System;
using Game.Scenes;
using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GBJAM13
{
    public class MapSceneController : MonoBehaviour
    {
        public WorldReference worldReference;
        
        [EntityDefinition] 
        public Object mapPlanetDefinition;

        public Vector2 separation;
        
        public void GenerateMapFromData()
        {
            var generatedGalaxy = GameParameters.galaxyData;

            if (GameParameters.galaxyData == null)
            {
                GameParameters.totalJumps = GameParameters.DefaultTotalJumps;
                GameSceneLoader.LoadNextScene("MapGenerator");
                return;
            }
            
            var world = worldReference.GetReference(gameObject);
            var nodePosition = new Vector2();
            
            foreach (var column in generatedGalaxy.columns)
            {
                foreach (var node in column.nodes)
                {
                    if (node != null)
                    {
                        if (!string.IsNullOrEmpty(node.type) &&
                            !node.type.Equals("empty", StringComparison.OrdinalIgnoreCase))
                        {
                            var nodeEntity = world.CreateEntity(mapPlanetDefinition);
                            nodeEntity.Get<PositionComponent>().value = transform.position.ToVector2() + nodePosition;
                            
                            nodeEntity.Get<MapElementComponent>().type = node.type;
                            nodeEntity.Get<MapElementComponent>().element = node.element;
                            nodeEntity.Get<MapElementComponent>().mainPath = node.mainPath;
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