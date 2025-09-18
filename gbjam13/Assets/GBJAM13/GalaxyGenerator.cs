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
            public bool mainPath;
        }

        public class GalaxyColumn
        {
            public const int RowsPerColumn = 5;
            public GalaxyNode[] nodes = new GalaxyNode[RowsPerColumn];
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
                public string[] element;
            }
            
            public NodeData wormHoleType;
            public NodeData[] otherTypes;

            public int maxColumnDistance;
            public float emptyChance;
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
                    nodes = new GalaxyData.GalaxyNode[GalaxyData.GalaxyColumn.RowsPerColumn]
                };
            }
            
            var startingColumn = galaxy.columns[0];
            var endingColumn = galaxy.columns[^1];

            var startingRow = UnityEngine.Random.Range(0, GalaxyData.GalaxyColumn.RowsPerColumn);
            var endingRow = UnityEngine.Random.Range(0, GalaxyData.GalaxyColumn.RowsPerColumn);
            
            startingColumn.nodes[startingRow] = new GalaxyData.GalaxyNode()
            {
                type = generatorData.wormHoleType.type,
                element =  generatorData.wormHoleType.element[0],
                mainPath = true
            };
            
            endingColumn.nodes[endingRow] = new GalaxyData.GalaxyNode()
            {
                type = generatorData.wormHoleType.type,
                element =  generatorData.wormHoleType.element[0],
                mainPath = true
            };
            
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                var column = galaxy.columns[i];
                for (var j = 0; j < GalaxyData.GalaxyColumn.RowsPerColumn; j++)
                {
                    var nodeData = generatorData.otherTypes.GetRandom();
                    
                    column.nodes[j] = new GalaxyData.GalaxyNode()
                    {
                        type = nodeData.type,
                        element = nodeData.element.GetRandom(),
                        mainPath = false
                    };
                }
            }
            
            // Calculate main path from start to end and mark it

            var totalTries = 100;
            
            var lastMainPathRow = GenerateMainPath(galaxy, startingRow, endingRow, generatorData.maxColumnDistance);

            while (Mathf.Abs(lastMainPathRow - endingRow) > generatorData.maxColumnDistance)
            {
                if (totalTries < 0)
                {
                    break;
                }
                
                lastMainPathRow = GenerateMainPath(galaxy, startingRow, endingRow, generatorData.maxColumnDistance);
                totalTries--;
            }

            if (totalTries == 0)
            {
                throw new Exception("FAILED TO GENERATE LEVEL");
            }
            
            RemoveRandomNodes(galaxy, generatorData.emptyChance);
            
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

            PruneNodes(galaxy, generatorData.maxColumnDistance, true);
            PruneNodes(galaxy, generatorData.maxColumnDistance, false);

            return galaxy;
        }

        private int GenerateMainPath(GalaxyData galaxy, int startingRow, int endingRow, int maxColumnDistance)
        {
            var currentRow = startingRow;
            
            // Clear main math first...
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                foreach (var galaxyNode in galaxy.columns[i].nodes)
                {
                    galaxyNode.mainPath = false;
                }
            }
            
            // Mark a new main path
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                var nextRow = currentRow +
                              UnityEngine.Random.Range(-maxColumnDistance, maxColumnDistance + 1);
                nextRow = Mathf.Clamp(nextRow, 0, galaxy.columns[i].nodes.Length - 1);

                if (nextRow < 0 || nextRow >= galaxy.columns[i].nodes.Length)
                {
                    Debug.Log("ERROR");
                }
                galaxy.columns[i].nodes[nextRow].mainPath = true;
                currentRow = nextRow;
            }

            return currentRow;
        }
        
        private void RemoveRandomNodes(GalaxyData galaxy, float chance)
        {
            // Clear main math first...
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                for (var j = 0; j < galaxy.columns[i].nodes.Length; j++)
                {
                    var galaxyNode = galaxy.columns[i].nodes[j];
                    if (galaxyNode.mainPath)
                    {
                        continue;
                    }

                    if (UnityEngine.Random.Range(0f, 1f) < chance)
                    {
                        Debug.Log($"NODE [{i},{j}] REMOVED BECAUSE EMPTY CHANCE");
                        galaxy.columns[i].nodes[j] = null;
                    }
                }
            }
        }

        public void PruneNodes(GalaxyData galaxy, int maxColumnDistance, bool forward)
        {
            if (forward)
            {
                for (var i = 1; i < galaxy.columns.Length - 1; i++)
                {
                    var column = galaxy.columns[i];
                    var previousColumn = galaxy.columns[i - 1];
            
                    for (var j = 0; j < GalaxyData.GalaxyColumn.RowsPerColumn; j++)
                    {
                        var hasConnection = false;
                    
                        for (var k = j - maxColumnDistance; k <= j + maxColumnDistance; k++)
                        {
                            // k outside valid ranges
                            if (k < 0 || k >= GalaxyData.GalaxyColumn.RowsPerColumn)
                                continue;
            
                            if (previousColumn.nodes[k] == null)
                            {
                                continue;
                            }
                        
                            hasConnection = true;
                            break;
                        }
            
                        if (!hasConnection)
                        {
                            Debug.Log($"NODE [{i},{j}] PRUNED BECAUSE THERE WAS NO TRAVEL AVAILABLE");
                            column.nodes[j] = null;
                        }
                    }
                }
            }
            else
            {
                for (var i = galaxy.columns.Length - 2; i > 0; i--)
                {
                    var column = galaxy.columns[i];
                    var nextColumn = galaxy.columns[i + 1];
            
                    for (var j = 0; j < GalaxyData.GalaxyColumn.RowsPerColumn; j++)
                    {
                        var hasConnection = false;
                    
                        for (var k = j - maxColumnDistance; k <= j + maxColumnDistance; k++)
                        {
                            // k outside valid ranges
                            if (k < 0 || k >= GalaxyData.GalaxyColumn.RowsPerColumn)
                                continue;
            
                            if (nextColumn.nodes[k] == null)
                            {
                                continue;
                            }
                        
                            hasConnection = true;
                            break;
                        }
            
                        if (!hasConnection)
                        {
                            Debug.Log($"NODE [{i},{j}] PRUNED BECAUSE THERE WAS NO TRAVEL AVAILABLE");
                            column.nodes[j] = null;
                        }
                    }
                }
            }
        }
        
    }
}