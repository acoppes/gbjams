using UnityEngine;

public class CharacterWorldView : MonoBehaviour
{
    public Vector3Int movement = new Vector3Int(4, 4, 0);
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-movement.x, 0, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(movement.x, 0, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position += new Vector3(0, movement.y, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position += new Vector3(0, -movement.y, 0);
        }
    }
}
