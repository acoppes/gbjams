using System;
using Game;
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
        
        [EntityDefinition] 
        public Object mapSelectionDefinition;

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

                            ref var mapElementComponent = ref nodeEntity.Get<MapElementComponent>();
                            mapElementComponent.type = node.type;
                            mapElementComponent.element = node.element;
                            mapElementComponent.mainPath = node.mainPath;
                            mapElementComponent.column = i;
                            mapElementComponent.row = j;
                            
                            if (GameParameters.currentColumn == i && GameParameters.currentNode == j)
                            {
                                nodeEntity.Add(new MapShipNodeComponent());
                                // nodeEntity.Get<MapElementComponent>().current = true;
                            }
                        }
                    }

                    nodePosition.y += separation.y;
                }

                nodePosition.x += separation.x;
                nodePosition.y = 0;
            }

            if (world.TryGetSingletonEntity<MapShipNodeComponent>(out var currentNodeEntity))
            {
                var mapShipEntity = world.CreateEntity(mapShipDefinition);
                mapShipEntity.Get<PositionComponent>().value = 
                    currentNodeEntity.Get<PositionComponent>().value + 
                    currentNodeEntity.Get<MapElementComponent>().shipOffset;
            }

            var mapElementsFilter = world.GetFilter<MapElementComponent>().End();
            var currentNextNode = Entity.NullEntity;
            
            foreach (var e in mapElementsFilter)
            {
                var mapElement = world.GetComponent<MapElementComponent>(e);
                if (mapElement.column == GameParameters.currentColumn + 1)
                {
                    if (!currentNextNode)
                    {
                        currentNextNode = world.GetEntity(e);
                    }
                    else
                    {
                        if (currentNextNode.Get<MapElementComponent>().row < mapElement.row)
                        {
                            currentNextNode = world.GetEntity(e);
                        }
                    }
                }
            }
            
            var mapSelectionEntity = world.CreateEntity(mapSelectionDefinition);
            mapSelectionEntity.Get<PositionComponent>().value = currentNextNode.Get<PositionComponent>().value;
        }
        
        // IF KEY UP/DOWN => swap selection
    }
}