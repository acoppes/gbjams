using GBJAM.Commons;
using UnityEngine;

namespace GBJAM10.Components
{
    public class SfxContainerComponent : MonoBehaviour, IEntityComponent
    {
        public SfxVariant walkSfx;
        public SfxVariant lowHealthSfx;
        public SfxVariant deathSfx;
    }
}