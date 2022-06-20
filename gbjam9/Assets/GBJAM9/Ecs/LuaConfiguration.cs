using System.IO;
using GBJAM9.Ecs;
using Gemserk.Leopotam.Ecs;
using MoonSharp.Interpreter;
using UnityEngine;

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
    
    public void Configure(World world, Entity entity)
    {
        luaEntity.world = world;
        luaEntity.entity = entity;
        script.Call(script.Globals["Configure"], luaEntity);
    }
}