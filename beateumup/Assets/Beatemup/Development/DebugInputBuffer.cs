using System;
using System.Collections.Generic;
using Beatemup.Ecs;
using Gemserk.Leopotam.Ecs;
using UnityEngine;
using UnityEngine.UI;

public class DebugInputBuffer : MonoBehaviour
{
    public string playerName;

    public GameObject actionPrefab;

    public Transform layoutTransform;

    public List<Sprite> actionSprites = new List<Sprite>();

    private World _world;
 
    private readonly List<Image> _inputBufferList = new List<Image>();
    
    public void Start()
    {
        for (int i = 0; i < ControlComponent.MaxBufferCount; i++)
        {
            var actionInstance = GameObject.Instantiate(actionPrefab, layoutTransform);
            actionInstance.SetActive(false);
            
            _inputBufferList.Add(actionInstance.GetComponent<Image>());
        }

        _world = World.Instance;
    }
    private void Update()
    {
        for (var i = 0; i < _inputBufferList.Count; i++)
        {
            _inputBufferList[i].gameObject.SetActive(false);
        }

        var entity = _world.GetEntityByName(playerName);
        if (entity == Entity.NullEntity)
        {
            return;
        }

        var controlComponent = _world.GetComponent<ControlComponent>(entity);
        
        for (var i = 0; i < _inputBufferList.Count; i++)
        {
            if (i >= controlComponent.buffer.Count)
            {
                break;
            }
                
            _inputBufferList[i].gameObject.SetActive(true);
            _inputBufferList[i].sprite = 
                actionSprites.Find(s => s.name.Equals(controlComponent.buffer[i], StringComparison.OrdinalIgnoreCase));
        }
    }
}
