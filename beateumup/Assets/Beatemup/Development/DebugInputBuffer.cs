using System;
using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class DebugInputBuffer : MonoBehaviour
{
    // public int playerInput;
    public string playerName;
    
    public int bufferSize = 10;

    public GameObject actionPrefab;

    public Transform layoutTransform;

    public List<Sprite> actionSprites = new List<Sprite>();

    private World world;

    private List<Image> inputBufferList = new List<Image>();
    
    public void Start()
    {
        for (int i = 0; i < bufferSize; i++)
        {
            var actionInstance = GameObject.Instantiate(actionPrefab, layoutTransform);
            actionInstance.SetActive(false);
            
            inputBufferList.Add(actionInstance.GetComponent<Image>());
        }

        world = World.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // [ A, A, A, A ]

        for (var i = 0; i < inputBufferList.Count; i++)
        {
            inputBufferList[i].gameObject.SetActive(false);
        }

        var entity = world.GetEntityByName(playerName);
        if (entity == Entity.NullEntity)
        {
            return;
        }

        var controlComponent = world.GetComponent<ControlComponent>(entity);

        if (controlComponent.forward.isPressed)
        {
            inputBufferList[0].gameObject.SetActive(true);
            inputBufferList[0].sprite = 
                actionSprites.Find(s => s.name.Equals("right", StringComparison.OrdinalIgnoreCase));
        }
        
    }
}
