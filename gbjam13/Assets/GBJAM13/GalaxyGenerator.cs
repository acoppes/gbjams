using System;
using MyBox;

namespace GBJAM13
{
    public class GalaxyData
    {
        public class GalaxyNode
        {
            public string type;
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
            public string wormHoleType;
            public string[] otherTypes;
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
                type = generatorData.wormHoleType
            };
            
            endingColumn.nodes[UnityEngine.Random.Range(0, GalaxyData.GalaxyColumn.NodesPerColumn)] = new GalaxyData.GalaxyNode()
            {
                type = generatorData.wormHoleType
            };
            
            for (var i = 1; i < galaxy.columns.Length - 1; i++)
            {
                var column = galaxy.columns[i];
                for (var j = 0; j < GalaxyData.GalaxyColumn.NodesPerColumn; j++)
                {
                    column.nodes[j] = new GalaxyData.GalaxyNode()
                    {
                        type = generatorData.otherTypes.GetRandom()
                    };
                }
            }

            return galaxy;
        }
    }
}