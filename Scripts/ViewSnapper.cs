using System.Collections;
using UnityEngine;

public class ViewSnapper : MonoBehaviour
{
    public Camera camera;
    public CameraHandler cameraHandler;
    public PlayerController playerController;
    public GameObject player;
    public float angle;
    public float xOffsetModifier;
    public float zOffsetModifier;
    private float angleAcc;
    public Vector3 front;
    public Vector3 back;
    public float movex;
    public float movez;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angleAcc = angle;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camera.transform.rotation = new Quaternion(camera.transform.rotation.x, 180, camera.transform.rotation.z, 1);
            player.transform.position = new Vector3(player.transform.position.x + movex, player.transform.position.y, player.transform.position.z + movez);
            cameraHandler.offsetx += xOffsetModifier;
            cameraHandler.offsetz += zOffsetModifier;
            playerController.setBackDirection(back);
            playerController.setFrontDirection(front);
            
        }
    }

}
