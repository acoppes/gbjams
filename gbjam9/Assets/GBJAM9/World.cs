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
        public readonly List<Entity> entities = new List<Entity>();

        private int vfxDoneHash = Animator.StringToHash("Done");

        public List<T> GetEntityList<T>() where T : IGameComponent
        {
            return entities.Where(u => u.GetComponent<T>() != null)
                .Select(u => u.GetComponent<T>()).ToList();
        }
        
        public void Update()
        {
            // perform general logics in order
            var toDestroy = new List<Entity>();

            var mainUnits = entities.Where(u => u.GetComponent<MainUnitComponent>() != null)
                .Select(u => u.GetComponent<MainUnitComponent>()).ToList();

            var iterationList = new List<Entity>(entities);
            
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

                if (e.input != null && e.gameboyController != null)
                {
                    var gameboyKeyMap = e.gameboyController.gameboyKeyMap;
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
                
                if (e.attack != null)
                {
                    e.state.swordAttacking = false;
                    e.state.kunaiAttacking = false;
                    
                    var projectilePrefab = e.attack.projectilePrefab;
                    if (e.input.enabled && e.input.attack && projectilePrefab != null)
                    {
                        var projectileObject = GameObject.Instantiate(projectilePrefab);
                        var projectile = projectileObject.GetComponent<KunaiController>();
                        projectile.Fire(e.transform.position, e.movement.lookingDirection);
                        projectile.entity.player.player = e.player.player;

                        e.state.kunaiAttacking = e.attack.attackType.Equals("kunai");
                        e.state.swordAttacking = e.attack.attackType.Equals("sword");
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
                
                if (e.colliderComponent != null)
                {
                    e.colliderComponent.inCollision = 
                        e.colliderComponent.collider.GetContacts(e.colliderComponent.contactsList) > 0;
                }

                if (e.pickup != null)
                {
                    if (e.colliderComponent != null)
                    {
                        if (e.colliderComponent.inCollision)
                        {
                            var contactUnit = e.colliderComponent.contactsList[0].collider.GetComponent<Entity>();
                            
                            if (e.pickup.pickupVfxPrefab != null)
                            {
                                var pickupVfx = GameObject.Instantiate(e.pickup.pickupVfxPrefab);
                                pickupVfx.transform.position = e.transform.position;
                            }
                            
                            e.SendMessage("OnPickup", contactUnit, SendMessageOptions.DontRequireReceiver);

                            e.destroyed = true;
                        }
                    }
                }

                if (e.vfx != null && !e.vfx.sfxSpawned)
                {
                    e.vfx.sfxVariant.Play();
                    e.vfx.sfxSpawned = true;
                }

                if (e.vfx != null && e.model != null)
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

        private void LateUpdate()
        {
            var iterationList = new List<Entity>(entities);

            foreach (var e in iterationList)
            {
                if (e.model != null)
                {
                    var animator = e.model.animator;
                    var unitState = e.state;
                    
                    if (animator != null && unitState != null)
                    {
                        animator.SetBool(UnitStateComponent.walkingStateHash, unitState.walking);
                        animator.SetBool(UnitStateComponent.kunaiAttackStateHash, unitState.kunaiAttacking);
                        animator.SetBool(UnitStateComponent.swordAttackStateHash, unitState.swordAttacking);
                        animator.SetBool(UnitStateComponent.dashingStateHash, unitState.dashing);
                        animator.SetBool(UnitStateComponent.hitStateHash, unitState.hit);
                    }

                    if (!e.model.rotateToDirection)
                    {
                        if (Mathf.Abs(e.model.lookingDirection.x) > 0)
                        {
                            e.model.model.flipX = e.model.lookingDirection.x < 0;
                        }
                    }
                    else
                    {
                        var angle = Mathf.Atan2(e.model.lookingDirection.y, e.model.lookingDirection.x) * Mathf.Rad2Deg;
                        e.model.model.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    }
                }
            }
        }
    }
}