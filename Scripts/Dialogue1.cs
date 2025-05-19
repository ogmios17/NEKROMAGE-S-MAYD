using UnityEngine;
using UnityEngine.UI;

public class Dialogue1 : MonoBehaviour
{

    public PlayerController player;
    public Dialogue dialogue;
    private CanvasGroup canvasGroup;
    public string[] lines;
    public Sprite[] images;
    public float[] speeds;
    public bool trigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = dialogue.gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && !trigger)
        {
            
            player.setInteractableState(true);

            dialogue.setSpeeds(speeds);
            dialogue.setLines(lines);
            dialogue.setImages(images);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            dialogue.StartDialogue();
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && trigger)
        {
            player.setInteractableState(true);

            dialogue.setSpeeds(speeds);
            dialogue.setLines(lines);
            dialogue.setImages(images);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            dialogue.StartDialogue();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && trigger)
        {
            Destroy(gameObject);
        }
    }
}
