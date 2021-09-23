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
        public readonly List<EntityComponent> entities = new List<EntityComponent>();

        private int vfxDoneHash = Animator.StringToHash("Done");

        public List<T> GetEntityList<T>() where T : IGameComponent
        {
            return entities.Where(u => u.GetComponent<T>() != null)
                .Select(u => u.GetComponent<T>()).ToList();
        }
        
        public void FixedUpdate()
        {
            // perform general logics in order
            var toDestroy = new List<EntityComponent>();

            var mainUnits = entities.Where(u => u.GetComponent<MainUnitComponent>() != null)
                .Select(u => u.GetComponent<MainUnitComponent>()).ToList();

            var iterationList = new List<EntityComponent>(entities);
            
            foreach (var e in iterationList)
            {
                var receivedDamage = false;
                
                var health = e.GetComponent<HealthComponent>();
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

                var soundEffect = e.GetComponent<SoundEffectComponent>();
                if (soundEffect != null)
                {
                    if (!soundEffect.started)
                    {
                        soundEffect.sfx.Play();
                        soundEffect.started = true;
                    }
                    else if (!soundEffect.sfx.isPlaying)
                    {
                        toDestroy.Add(e);
                    }
                }
                
                if (health != null)
                {
                    if (health.current <= 0)
                    {
                        // TODO: spawn death unit
                        e.destroyed = true;
                    }
                }

                if (e.input != null && e.gameboyControllerComponent != null)
                {
                    var gameboyKeyMap = e.gameboyControllerComponent.gameboyKeyMap;
                    e.input.movementDirection = gameboyKeyMap.direction;
                    e.input.attack = gameboyKeyMap.button1Pressed;
                    e.input.dash = gameboyKeyMap.button2Pressed;
                }
                
                if (e.input != null)
                {
                    if (e.movement != null && e.input.enabled)
                    {
                        e.movement.movingDirection = e.input.movementDirection;
                    }
                    
                    if (e.state != null)
                    {
                        e.state.walking = e.input.enabled && e.input.movementDirection.SqrMagnitude() > 0;
                    }
                }

                if (e.movement != null)
                {
                    var speed = e.movement.speed;
                    var direction = e.movement.movingDirection;

                    if (e.state != null && e.state.dashing)
                    {
                        speed = e.movement.dashSpeed;
                        direction = e.movement.lookingDirection;
                    }
                    
                    var newPosition = e.transform.localPosition;
                    e.movement.velocity = direction * speed * Time.deltaTime;

                    newPosition.x += e.movement.velocity.x * e.movement.perspective.x;
                    newPosition.y += e.movement.velocity.y * e.movement.perspective.y;

                    e.transform.localPosition = newPosition;

                    if (e.movement.velocity.SqrMagnitude() > 0)
                    {
                        e.movement.lookingDirection = e.movement.velocity.normalized;
                    }
                }
                
                if (e.model != null && e.movement != null)
                {
                    e.model.lookingDirection = e.movement.lookingDirection;
                }
                
                var roomEnd = e.GetComponentInChildren<RoomExitComponent>();
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

                if (e.pickupComponent != null)
                {
                    if (e.colliderComponent != null)
                    {
                        var contactsList = new List<ContactPoint2D>();
                        if (e.colliderComponent.collider.GetContacts(contactsList) > 0)
                        {
                            var contactUnit = contactsList[0].collider.GetComponent<EntityComponent>();
                            
                            if (e.pickupComponent.pickupVfxPrefab != null)
                            {
                                var pickupVfx = GameObject.Instantiate(e.pickupComponent.pickupVfxPrefab);
                                pickupVfx.transform.position = e.transform.position;
                            }
                            
                            e.SendMessage("OnPickup", contactUnit, SendMessageOptions.DontRequireReceiver);

                            e.destroyed = true;
                        }
                    }
                }

                if (e.visualEffectComponent != null && e.model != null)
                {
                    e.destroyed =
                        e.model.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == vfxDoneHash;
                }

                if (e.destroyed)
                {
                    toDestroy.Add(e);
                }
                
                
            }

            foreach (var unit in toDestroy)
            {
                Destroy(unit.gameObject);
            }
        }
    }
}