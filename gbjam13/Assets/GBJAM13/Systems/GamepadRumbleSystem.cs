using GBJAM13.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.InputSystem;

namespace GBJAM13.Systems
{
    public class GamepadRumbleSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<GamepadRumbleComponent, DestroyableComponent>, Exc<DisabledComponent>> 
            filter = default;
        
        private bool rumbleIsActive;
        
        // PAUSE HAPTICS ON PAUSE TOO

        public void Run(EcsSystems systems)
        {
            if (Gamepad.current == null)
            {
                foreach (var e in filter.Value)
                {
                    ref var destroyable = ref filter.Pools.Inc2.Get(e);
                    destroyable.destroy = true;
                }
                
                return;
            }
            
            var deltaTime = dt;

            var rumbleCount = 0;
            
            foreach (var e in filter.Value)
            {
                ref var gamepadRumble = ref filter.Pools.Inc1.Get(e);
                ref var destroyable = ref filter.Pools.Inc2.Get(e);
                
                if (!gamepadRumble.active)
                {
                    // start rumble
                    gamepadRumble.active = true;
                    gamepadRumble.currentTime = gamepadRumble.totalTime * gamepadRumble.instensityMultiplier;
                }
                
                if (gamepadRumble.active)
                {
                    gamepadRumble.currentTime -= deltaTime;
                    rumbleCount++;
                    
                    if (gamepadRumble.currentTime <= 0)
                    {
                        destroyable.destroy = true;
                    }
                }
            }

            if (!rumbleIsActive && rumbleCount > 0)
            {
                Gamepad.current.SetMotorSpeeds(0.15f, 0.25f);
                Gamepad.current.ResumeHaptics();
                rumbleIsActive = true;
            } else if (rumbleIsActive && rumbleCount == 0)
            {
                Gamepad.current.PauseHaptics();
                rumbleIsActive = false;
            }
        }


    }
}