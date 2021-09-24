using System;
using GBJAM9.Components;
using UnityEngine;

namespace GBJAM9.Controllers
{
    public abstract class EntityController : MonoBehaviour
    {
        [NonSerialized]
        public Entity entity;

        public virtual void OnInit(World world)
        {
            
        }

        public virtual void OnWorldUpdate(World world)
        {
            
        }
        
    }
}