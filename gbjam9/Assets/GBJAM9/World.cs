using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9
{
    public class World : MonoBehaviour
    {
        [NonSerialized]
        public readonly List<UnitComponent> units = new List<UnitComponent>();

        private int vfxDoneHash = Animator.StringToHash("Done");

        public List<T> GetEntityList<T>() where T : IGameComponent
        {
            return units.Where(u => u.GetComponent<T>() != null)
                .Select(u => u.GetComponent<T>()).ToList();
        }
        
        public void FixedUpdate()
        {
            // perform general logics in order
            var toDestroyUnits = new List<UnitComponent>();

            var mainUnits = units.Where(u => u.GetComponent<MainUnitComponent>() != null)
                .Select(u => u.GetComponent<MainUnitComponent>()).ToList();

            var iterationList = new List<UnitComponent>(units);
            
            foreach (var unit in iterationList)
            {
                var receivedDamage = false;
                
                var health = unit.GetComponent<HealthComponent>();
                if (health != null)
                {
                    receivedDamage = health.damages > 0;
                    
                    if (receivedDamage)
                    {
                        health.current -= health.damages;
                        health.damages = 0;
                    }
                }
                
                // TODO: blink animation state

                var soundEffect = unit.GetComponent<SoundEffectComponent>();
                if (soundEffect != null)
                {
                    if (!soundEffect.started)
                    {
                        soundEffect.sfx.Play();
                        soundEffect.started = true;
                    }
                    else if (!soundEffect.sfx.isPlaying)
                    {
                        toDestroyUnits.Add(unit);
                    }
                }
                
                if (health != null)
                {
                    if (health.current <= 0)
                    {
                        // TODO: spawn death unit
                        unit.destroyed = true;
                    }
                }

                var roomEnd = unit.GetComponentInChildren<RoomExitComponent>();
                if (roomEnd != null)
                {
                    roomEnd.mainUnitCollision = false;

                    foreach (var mainUnit in mainUnits)
                    {
                        if (Vector2.Distance(mainUnit.transform.position, roomEnd.transform.position) < 0.5f)
                        {
                            roomEnd.mainUnitCollision = true;
                            break;
                        }
                    }
                }

                if (unit.pickupComponent != null)
                {
                    if (unit.colliderComponent != null)
                    {
                        var contactsList = new List<ContactPoint2D>();
                        if (unit.colliderComponent.collider.GetContacts(contactsList) > 0)
                        {
                            if (unit.pickupComponent.pickupVfxPrefab != null)
                            {
                                var pickupVfx = GameObject.Instantiate(unit.pickupComponent.pickupVfxPrefab);
                                pickupVfx.transform.position = unit.transform.position;
                            }
                            unit.destroyed = true;
                        }
                    }
                }

                if (unit.visualEffectComponent != null && unit.unitModel != null)
                {
                    unit.destroyed =
                        unit.unitModel.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == vfxDoneHash;
                }

                if (unit.destroyed)
                {
                    toDestroyUnits.Add(unit);
                }
                
                
            }

            foreach (var unit in toDestroyUnits)
            {
                Destroy(unit.gameObject);
            }
        }
    }
}