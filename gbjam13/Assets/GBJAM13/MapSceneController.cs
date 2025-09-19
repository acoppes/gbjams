using System;
using System.Collections.Generic;
using Game;
using Game.Scenes;
using GBJAM13.Components;
using GBJAM13.UI;
using Gemserk.Leopotam.Ecs;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;
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

        public InputActionReference upAction;
        public InputActionReference downAction;

        public UIMapSelection uiMapSelection;
        
        private Entity mapSelectionEntity;
        private Entity mapDestinationEntity;
        private List<Entity> nextColumnEntities = new List<Entity>();
        
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
                            mapElementComponent.name = node.name;
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
            mapDestinationEntity = Entity.NullEntity;
            
            foreach (var e in mapElementsFilter)
            {
                var mapElement = world.GetComponent<MapElementComponent>(e);
                if (mapElement.column == GameParameters.currentColumn + 1)
                {
                    nextColumnEntities.Add(world.GetEntity(e));
                    
                    if (!mapDestinationEntity)
                    {
                        mapDestinationEntity = world.GetEntity(e);
                    }
                    else
                    {
                        if (mapDestinationEntity.Get<MapElementComponent>().row < mapElement.row)
                        {
                            mapDestinationEntity = world.GetEntity(e);
                        }
                    }
                }
            }
            
            mapSelectionEntity = world.CreateEntity(mapSelectionDefinition);
        }
        
        // IF KEY UP/DOWN => swap selection

        private void Update()
        {
            if (nextColumnEntities.Count == 0)
            {
                return;
            }
            
            var currentIndex = nextColumnEntities.IndexOf(mapDestinationEntity);
            
            if (upAction.action.WasPerformedThisFrame())
            {
                currentIndex++;
                if (currentIndex >= nextColumnEntities.Count)
                {
                    currentIndex = 0;
                }
            }

            if (downAction.action.WasPerformedThisFrame())
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = nextColumnEntities.Count - 1;
                }
            }

            mapDestinationEntity = nextColumnEntities[currentIndex];
            
            mapSelectionEntity.Get<PositionComponent>().value = mapDestinationEntity.Get<PositionComponent>().value;
            
            uiMapSelection.SetSelectedElementData(mapDestinationEntity.Get<MapElementComponent>().name);
        }
    }
}