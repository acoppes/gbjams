using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GBJAM13.UI
{
    public class UIDialogController : MonoBehaviour
    {
        public UIDialog uiDialog;
        public InputActionReference pressAction;

        private void Awake()
        {
            pressAction.action.performed += OnPressAction;
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
            if (!uiDialog.completed)
            {
                uiDialog.ForceComplete();
            }
        }
    }
}