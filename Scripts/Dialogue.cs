using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public PlayerController player;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI text;
    public GameObject imagePlaceholder;
    private Image toInsert;
    public float[] speeds;
    private string[] lines;
    private Sprite[] images;
    private int index;
    void Start()
    {
        toInsert = imagePlaceholder.GetComponent<Image>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        text.text = "";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canvasGroup.interactable)
        {
            if (text.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
            }
        }
    }
    public void StartDialogue()
    {
        index = 0;
        toInsert.sprite= images[0];
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        text.text = "";
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(speeds[index]);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = "";
            toInsert.sprite = images[index];
            StartCoroutine(Type());
        }
        else
        {
            player.setTalkingState(false);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
        }
    }
    public void setLines(string[] lines)
    {
        this.lines = lines;
    }

    public void setImages(Sprite[] images)
    {
        this.images = images;
    }

    public void setSpeeds(float[] speeds)
    {
        this.speeds = speeds;
    }
}
