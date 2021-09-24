using System;
using System.Collections.Generic;
using System.Linq;
using GBJAM9.Components;
using GBJAM9.Controllers;
using UnityEngine;

namespace GBJAM9
{
    public class World : MonoBehaviour
    {
        [NonSerialized]
        public readonly List<Entity> entities = new List<Entity>();

        private readonly int vfxDoneHash = Animator.StringToHash("Done");

        public Action<Entity> onPickup;
        
        private int playerLayer;
        private int enemyLayer;

        private int playerProjectilesLayer;
        private int enemyProjectilesLayer;

        public List<T> GetEntityList<T>() where T : IGameComponent
        {
            return entities.Where(u => u.GetComponent<T>() != null)
                .Select(u => u.GetComponent<T>()).ToList();
        }

        private void Awake()
        {
            playerLayer = LayerMask.NameToLayer("Player");
            enemyLayer = LayerMask.NameToLayer("Enemy");
            playerProjectilesLayer = LayerMask.NameToLayer("Player_Attack");
            enemyProjectilesLayer = LayerMask.NameToLayer("Enemy_Attack");
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
                if (e.player != null)
                {
                    e.player.layer = e.player.player == 0 ? playerLayer : enemyLayer;
                    e.player.enemyLayer = e.player.player == 0 ? enemyLayer : playerLayer;
                    e.player.projectileLayer = e.player.player == 0 ? playerProjectilesLayer : enemyProjectilesLayer;

                    e.player.layerMask = LayerMask.GetMask(e.player.player == 0 ? "Player" : "Enemy");
                    e.player.enemyLayerMask = LayerMask.GetMask(e.player.player == 0 ? "Enemy" : "Player");
                }

                if (e.controller != null)
                {
                    var controllerObject = e.controller.controllerObject as MonoBehaviour;
                    
                    if (controllerObject != null)
                    {

                        if (!e.controller.initialized)
                        {
                            controllerObject.SendMessage("OnEnterWorld", this);
                            e.controller.initialized = true;
                        }
                        
                        controllerObject.SendMessage("OnWorldUpdate", this);
                    }
                }

                if (e.model != null && e.model.optionalStartLookAt != null)
                {
                    e.model.lookingDirection = e.model.optionalStartLookAt.localPosition.normalized;
                    
                    Destroy(e.model.optionalStartLookAt.gameObject);
                    e.model.optionalStartLookAt = null;
                }
                
                var health = e.health;
                if (health != null)
                {
                    var receivedDamage = health.damages > 0;
                    
                    if (receivedDamage)
                    {
                        health.current -= health.damages;
                        health.damages = 0;
                    }

                    if (e.state != null)
                    {
                        e.state.hit = receivedDamage;
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
                    if (e.movement != null)
                    {
                        if (e.input.enabled)
                        {
                            e.movement.movingDirection = e.input.movementDirection;
                        }
                        else
                        {
                            e.movement.movingDirection = Vector2.zero;
                        }
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
                    e.attack.cooldown -= Time.deltaTime;

                    e.state.swordAttacking = false;
                    e.state.kunaiAttacking = false;

                    var weaponData = e.attack.weaponData;
                    
                    if (weaponData != null)
                    {
                        var projectilePrefab = weaponData.projectilePrefab;
                        if (e.input.enabled && e.input.attack && projectilePrefab != null && e.attack.cooldown < 0)
                        {
                            var projectileObject = GameObject.Instantiate(projectilePrefab);
                            var projectile = projectileObject.GetComponent<ProjectileController>();
                            projectile.Fire(e.transform.position + e.attack.attackAttachPoint.localPosition,
                                e.movement.lookingDirection);
                            projectile.entity.player.player = e.player.player;

                            if (e.player.player == 0)
                            {
                                projectile.gameObject.layer = playerProjectilesLayer;
                            }
                            else
                            {
                                projectile.gameObject.layer = enemyProjectilesLayer;
                            }

                            e.state.kunaiAttacking = weaponData.attackType.Equals("kunai");
                            e.state.swordAttacking = weaponData.attackType.Equals("sword");

                            e.attack.cooldown = weaponData.cooldown;
                        }
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
                        if (Vector2.Distance(mainUnit.transform.position, roomEnd.transform.position) < roomEnd.distance)
                        {
                            roomEnd.mainUnitCollision = true;
                            break;
                        }
                    }
                }
                
                if (e.colliderComponent != null)
                {
                    e.colliderComponent.contactsList.Clear();
                    e.colliderComponent.collidingEntities.Clear();
                    
                    e.colliderComponent.inCollision = 
                        e.colliderComponent.collider.GetContacts(e.colliderComponent.contactsList) > 0;
                    
                    // filter duplicates?
                    e.colliderComponent.collidingEntities = e.colliderComponent.contactsList
                            .Where(c => c.collider.GetComponent<Entity>() != null)
                            .Select(c => c.collider.GetComponent<Entity>())
                            .Distinct()
                            .ToList();
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

                            onPickup?.Invoke(e);
                        }
                    }
                }

                if (e.projectile != null && e.colliderComponent != null && !e.projectile.damagePerformed)
                {
                    if (e.colliderComponent.inCollision)
                    {
                        foreach (var otherEntity in e.colliderComponent.collidingEntities)
                        {
                            if (otherEntity != null && e.projectile.totalTargets > 0)
                            {
                                if (otherEntity.player == null)
                                    continue;
                                
                                if (otherEntity.player.player == e.player.player)
                                    continue;
                
                                if (otherEntity.health != null)
                                {
                                    otherEntity.health.damages += e.projectile.damage;
                                    e.projectile.totalTargets--;
                                }
                            }
                        }
                        
                        e.destroyed = true;
                        e.projectile.damagePerformed = true;

                        if (e.projectile.hitSfxPrefab != null)
                        {
                            Instantiate(e.projectile.hitSfxPrefab, transform.position, Quaternion.identity);
                        }
                    }
                }

                if (e.vfx != null && !e.vfx.sfxSpawned)
                {
                    if (e.vfx.sfxVariant != null)
                    {
                        e.vfx.sfxVariant.Play();
                    }
                    e.vfx.sfxSpawned = true;
                }

                if (e.vfx != null && e.model != null)
                {
                    e.destroyed =
                        e.model.animator.GetCurrentAnimatorStateInfo(0).shortNameHash == vfxDoneHash;
                }

                if (e.decoComponent != null)
                {
                    // round positions to pixel perfect...
                    // e.transform.position.
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
                    var state = e.state;
                    
                    if (animator != null && state != null)
                    {
                        animator.SetBool(UnitStateComponent.walkingStateHash, state.walking);
                        animator.SetBool(UnitStateComponent.kunaiAttackStateHash, state.kunaiAttacking);
                        animator.SetBool(UnitStateComponent.swordAttackStateHash, state.swordAttacking);
                        animator.SetBool(UnitStateComponent.dashingStateHash, state.dashing);
                        animator.SetBool(UnitStateComponent.hitStateHash, state.hit);
                    }

                    if (!e.model.rotateToDirection)
                    {
                        var scale = e.model.transform.localScale;

                        if (Mathf.Abs(e.model.lookingDirection.x) > 0)
                        {
                            // e.model.model.flipX = e.model.lookingDirection.x < 0;
                            scale.x = e.model.lookingDirection.x < 0 ? -1 : 1;
                        }

                        if (e.model.verticalFlip && Mathf.Abs(e.model.lookingDirection.y) > 0)
                        {
                            // e.model.model.flipY = e.model.lookingDirection.y > 0;
                            scale.y = e.model.lookingDirection.y > 0 ? -1 : 1;
                        }

                        e.model.transform.localScale = scale;
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