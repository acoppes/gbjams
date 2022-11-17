using System.IO;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Gameplay.Controllers;
using Gemserk.Leopotam.Gameplay.Events;
using MoonSharp.Interpreter;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class LuaController : MonoBehaviour, IController, IInit, IConfigurable
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

        public void Bind(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            this.world = world;
            this.entity = entity;
        }

        public void OnUpdate(float dt)
        {
            luaEntity.world = world;
            luaEntity.entity = entity;

            // scriptValue.Table["deltaTime"] = DynValue.NewNumber(dt);
            script.Globals["deltaTime"] = DynValue.NewNumber(dt);
            
            script.Call(script.Globals["OnUpdate"], luaEntity);
            // script.Call(scriptValue.Table["onUpdate"], luaEntity);
        }

        public void OnInit()
        {
            luaEntity.world = world;
            luaEntity.entity = entity;
            script.Call(script.Globals["OnInit"], luaEntity);
        }

        public void OnConfigured()
        {
            luaEntity.world = world;
            luaEntity.entity = entity;
            script.Call(script.Globals["OnConfigured"], luaEntity);
        }
    }
}