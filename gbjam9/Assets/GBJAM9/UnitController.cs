using UnityEngine;

namespace GBJAM9
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField]
        protected UnitInput unitInput;

        [SerializeField]
        protected UnitMovement unitMovement;
        
        [SerializeField]
        protected UnitModel unitModel;

        // Update is called once per frame
        private void Update()
        {
            unitMovement.Move(unitInput.movementDirection);
            unitModel.velocity = unitMovement.velocity;
        }
    }
}
