using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InputRandomizer : MonoBehaviour
{
    private bool timerActive = true;
    public InputVisualizer inputVisualizer;
    private GameObject interactButton;
    public Image backSprite;
    public Image forwardSprite;
    public Image jumpSprite;
    public bool randomizeBack = true;
    public bool randomizeForward = true;
    public bool randomizeJump = true;
    public bool randomizeInteract = true;

    private KeyCode[] keys = {
            KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
            KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
            KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
            KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
            KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y, KeyCode.Z,
            KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
            KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
            KeyCode.Space, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
    };
    private int index;
    private Queue<KeyCode> interactQueue;
    private Queue<KeyCode> backQueue;
    private Queue<KeyCode> forwardQueue;
    private Queue<KeyCode> jumpQueue;
    public KeyCode interactInput;
    public KeyCode backInput;
    public KeyCode forwardInput;
    public KeyCode jumpInput;
    private float timeSpanInteract;
    private float timeSpanBack;
    private float timeSpanForward;
    private float timeSpanJump;
    private KeyCode[] currentKeys = new KeyCode[7];
    private int currentKeyIndex;
    public float minInteract = 2;
    public float minBack= 10;
    public float minForward = 10;
    public float minJump = 10;
    public float maxInteract = 5;
    public float maxBack = 30;
    public float maxForward = 30;
    public float maxJump = 30;
    void Start()
    { 

        interactButton = GameObject.FindWithTag("InteractButton");
        timeSpanInteract = 0;
        timeSpanBack = 0;
        timeSpanForward = 0;
        timeSpanJump = 0;
        interactQueue = new Queue<KeyCode>();
        backQueue = new Queue<KeyCode>();
        forwardQueue = new Queue<KeyCode>();
        jumpQueue = new Queue<KeyCode>();

        if(randomizeBack)
            Randomize(backQueue, true);
        else
        {
            backInput = KeyCode.A;
            backSprite.sprite = inputVisualizer.getSprite(backInput);
        }
        if(randomizeForward)
            Randomize(forwardQueue, true);
        else
        {
            forwardInput = KeyCode.D;
            forwardSprite.sprite = inputVisualizer.getSprite(forwardInput);
        }
        if(randomizeJump)
            Randomize(jumpQueue, true);
        else
        {
            jumpInput = KeyCode.Space;
            jumpSprite.sprite = inputVisualizer.getSprite(jumpInput);
        }
        if (randomizeInteract)
            Randomize(interactQueue, true);
        else
        {
            interactInput = KeyCode.E;
            interactButton.GetComponent<SpriteRenderer>().sprite = inputVisualizer.getSprite(interactInput);
        }
    }

    void Update()
    {
        if (timerActive)
        {
            timeSpanInteract -= Time.deltaTime;
            timeSpanBack -= Time.deltaTime;
            timeSpanForward -= Time.deltaTime;
            timeSpanJump -= Time.deltaTime;
        }

        /*backText.text = timeSpanBack.ToString()+"/////////"+backQueue.Peek();
        forwardText.text = timeSpanForward.ToString() + "/////////" + forwardQueue.Peek();
        jumpText.text = timeSpanJump.ToString() + "/////////" + jumpQueue.Peek();*/
        
        if (randomizeInteract && timeSpanInteract <= 0)
        {
            interactInput = Randomize(ref timeSpanInteract, minInteract, maxInteract);
            UpdateInteractSprite();
        }
        if (randomizeBack && timeSpanBack <= 0)
        {
            backInput = Randomize(ref timeSpanBack, ref backQueue, minBack, maxBack);
            UpdateBackSprite();
        }
        if (randomizeForward && timeSpanForward <= 0)
        {
            forwardInput = Randomize(ref timeSpanForward, ref forwardQueue, minForward, maxForward);
            UpdateForwardSprite();
        }
        if (randomizeJump && timeSpanJump <= 0)
        {
            jumpInput = Randomize(ref timeSpanJump, ref jumpQueue, minJump, maxJump);
            UpdateJumpSprite();
        }   
    }

    KeyCode Randomize(ref float timeSpan, ref Queue<KeyCode> queue, float min, float max)
    {
        timeSpan = Random.Range(min,max);
        index = Random.Range(0, keys.Length);
        for (int i = 0; i < 7; i++)
        {
            while (keys[index] == currentKeys[i])
            {
                index = Random.Range(0, keys.Length);
                i = 0;
            }

        }
        currentKeys[currentKeyIndex % 7] = keys[index];
        currentKeyIndex++;
        queue.Enqueue(keys[index]);
        return queue.Dequeue();
    }

    KeyCode Randomize(ref float timeSpan, float min, float max)
    {
        timeSpan = Random.Range(min,max);
        index = Random.Range(0, keys.Length);
        for (int i = 0; i < 7; i++)
        {
            while (keys[index] == currentKeys[i])
            {
                index = Random.Range(0, keys.Length);
                i = 0;
            }

        }
        currentKeys[currentKeyIndex % 7] = keys[index];
        currentKeyIndex++;
        return keys[index];
    }

    public KeyCode Randomize(Queue<KeyCode> queue, bool addToQueue)
    {
        index = Random.Range(0, keys.Length);
        for (int i = 0; i < 6; i++)
        {
            while (keys[index] == currentKeys[i])
            {
                index = Random.Range(0, keys.Length);
                i = 0;
            }

        }
        currentKeys[currentKeyIndex % 6] = keys[index];
        currentKeyIndex++;
        if(addToQueue)
            queue.Enqueue(keys[index]);

        return keys[index];
        
        
    }

    public KeyCode GetBack()
    {
        return  backInput ;
    }
    public KeyCode GetForward()
    {
        return forwardInput ;
    }
    public KeyCode GetJump()
    {
        return jumpInput ;
    }
    public KeyCode GetInteract()
    {
        return interactInput;
    }

    public void setTimer(bool timer)
    {
        timerActive = timer;
    }

    public Queue<KeyCode> GetInteractQueue()
    {
        return interactQueue;
    }

    public Queue<KeyCode> GetJumpQueue()
    {
        return jumpQueue;
    }

    public Queue<KeyCode> GetForwardQueue()
    {
        return forwardQueue;
    }

    public Queue<KeyCode> GetBackQueue()
    {
        return backQueue;
    }

    public void UpdateBackSprite()
    {
        backSprite.sprite = inputVisualizer.getSprite(backInput);
    }

    public void UpdateForwardSprite()
    {
        forwardSprite.sprite = inputVisualizer.getSprite(forwardInput);
    }

    public void UpdateJumpSprite()
    {
        jumpSprite.sprite = inputVisualizer.getSprite(jumpInput);
    }

    public void UpdateInteractSprite()
    {
        interactButton.GetComponent<SpriteRenderer>().sprite = inputVisualizer.getSprite(interactInput);
    }
}
