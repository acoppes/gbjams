using UnityEngine;

public class Shadow : MonoBehaviour
{
    public SpriteRenderer source;
    public SpriteRenderer target;

    void LateUpdate()
    {
        target.sprite = source.sprite;
    }
}
