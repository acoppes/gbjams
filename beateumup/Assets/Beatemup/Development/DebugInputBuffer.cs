using System.Collections.Generic;
using UnityEngine;

public class DebugInputBuffer : MonoBehaviour
{
    public int playerInput;

    public int bufferSize = 10;

    public GameObject actionPrefab;

    public Transform layoutTransform;

    public List<Sprite> actionSprites = new List<Sprite>();
    
    public void Start()
    {
        for (int i = 0; i < bufferSize; i++)
        {
            var actionInstance = GameObject.Instantiate(actionPrefab, layoutTransform);
            actionInstance.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // [ A, A, A, A ]
    }
}
