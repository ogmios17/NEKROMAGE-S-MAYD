using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public InputRandomizer inputRandomizer;
    public GameObject UI;
    public PlayerController player;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI text;
    public GameObject imagePlaceholder;
    private Image toInsert;
    public DialogueParameters []parameters;
    private int index;
    private bool typedAll;
    private string cumulativeDialogue;

    void Start()
    {
        toInsert = imagePlaceholder.GetComponent<Image>();
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        text.text = "";
    }
    void Update()
    {
        if (Input.anyKeyDown && canvasGroup.interactable && !parameters[index].endsAutomatically)
        {
            if (text.text == cumulativeDialogue)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                text.text = cumulativeDialogue;
            }
        }
        if(index< parameters.Length-1 && parameters[index].endsAutomatically && typedAll)
        {
            typedAll = false;
            NextLine();
        }
    }
    public void StartDialogue()
    {
        cumulativeDialogue = parameters[0].line;
        UI.SetActive(false);
        inputRandomizer.setTimer(false);
        index = 0;
        toInsert.sprite= parameters[0].image;
        StartCoroutine(Type());
    }

    IEnumerator Type()
    {
        foreach(char c in parameters[index].line.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(parameters[index].speed);
        }
        typedAll = true;
    }

    void NextLine()
    {
        if (index < parameters.Length - 1)
        {
            index++;
            if (!parameters[index].showsInSameBox)
            {
                text.text = "";
                cumulativeDialogue = parameters[index].line;
            }
            else cumulativeDialogue += parameters[index].line;
            toInsert.sprite = parameters[index].image;
            StartCoroutine(Type());
        }
        else
        {
            index = 0;
            player.setInteractableState(true);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            UI.SetActive(true);
            inputRandomizer.setTimer(true);
        }
    }
    public void SetParameters(DialogueParameters []parameters)
    {
        this.parameters = parameters;
    }
}
[System.Serializable]
public class DialogueParameters
{
    public string line;
    public Sprite image;
    public float speed;
    public bool showsInSameBox;
    public bool endsAutomatically;
}
