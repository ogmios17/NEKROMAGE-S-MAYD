using UnityEngine;
using UnityEngine.UI;

public class Dialogue1 : MonoBehaviour
{
    private bool hasAlreadyTriggered = false;
    public PlayerController player;
    public Dialogue dialogue;
    private CanvasGroup canvasGroup;
    public DialogueParameters[] parameters;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = dialogue.gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&!hasAlreadyTriggered)
        {
            hasAlreadyTriggered = true;
            player.setInteractableState(false);

            dialogue.SetParameters(parameters);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            dialogue.StartDialogue();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}


