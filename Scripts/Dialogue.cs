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
    private bool blockPlayer = true;

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
        if(index< parameters.Length && parameters[index].endsAutomatically && typedAll)
        {
            NextLine();
        }
    }
    public void StartDialogue()
    {
        text.text = "";
        cumulativeDialogue = parameters[0].line;
        if (blockPlayer)
        {
            player.setTalkingState(true);
            UI.SetActive(false);
            inputRandomizer.setTimer(false);
        }
        else canvasGroup.transform.position = new Vector3(canvasGroup.transform.position.x, -canvasGroup.transform.position.y, canvasGroup.transform.position.z);
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
        if (parameters[index].endsAutomatically)
        {
            yield return new WaitForSeconds(parameters[index].waitTime);
        }
        typedAll = true;
    }

    void NextLine()
    {
        typedAll = false;
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
            blockPlayer = true;
            player.setInteractableState(true);
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            UI.SetActive(true);
            inputRandomizer.setTimer(true);
            player.setTalkingState(false);
        }
    }
    public void SetParameters(DialogueParameters []parameters)
    {
        this.parameters = parameters;
    }

    public void NotInterrupetd()
    {
        blockPlayer = false;
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
    public float waitTime;
}

