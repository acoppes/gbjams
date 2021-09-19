using System.Collections;
using System.Collections.Generic;
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
    }
}
