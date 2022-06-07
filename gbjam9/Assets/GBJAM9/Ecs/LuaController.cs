using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class LuaController : MonoBehaviour, IController
    {
        public TextAsset luaScript;
        
        private Script script;

        private LuaEntity luaEntity = new ();

        public void OnUpdate(float dt, Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            if (script == null)
            {
                script = new Script();
                script.DoString(luaScript.text);
            }
            
            luaEntity.world = world;
            luaEntity.entity = entity;

            script.Globals["deltaTime"] = DynValue.NewNumber(dt);
            
            script.Call(script.Globals["onUpdate"], luaEntity);
        }
        
    }
}