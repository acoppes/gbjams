using UnityEngine;

namespace GBJAM9
{
    public class NinjaCatController : MonoBehaviour
    {
        [SerializeField]
        protected UnitInput unitInput;

        [SerializeField]
        protected UnitMovement unitMovement;
        
        [SerializeField]
        protected UnitModel unitModel;

        [SerializeField]
        protected GameObject kunaiPrefab;

        // Update is called once per frame
        private void Update()
        {
            if (unitInput.movementDirection.SqrMagnitude() > 0)
            {
                unitMovement.lookingDirection = unitInput.movementDirection;
                unitMovement.Move();
            }
            
            unitModel.lookingDirection = unitMovement.lookingDirection;
            
            if (unitInput.fireKunai && kunaiPrefab != null)
            {
                // fire kunai!!
                var kunaiObject = GameObject.Instantiate(kunaiPrefab);
                var kunai = kunaiObject.GetComponent<KunaiController>();
                kunai.Fire(transform.position, unitMovement.lookingDirection);
            }
        }
    }
}
