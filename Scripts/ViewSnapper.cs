using System.Collections;
using UnityEngine;

public class ViewSnapper : MonoBehaviour
{
    private bool hasAlreadyTriggered = false;
    public Camera camera;
    public CameraHandler cameraHandler;
    public PlayerController playerController;
    public GameObject player;
    public float angle;
    public float xOffsetModifier;
    public float zOffsetModifier;
    public Vector3 front;
    public Vector3 back;
    public Vector3 animStart;
    public float movex;
    public float movez;
    public float xRotation;
    public float yRotation;
    public float zRotation;
    public float rotationSmoothness;
    public float turningSmoothness;
    public float playerActiveTimer =2f;
    private float cameraNormalSmoothness;
    public Animator playerAnimator;

    private Vector3 velocity = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraNormalSmoothness = cameraHandler.getTurningSmoothness();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(xOffsetModifier-cameraHandler.GetCurrentOffset().x)<0.5 && Mathf.Abs(zOffsetModifier - cameraHandler.GetCurrentOffset().z) < 0.5)
        {
            playerController.enabled = true;
            playerController.setInteractableState(true);
            cameraHandler.offsetx = xOffsetModifier;
            cameraHandler.offsetz = zOffsetModifier;
            cameraHandler.setTurningSmoothness(cameraNormalSmoothness);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&!hasAlreadyTriggered)
        {
            playerController.setInteractableState(false);
            playerAnimator.SetBool("isRunning", false);
            playerController.enabled = false;
            hasAlreadyTriggered = true;
            player.transform.rotation = Quaternion.Euler(0, angle, 0);
            player.transform.position = new Vector3(player.transform.position.x + movex, player.transform.position.y, player.transform.position.z + movez);
            playerController.setBackDirection(back);
            playerController.setFrontDirection(front);
            cameraHandler.setTurningSmoothness(turningSmoothness);
            cameraHandler.SetTargetOffset(xOffsetModifier, zOffsetModifier);
            cameraHandler.AdjustRotation(new Quaternion(xRotation, yRotation, zRotation, 1), rotationSmoothness);
            
        }
    }

    
}
