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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(imagePosition == null)
        {
            imagePosition = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
        }
        image.transform.position = imagePosition;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Mathf.Abs(gameObject.transform.position.z-player.transform.position.z) < 5)
        {
            image.SetActive(true);
            inRange = true;
        }
        else
        {
            image.SetActive(false);
            inRange = false;
        }

        if (Input.GetKeyDown(randomizer.GetInteract()) && inRange &&!AlreadyInteracted){
            
            playerController.setInteractableState(false);
            randomizer.randomize = true;
            AlreadyInteracted = true;

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
