using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GBJAM14.UI
{
    public class UIDialogController : MonoBehaviour, ISubmitHandler
    {
        public UIDialog uiDialog;
        public InputActionReference pressAction;

        private void Awake()
        {
            if (pressAction)
            {
                pressAction.action.performed += OnPressAction;
            }
        }

        private void OnDestroy()
        {
            if (pressAction)
            {
                pressAction.action.performed -= OnPressAction;
            }
        }

        private void OnEnable()
        {
            pressAction.action.Enable();
        }

        private void OnDisable()
        {
            pressAction.action.Disable();
        }

        private void OnPressAction(InputAction.CallbackContext obj)
        {
            OnActionPressed();
        }

        public void OnActionPressed()
        {
            if (!uiDialog.completed)
            {
                uiDialog.ForceComplete();
            }
            else
            {
                uiDialog.CompleteWaiting();
                uiDialog.window.Close();
            }
        }

        public void OnSubmit(BaseEventData eventData)
        {
            OnActionPressed();
        }
    }
}