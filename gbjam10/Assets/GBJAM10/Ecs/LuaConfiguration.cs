using System.IO;
using Gemserk.Leopotam.Ecs;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GBJAM10.Ecs
{
    public class LuaConfiguration : MonoBehaviour, IConfiguration
    {
        public string luaScriptPath;
        
        private Script script;

        private LuaEntity luaEntity = new ();

        private Gemserk.Leopotam.Ecs.World world;
        private Entity entity;

        public void LoadScript()
        {
            script = new Script();
            script.DoFile(
                Path.Combine(Application.streamingAssetsPath, luaScriptPath));
        }
    
        public void Configure(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            luaEntity.world = world;
            luaEntity.entity = entity;
            script.Call(script.Globals["Configure"], luaEntity);
        }
    }
}