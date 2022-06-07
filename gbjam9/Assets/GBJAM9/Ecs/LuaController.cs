using System.IO;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class LuaController : MonoBehaviour, IController
    {
        public string luaScriptPath;
        
        private Script script;

        private LuaEntity luaEntity = new ();

        public void LoadScript()
        {
            script = new Script();
            script.DoFile(
                Path.Combine(Application.streamingAssetsPath, luaScriptPath));
        }

        public void OnUpdate(float dt, Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            luaEntity.world = world;
            luaEntity.entity = entity;

            // scriptValue.Table["deltaTime"] = DynValue.NewNumber(dt);
            script.Globals["deltaTime"] = DynValue.NewNumber(dt);
            
            script.Call(script.Globals["onUpdate"], luaEntity);
            // script.Call(scriptValue.Table["onUpdate"], luaEntity);
        }
        
    }
}