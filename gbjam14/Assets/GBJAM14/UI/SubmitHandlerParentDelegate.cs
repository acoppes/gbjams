using UnityEngine;
using UnityEngine.EventSystems;

namespace GBJAM14.UI
{
    public class SubmitHandlerParentDelegate : MonoBehaviour, ISubmitHandler
    {
        public void OnSubmit(BaseEventData eventData)
        {
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.submitHandler);
        }
    }
}