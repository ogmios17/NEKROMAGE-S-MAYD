using UnityEngine;
using UnityEngine.UI;

public class TriggeredDialogue : MonoBehaviour
{
    private bool hasAlreadyTriggered = false;
    public PlayerController player;
    public Dialogue dialogue;
    private CanvasGroup canvasGroup;
    public DialogueParameters[] parameters;
    public bool interruptPlayer = false;
    public bool isActive = true;

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
        if (isActive && other.CompareTag("Player") && !hasAlreadyTriggered)
        {

            hasAlreadyTriggered = true;
            if (interruptPlayer)
            {
                player.setInteractableState(false);
            }
            else dialogue.NotInterrupeted();
            dialogue.SetParameters(parameters);
            canvasGroup.alpha = 1f;
            if (interruptPlayer)
                canvasGroup.interactable = true;
            dialogue.StartDialogue();
        }
    }

  

}


