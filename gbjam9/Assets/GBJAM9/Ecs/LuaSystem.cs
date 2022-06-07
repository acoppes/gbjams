using System.Linq;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using MoonSharp.Interpreter;
using UnityEngine;

namespace GBJAM9.Ecs
{
    public class LuaSystem : BaseSystem, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            // object[] result = Resources.LoadAll("Lua", typeof(TextAsset));
            // var scripts = result.OfType<TextAsset>().ToDictionary(ta => ta.name, ta => ta.text);
            // Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.UnityAssetsScriptLoader(scripts);
            
            UserData.RegisterType<LuaEntity>();
            UserData.RegisterType<Vector2>();
            UserData.RegisterType<UnitMovementComponent>();
        }
    }
}