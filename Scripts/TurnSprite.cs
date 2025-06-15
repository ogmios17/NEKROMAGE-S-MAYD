using System;
using UnityEngine;

public class TurnSprite : MonoBehaviour
{
    public PlayerController playerController;
    private Vector3 initialRotation;
    private Vector3 target;
    private Vector3 currentRotation;
    public float smoothness;
    public InputRandomizer randomizer;
    private Vector3 velocity;
    private bool isTurning = false;
    private bool alreadySet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRotation = transform.rotation.eulerAngles;
        target = initialRotation;
        currentRotation = transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerController.IsInteractable())
        {
            target.y = initialRotation.y + 90;
            isTurning = true;
            playerController.setInteractableState(false);
            alreadySet = false;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isTurning = false;
            target.y = initialRotation.y;
            
        }
        
            currentRotation = Vector3.SmoothDamp(currentRotation, target, ref velocity, smoothness);
            transform.rotation = Quaternion.Euler(currentRotation);

        if (Mathf.Abs(transform.rotation.eulerAngles.y - target.y) < 0.5f)  //If the animation is close to completion
        {
            //be invisible
        }
        if (Mathf.Abs(transform.rotation.eulerAngles.y - initialRotation.y) < 0.5f && !playerController.IsTalking() &&!alreadySet)  //If the animation is close to completion
        {
            playerController.setInteractableState(true);//regain control
            alreadySet = true;
        }
    }

    public void SetBaseRotation(Vector3 rotation)
    {
        initialRotation = rotation;
        transform.rotation = Quaternion.Euler(rotation);
        target = initialRotation;
        currentRotation = rotation;
    }
}
