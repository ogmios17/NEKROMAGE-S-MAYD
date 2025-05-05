using UnityEngine;

public class CameraHandler : MonoBehaviour
{

    public GameObject player;
    public float offsetz = -9;
    public float offsety= 12;
    public float offsetx = -30;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = player.transform.position.y + offsety;
        pos.z = player.transform.position.z + offsetz;
        pos.x = player.transform.position.x + offsetx;
        transform.position = pos;
    }
}
