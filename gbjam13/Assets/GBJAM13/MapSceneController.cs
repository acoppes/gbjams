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
        
        [EntityDefinition] 
        public Object mapShipDefinition;

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

            var currentNodeEntity = Entity.NullEntity;

            for (var i = 0; i < generatedGalaxy.columns.Length; i++)
            {
                var column = generatedGalaxy.columns[i];
                for (var j = 0; j < column.nodes.Length; j++)
                {
                    var node = column.nodes[j];
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

                            if (GameParameters.currentColumn == i && GameParameters.currentNode == j)
                            {
                                // nodeEntity.Add(new MapShipNodeComponent());
                                currentNodeEntity = nodeEntity;
                            }
                        }
                    }

                    nodePosition.y += separation.y;
                }

                nodePosition.x += separation.x;
                nodePosition.y = 0;
            }

            if (currentNodeEntity)
            {
                var mapShipEntity = world.CreateEntity(mapShipDefinition);
                mapShipEntity.Get<PositionComponent>().value = 
                    currentNodeEntity.Get<PositionComponent>().value + 
                    currentNodeEntity.Get<MapElementComponent>().shipOffset;
            }
        }
    }
}