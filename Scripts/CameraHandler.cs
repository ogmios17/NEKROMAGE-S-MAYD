using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    public GameObject player;
    public float offset;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y;
        pos.z = player.transform.position.z + offset;
        transform.position = pos;
    }
}
