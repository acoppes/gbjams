using System;
using System.Collections;
using System.Collections.Generic;
using GBJAM.Commons;
using UnityEngine;

public class FreeUnitMovement : MonoBehaviour
{
    [SerializeField]
    protected Transform transform;

    [SerializeField]
    protected Transform cameraTransform;

    [SerializeField]
    protected GameboyButtonKeyMapAsset gameboyKeyMap;

    [SerializeField]
    protected float speed;

    [SerializeField]
    protected Vector2 perspective = new Vector2(1.0f, 0.75f);
    
    // Update is called once per frame
    private void Update()
    {
        var myPosition = transform.localPosition;
        var velocity = gameboyKeyMap.direction * speed * Time.deltaTime;

        // TODO: vertical movement perspective....
        
        myPosition.x += velocity.x * perspective.x;
        myPosition.y += velocity.y * perspective.y;

        transform.localPosition = myPosition;
    }

    private void LateUpdate()
    {
        var p = cameraTransform.transform.position;
        p.x = transform.position.x;
        p.y = transform.position.y;
        cameraTransform.transform.position = p;

    }
}
