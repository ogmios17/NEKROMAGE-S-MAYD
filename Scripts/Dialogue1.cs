using UnityEngine;
using UnityEngine.UI;

public class Dialogue1 : MonoBehaviour
{

    public PlayerController player;
    public Dialogue dialogue;
    private CanvasGroup canvasGroup;
    public string[] lines;
    public Image[] images;
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
        if (other.CompareTag("Player"))
        {
            player.setTalkingState(true);
            

            dialogue.setLines(lines);
            dialogue.setImages(images);
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            dialogue.StartDialogue();
        }
    }
}
