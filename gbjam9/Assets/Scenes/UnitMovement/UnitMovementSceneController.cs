using System.Collections;
using System.Collections.Generic;
using GBJAM9;
using UnityEngine;

public class UnitMovementSceneController : MonoBehaviour
{
    public UnitModel model;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (model != null)
            {
                model.hit = true;
            }
        }
    }
}
