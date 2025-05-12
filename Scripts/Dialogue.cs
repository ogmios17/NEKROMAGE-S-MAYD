using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public PlayerController player;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI text;
    public float speed;
    public string[] lines;
    private int index;
    void Start()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        text.text = "";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
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
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        text.text = "";
        foreach(char c in lines[index].ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            text.text = "";
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
}
