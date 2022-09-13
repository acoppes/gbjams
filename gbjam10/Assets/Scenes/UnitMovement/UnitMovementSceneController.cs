using System.Collections;
using System.Collections.Generic;
using GBJAM9;
using GBJAM9.Components;
using UnityEngine;

public class UnitMovementSceneController : MonoBehaviour
{
    public UnitStateComponent unitState;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (unitState != null)
            {
                unitState.hit = !unitState.hit;
            }
        }
    }
}
