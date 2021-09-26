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

        public List<T> GetComponentList<T>() where T : IEntityComponent
        {
            return entities.Where(u => u.GetComponent<T>() != null)
                .Select(u => u.GetComponent<T>()).ToList();
        }
        
        public List<Entity> GetEntitiesWith<T>() where T : IEntityComponent
        {
            return entities.Where(e => e.GetComponent<T>() != null)
                .ToList();
        }
        
        public List<Entity> GetEntitiesWith<T1, T2>() 
            where T1 : IEntityComponent 
            where T2 : IEntityComponent
        {
            return entities
                .Where(e => e.GetComponent<T1>() != null)
                .Where(e => e.GetComponent<T2>() != null)
                .ToList();
        }

        public Entity GetSingleton(string name)
        {
            return entities
                .FirstOrDefault(e => e.singleton != null && e.singleton.uniqueName.Equals(name));
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
                    if (e.controller.controllerObject is EntityController controllerObject)
                    {
                        if (!e.controller.initialized)
                        {
                            controllerObject.entity = e;
                            controllerObject.OnInit(this);
                            e.controller.initialized = true;
                        }

                        controllerObject.OnWorldUpdate(this);
                    }
                }

                if (e.model != null && e.model.optionalStartLookAt != null)
                {
                    e.model.lookingDirection = e.model.optionalStartLookAt.localPosition.normalized;
                    
                    Destroy(e.model.optionalStartLookAt.gameObject);
                    e.model.optionalStartLookAt = null;
                }

                if (e.state != null)
                {
                    // reset the hit state
                    e.state.hit = false;
                    e.state.dead = false;
                }
                
                var health = e.health;
                if (health != null)
                {
                    if (health.current > 0)
                    {
                        var receivedDamage = health.damages > 0;

                        if (receivedDamage && !health.immortal)
                        {
                            health.current -= health.damages;
                        }

                        health.damages = 0;
                        health.alive = health.current > 0;

                        if (e.state != null)
                        {
                            e.state.hit = receivedDamage;

                            if (!health.alive)
                            {
                                e.state.dead = true;

                                if (health.vfxPrefab != null)
                                {
                                    var vfxObject = GameObject.Instantiate(e.health.vfxPrefab);
                                    vfxObject.transform.position = e.health.vfxAttachPoint.position;
                                }
                            }
                        }
                    }

                    health.current = Mathf.Clamp(health.current, 0, health.total);
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

                // dont destroy on death
                // if (health != null)
                // {
                //     if (health.current <= 0)
                //     {
                //         // TODO: spawn death unit
                //         e.destroyed = true;
                //     }
                // }

                if (e.input != null && e.gameboyController != null)
                {
                    var gameboyKeyMap = e.gameboyController.gameboyKeyMap;
                    e.input.movementDirection = gameboyKeyMap.direction;
                    e.input.attack = gameboyKeyMap.button1Pressed;
                    e.input.dash = gameboyKeyMap.button2Pressed;
                }
                
                if (e.input != null)
                {
                    if (e.health != null && !e.health.alive)
                    {
                        e.input.enabled = false;
                    }
                    
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

                if (e.roomExit != null)
                {
                    e.roomExit.playerInExit = false;

                    if (e.roomExit.open)
                    {
                        // TODO: change to collider
                        foreach (var mainUnit in mainUnits)
                        {
                            if (Vector2.Distance(mainUnit.transform.position, e.roomExit.transform.position) <
                                e.roomExit.distance)
                            {
                                e.roomExit.playerInExit = true;
                                break;
                            }
                        }
                    }
                    
                    if (e.state != null)
                    {
                        e.state.dead = e.roomExit.open;
                    }
                }

                if (e.collider != null)
                {
                    e.collider.contactsList.Clear();
                    e.collider.collidingEntities.Clear();

                    e.collider.inCollision = false;
                    
                    var updateCollider = true;
                    
                    if (e.health != null && !e.health.alive)
                    {
                        updateCollider = false;
                        
                        if (e.collider.rigidbody != null)
                        {
                            e.collider.rigidbody.bodyType = RigidbodyType2D.Kinematic;
                        }

                        if (e.collider.collider != null)
                        {
                            e.collider.collider.enabled = false;
                        }
                        // turn off collider and rigid body too
                    }
                    
                    if (updateCollider)
                    {
                        e.collider.inCollision =
                            e.collider.collider.GetContacts(e.collider.contactsList) > 0;

                        // filter duplicates?
                        e.collider.collidingEntities = e.collider.contactsList
                            .Where(c => c.collider.GetComponent<Entity>() != null)
                            .Select(c => c.collider.GetComponent<Entity>())
                            .Distinct()
                            .ToList();
                    }
                }

                if (e.pickup != null)
                {
                    if (e.collider != null)
                    {
                        if (e.collider.inCollision)
                        {
                            var contactUnit = e.collider.contactsList[0].collider.GetComponent<Entity>();
                            
                            if (e.pickup.pickupVfxPrefab != null)
                            {
                                var pickupVfx = GameObject.Instantiate(e.pickup.pickupVfxPrefab);
                                pickupVfx.transform.position = e.transform.position;
                            }

                            e.pickup.picked = true;
                            
                            e.SendMessage("OnPickup", contactUnit, SendMessageOptions.DontRequireReceiver);

                            e.destroyed = true;

                            onPickup?.Invoke(e);
                        }
                    }
                }

                if (e.projectile != null && e.collider != null && !e.projectile.damagePerformed)
                {
                    if (e.collider.inCollision)
                    {
                        foreach (var otherEntity in e.collider.collidingEntities)
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
                        
                        // animator.SetBool(UnitStateComponent.deadStateHash, state.dead);
                        
                        if (state.hit)
                        {
                            animator.SetTrigger(UnitStateComponent.hittedStateHash);
                        }

                        if (state.dead)
                        {
                            animator.SetTrigger(UnitStateComponent.deadStateHash);
                        }
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