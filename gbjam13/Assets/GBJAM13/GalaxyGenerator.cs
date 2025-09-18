using System;
using MyBox;
using UnityEngine;

namespace GBJAM13
{
    public class GalaxyData
    {
        public class GalaxyNode
        {
            public string type;
            public string element;
            public bool disableTravel;
        }

        public class GalaxyColumn
        {
            public const int NodesPerColumn = 5;
            public GalaxyNode[] nodes = new GalaxyNode[NodesPerColumn];
        }
        
        public int totalJumps;

        public GalaxyColumn[] columns;
    }
    
    public class GalaxyGenerator
    {
        
        [Serializable]
        public class GalaxyGeneratorData
        {
            [Serializable]
            public class NodeData
            {
                public string type;
                public bool disableTravel;
                
                public string[] element;
            }
            
            public NodeData wormHoleType;
            public NodeData[] otherTypes;

            public int maxColumnDistance;
        }
        
        public GalaxyData GenerateGalaxy(GalaxyGeneratorData generatorData, int totalJumps)
        {
            var galaxy = new GalaxyData
            {
                totalJumps = totalJumps,
                columns = new GalaxyData.GalaxyColumn[totalJumps + 1]
            };

            for (var i = 0; i < galaxy.columns.Length; i++)
            {
                galaxy.columns[i] = new GalaxyData.GalaxyColumn()
                {
                    nodes = new GalaxyData.GalaxyNode[GalaxyData.GalaxyColumn.NodesPerColumn]
                };
            }
            
            var startingColumn = galaxy.columns[0];
            var endingColumn = galaxy.columns[^1];

            startingColumn.nodes[UnityEngine.Random.Range(0, GalaxyData.GalaxyColumn.NodesPerColumn)] = new GalaxyData.GalaxyNode()
            {
                type = generatorData.wormHoleType.type,
                element =  generatorData.wormHoleType.element[0]
            };
            
            endingColumn.nodes[UnityEngine.Random.Range(0, GalaxyData.GalaxyColumn.NodesPerColumn)] = new GalaxyData.GalaxyNode()
            {
                type = generatorData.wormHoleType.type,
                element =  generatorData.wormHoleType.element[0]
            };
            
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                var column = galaxy.columns[i];
                for (var j = 0; j < GalaxyData.GalaxyColumn.NodesPerColumn; j++)
                {
                    var nodeData = generatorData.otherTypes.GetRandom();
                    
                    column.nodes[j] = new GalaxyData.GalaxyNode()
                    {
                        type = nodeData.type,
                        element = nodeData.element.GetRandom(),
                        disableTravel = nodeData.disableTravel
                    };
                }
            }
            
            // prune backwards
            
            // for (var i = galaxy.columns.Length - 1; i > 0; i--)
            // {
            //     var column = galaxy.columns[i];
            //     var previousColumn = galaxy.columns[i];
            //
            //     for (var j = 0; j < GalaxyData.GalaxyColumn.NodesPerColumn; j++)
            //     {
            //         var hasConnectionFromPreviousColumn = false;
            //         
            //         for (var k = j-generatorData.maxColumnDistance; k < j+generatorData.maxColumnDistance; k++)
            //         {
            //             // k outside valid ranges
            //             if (k < 0 || k >= GalaxyData.GalaxyColumn.NodesPerColumn)
            //                 continue;
            //
            //             if (previousColumn.nodes[k] == null)
            //             {
            //                 continue;
            //             }
            //             
            //             if (!previousColumn.nodes[k].disableTravel)
            //             {
            //                 hasConnectionFromPreviousColumn = true;
            //                 break;
            //             }
            //         }
            //
            //         if (!hasConnectionFromPreviousColumn)
            //         {
            //             Debug.Log($"NODE [{i},{j}] PRUNED BECAUSE THERE WAS NO TRAVEL AVAILABLE");
            //             column.nodes[j] = null;
            //         }
            //     }
            // }
            
            // prune forward

            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                var column = galaxy.columns[i];
                var previousColumn = galaxy.columns[i - 1];

                for (var j = 0; j < GalaxyData.GalaxyColumn.NodesPerColumn; j++)
                {
                    var hasConnectionFromPreviousColumn = false;
                    
                    for (var k = j-generatorData.maxColumnDistance; k < j+generatorData.maxColumnDistance; k++)
                    {
                        // k outside valid ranges
                        if (k < 0 || k >= GalaxyData.GalaxyColumn.NodesPerColumn)
                            continue;

                        if (previousColumn.nodes[k] == null)
                        {
                            continue;
                        }
                        
                        if (!previousColumn.nodes[k].disableTravel)
                        {
                            hasConnectionFromPreviousColumn = true;
                            break;
                        }
                    }

                    if (!hasConnectionFromPreviousColumn)
                    {
                        Debug.Log($"NODE [{i},{j}] PRUNED BECAUSE THERE WAS NO TRAVEL AVAILABLE");
                        column.nodes[j] = null;
                    }
                }
            }

            return galaxy;
        }
    }
}