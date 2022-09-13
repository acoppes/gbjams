using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Leopotam.EcsLite;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using UnityEngine;

namespace GBJAM10.Ecs
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
            Script.DefaultOptions.DebugPrint = s => Debug.Log(s.ToLower());
            
            UserData.RegisterType<LuaEntity>();
            UserData.RegisterType<LuaStatesComponent>();
            UserData.RegisterType<LuaAbilitiesComponent>();
            UserData.RegisterType<LuaAbility>();
            UserData.RegisterType<LuaTarget>();

            UserData.RegisterType<Vector2>();
            UserData.RegisterType<UnitMovementComponent>();
            
        }

        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            if (world.HasComponent<ConfigurationComponent>(entity))
            {
                var configurationComponent = world.GetComponent<ConfigurationComponent>(entity);

                if (configurationComponent.configuration is LuaConfiguration luaConfiguration)
                {
                    luaConfiguration.LoadScript();
                }
            }
            
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