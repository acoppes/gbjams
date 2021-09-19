using System.Collections;
using System.Collections.Generic;
using GBJAM.Commons.Transitions;
using UnityEngine;

public class TestTransitionSceneController : MonoBehaviour
{
    public Transition transition;
    
    // Update is called once per frame
    private void Update()
    {
        if (transition == null)
        {
            return;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            transition.Open();
        }    
        
        if (Input.GetMouseButtonUp(1))
        {
            transition.Close();
        }    
        
        if (transition.isOpen)
        {
            Debug.Log($"Transition open");
        } else if (transition.isClosed)
        {
            Debug.Log($"Transition closed");
        }
    }
}
