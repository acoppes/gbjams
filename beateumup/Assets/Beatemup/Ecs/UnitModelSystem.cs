using Beatemup.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Gameplay;
using Leopotam.EcsLite;
using UnityEngine;

namespace Beatemup.Ecs
{
    public class UnitModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        public float baseShadowOpacity = 0.4f;
        
        public GamePerspectiveAsset gamePerspective;
        
        private GameObject instancesParent;
        
        public void Init(EcsSystems systems)
        {
            instancesParent = new GameObject("~Models");
        }
        
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // create model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                var modelInstance = Instantiate(model.prefab);
                modelInstance.transform.parent = instancesParent.transform;
                
                model.instance = modelInstance.GetComponent<Model>();
                model.instance.gameObject.SetActive(true);
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // destroy model if model component
            var models = world.GetComponents<UnitModelComponent>();
            if (models.Has(entity))
            {
                ref var model = ref models.Get(entity);
                if (model.instance != null)
                {
                    Destroy(model.instance.gameObject);
                }
                model.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var modelComponents = world.GetComponents<UnitModelComponent>();
            var positionComponents = world.GetComponents<PositionComponent>();
            
            var lookingDirectionComponents = world.GetComponents<LookingDirection>();
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().End())
            {
                var modelComponent = modelComponents.Get(entity);

                var model = modelComponent.instance;
                
                if (model.shadow != null)
                {
                    model.shadow.enabled = modelComponent.hasShadow;
                    
                    
                    if (modelComponent.rotation == UnitModelComponent.RotationType.FlipToLookingDirection)
                    {
                        model.shadow.transform.localScale = new Vector3(1,
                            modelComponent.shadowPerspective, 1);
                    } else if (modelComponent.rotation == UnitModelComponent.RotationType.Rotate)
                    {
                        model.shadow.transform.localScale = Vector3.one;
                    }
                    
                    var shadowColor = model.shadow.color;
                    shadowColor.a = baseShadowOpacity * modelComponent.color.a;
                    model.shadow.color = shadowColor;
                }
                
                // disable by default
                if (model.playerIndicator != null)
                {
                    model.playerIndicator.enabled = false;
                }

                model.model.color = modelComponent.color;
            }
            
            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<PositionComponent>().End())
            {
                ref var modelComponent = ref modelComponents.Get(entity);
                var positionComponent = positionComponents.Get(entity);

                var position = gamePerspective.ConvertFromWorld(positionComponent.value);

                modelComponent.instance.transform.position = new Vector3(position.x, position.y, 0);
                modelComponent.instance.model.transform.localPosition = new Vector3(0, position.z, 0);
            }

            foreach (var entity in world.GetFilter<UnitModelComponent>().Inc<LookingDirection>().End())
            {
                var modelComponent = modelComponents.Get(entity);
                var lookingDirection = lookingDirectionComponents.Get(entity);

                var modelInstance = modelComponent.instance;

                var scale = modelInstance.transform.localScale;

                if (modelComponent.rotation == UnitModelComponent.RotationType.FlipToLookingDirection)
                {
                    if (Mathf.Abs(lookingDirection.value.x) > 0)
                    {
                        scale.x = lookingDirection.value.x < 0 ? -1 : 1;
                    }
                    
                    modelInstance.transform.localScale = scale;
                }
                else if (modelComponent.rotation == UnitModelComponent.RotationType.Rotate)
                {
                    var direction3d = lookingDirection.value;
                    var objectModel = modelComponent.instance;
                    
                    var modelAngleForward = Vector2.SignedAngle(Vector2.right, new Vector2(direction3d.x, direction3d.y + direction3d.z));
                    var modelAngleRight = Vector2.Angle(Vector2.right, new Vector2(Mathf.Abs(direction3d.x) + 0.25f, direction3d.z * 0.75f));
            
                    var shadowAngleForward = Vector2.SignedAngle(Vector2.right, new Vector2(direction3d.x, direction3d.y + direction3d.z));
                    // var shadowAngleRight = Vector2.Angle(Vector2.right, new Vector2(Mathf.Abs(direction3d.z) + 0.25f, direction3d.x * 0.75f));

                    var angleAxis = Quaternion.AngleAxis(modelAngleForward, Vector3.forward);
                    var angleRightAxis = Quaternion.AngleAxis(modelAngleRight, Vector3.right);
                    
                    // var angleAxis2 = Quaternion.AngleAxis(shadowAngleRight, Vector3.right);
                    var angleRightAxis2 = Quaternion.AngleAxis(shadowAngleForward, Vector3.forward);
                    
                    objectModel.model.transform.localEulerAngles = angleAxis.eulerAngles + angleRightAxis.eulerAngles;
                    
                    if (modelComponent.hasShadow)
                    {
                        objectModel.shadow.transform.localEulerAngles =
                            angleRightAxis2.eulerAngles + new Vector3(70, 0, 0);
                    }
                    
                    // var eulerAngles = angleAxis.eulerAngles;
                    //
                    //
                    // modelComponent.instance.model.transform.localEulerAngles = eulerAngles;
                    //
                    // // var rotation = Quaternion.LookRotation(lookingDirection.value, Vector3.forward);
                    // // modelComponent.instance.model.transform.rotation = rotation;
                    //
                    // if (modelComponent.hasShadow)
                    // {
                    //     modelComponent.instance.shadow.transform.localEulerAngles = eulerAngles;
                    //     // modelComponent.instance.shadow.transform.rotation = rotation;
                    // }
                }
            }
        }
    }
}