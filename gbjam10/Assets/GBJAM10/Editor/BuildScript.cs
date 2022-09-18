using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GBJAM10.Editor
{
    public class BuildScript
    {
        public static void BuildWebGL()
        {
            var buildPath = Environment.GetEnvironmentVariable("GB_GAME_BUILD_PATH");
            
            if (string.IsNullOrEmpty(buildPath))
            {
                Debug.Log("Failed to build, missing build path");
                return;
            }

            var buildOptions = new BuildPlayerOptions
            {
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
                options = BuildOptions.None,
                locationPathName = buildPath,
                scenes = EditorBuildSettings.scenes.Select(s => s.path).ToArray()
            };

            BuildPipeline.BuildPlayer(buildOptions);
        }
    }
}