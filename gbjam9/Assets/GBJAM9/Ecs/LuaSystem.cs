using System.Linq;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class LuaSystem : BaseSystem, IEcsInitSystem, IEntityCreatedHandler
    {
        public void Init(EcsSystems systems)
        {
            // object[] result = Resources.LoadAll("Lua", typeof(TextAsset));
            // var scripts = result.OfType<TextAsset>().ToDictionary(ta => ta.name, ta => ta.text);
            // Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader(scripts);

            MoonSharp.Interpreter.
            Script.DefaultOptions.ScriptLoader = new FileSystemScriptLoader();
            
            UserData.RegisterType<LuaEntity>();
            UserData.RegisterType<LuaStatesComponent>();

            UserData.RegisterType<Vector2>();
            UserData.RegisterType<UnitMovementComponent>();
            
        }

        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            if (world.HasComponent<ControllerComponent>(entity))
            {
                var controllerComponent = world.GetComponent<ControllerComponent>(entity);
                foreach (var controller in controllerComponent.controllers)
                {
                    if (controller is LuaController luaController)
                    {
                        // cache script?
                        luaController.LoadScript();
                    }
                }
            }
        }
    }
}