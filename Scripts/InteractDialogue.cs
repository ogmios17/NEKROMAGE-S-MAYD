using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;



public class InteractDialogue : MonoBehaviour
{
    public GameObject image;
    public GameObject player;
    public PlayerController playerController;
    public Dialogue dialogue;
    public CanvasGroup uiCanvas;
    public InputRandomizer randomizer;
    public DialogueParameters[] parameters;
    private bool inRange;
    public Vector3 imagePosition;
    private bool AlreadyInteracted = false;
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(imagePosition == null || imagePosition == Vector3.zero)
        {
            imagePosition = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        }
        image.transform.position = imagePosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        Debug.Log(distance);
        if (Mathf.Abs(distance) < 15)
        {
            image.SetActive(true);
            if(!playerController.IsTalking())
                inRange = true;
        }
        else
        {
            image.SetActive(false);
            inRange = false;
        }

        if (Input.GetKeyDown(randomizer.GetInteract()) && inRange &&!AlreadyInteracted){
            
            playerController.setInteractableState(false);
            AlreadyInteracted = true;
            dialogue.ignoreNextInput = true;
            dialogue.SetParameters(parameters);
            uiCanvas.alpha = 1f;
            uiCanvas.interactable = true;
            dialogue.StartDialogue();
        }
        if (playerController.IsInteractable()) {
            AlreadyInteracted = false;
        }
    }
}
