using UnityEngine;
using UnityEngine.EventSystems;

namespace GBJAM14.UI
{
    public class UIDialogController : MonoBehaviour, ISubmitHandler
    {
        public UIDialog uiDialog;
        
        public void OnActionPressed()
        {
            if (!uiDialog.completed)
            {
                uiDialog.ForceComplete();
            }
            else
            {
                uiDialog.CompleteWaiting();
                if (uiDialog.HasPendingText())
                {
                    uiDialog.ShowNext();
                }
                else
                {
                    uiDialog.window.Close();
                }
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            OnActionPressed();
        }
    }
}