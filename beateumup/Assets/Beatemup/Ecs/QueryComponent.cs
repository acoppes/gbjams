using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;

namespace Beatemup.Ecs
{
    public struct QueryComponent : IEntityComponent
    {
        public string name;
        public int player;
    }

    public struct Query
    {
        public bool checkName;
        public string name;

        public bool checkPlayer;
        public int player;

        public Query CheckName(string name)
        {
            checkName = true;
            this.name = name;
            return this;
        }
        
        public Query CheckPlayer(int player)
        {
            checkPlayer = true;
            this.player = player;
            return this;
        }
    }

    public static class QueryUtils
    {
        public static bool Query(this World world, Query query, List<Entity> results = null)
        {
            var queryComponents = world.GetComponents<QueryComponent>();
            var found = 0;
            
            foreach (var entity in world.GetFilter<QueryComponent>().End())
            {
                var queryComponent = queryComponents.Get(entity);
                
                if (query.checkPlayer)
                {
                    if (queryComponent.player != query.player)
                        continue;
                }
                
                if (query.checkName)
                {
                    if (!query.name.Equals(queryComponent.name, StringComparison.OrdinalIgnoreCase))
                        continue;
                }

                found++;

                results?.Add(world.GetEntity(entity));
            }

            return found > 0;
        } 
    }
}