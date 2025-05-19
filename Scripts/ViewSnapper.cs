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
    public Vector3 front;
    public Vector3 back;
    public float movex;
    public float movez;
    public Animation cameraSnapAnim;
    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cameraHandler.enabled = false;

            player.transform.rotation = Quaternion.Euler(0, angle, 0);
            player.transform.position = new Vector3(player.transform.position.x + movex, player.transform.position.y, player.transform.position.z + movez);
            playerController.setBackDirection(back);
            playerController.setFrontDirection(front);
            playerController.setInteractableState(false);
            cameraSnapAnim.Play();
            
            cameraHandler.offsetx = xOffsetModifier;
            cameraHandler.offsetz = zOffsetModifier;

            cameraHandler.enabled = true;
        }
    }
}
