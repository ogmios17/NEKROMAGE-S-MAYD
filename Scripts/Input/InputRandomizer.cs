using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputRandomizer : MonoBehaviour
{
    public enum devices{keyboard,controller};
    public devices currentDevice;
    public Actions playerInput;
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
    [HideInInspector]
    public string[] inputPool =
    {
        "<Keyboard>/a", "<Keyboard>/b", "<Keyboard>/c", "<Keyboard>/d", "<Keyboard>/e",
                    "<Keyboard>/f", "<Keyboard>/g", "<Keyboard>/h", "<Keyboard>/i", "<Keyboard>/j",
                    "<Keyboard>/k", "<Keyboard>/l", "<Keyboard>/m", "<Keyboard>/n", "<Keyboard>/o",
                    "<Keyboard>/p", "<Keyboard>/q", "<Keyboard>/r", "<Keyboard>/s", "<Keyboard>/t",
                    "<Keyboard>/u", "<Keyboard>/v", "<Keyboard>/w", "<Keyboard>/x", "<Keyboard>/y", "<Keyboard>/z",
                    "<Keyboard>/1", "<Keyboard>/2", "<Keyboard>/3", "<Keyboard>/4", "<Keyboard>/5",
                    "<Keyboard>/6", "<Keyboard>/7", "<Keyboard>/8", "<Keyboard>/9", "<Keyboard>/0",
                    "<Keyboard>/space", "<Keyboard>/up", "<Keyboard>/down", "<Keyboard>/left", "<Keyboard>/right",
                    "<Gamepad>/buttonSouth",
                    "<Gamepad>/buttonEast",
                    "<Gamepad>/buttonWest",
                    "<Gamepad>/buttonNorth",
                    "<Gamepad>/dpad/up",
                    "<Gamepad>/dpad/down",
                    "<Gamepad>/dpad/left",
                    "<Gamepad>/dpad/right",
                    "<Gamepad>/leftStick/up",
                    "<Gamepad>/leftStick/down",
                    "<Gamepad>/leftStick/left",
                    "<Gamepad>/leftStick/right",
                    "<Gamepad>/rightStick/up",
                    "<Gamepad>/rightStick/down",
                    "<Gamepad>/rightStick/left",
                    "<Gamepad>/rightStick/right",
                    "<Gamepad>/leftTrigger",
                    "<Gamepad>/rightTrigger",
                    "<Gamepad>/leftShoulder",
                    "<Gamepad>/rightShoulder",
                    "<Gamepad>/start",
                    "<Gamepad>/select",
                    "<Gamepad>/leftStickPress",
                    "<Gamepad>/rightStickPress"
    };

    private string[] keys;
    private int index;
    private string backInput;
    private string forwardInput;
    private string jumpInput;
    private string interactInput;
    private Queue<string> interactQueue;
    private Queue<string> backQueue;
    private Queue<string> forwardQueue;
    private Queue<string> jumpQueue;
    private float timeSpanInteract;
    private float timeSpanBack;
    private float timeSpanForward;
    private float timeSpanJump;
    private string[] currentKeys = new string[7];
    private int currentKeyIndex;
    public float minInteract = 2;
    public float minBack= 10;
    public float minForward = 10;
    public float minJump = 10;
    public float maxInteract = 5;
    public float maxBack = 30;
    public float maxForward = 30;
    public float maxJump = 30;

    private void Awake()
    {
        if (playerInput == null)
            playerInput = new Actions();

        playerInput.Player.Enable();
        if (currentDevice == devices.controller) ChangeInputDevice(devices.controller);
        else ChangeInputDevice(devices.keyboard);
        
    }
    void Start()
    { 

        interactButton = GameObject.FindWithTag("InteractButton");
        timeSpanInteract = 0;
        timeSpanBack = 0;
        timeSpanForward = 0;
        timeSpanJump = 0;
        interactQueue = new Queue<string>();
        backQueue = new Queue<string>();
        forwardQueue = new Queue<string>();
        jumpQueue = new Queue<string>();

        
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
            playerInput.Player.Interact.ApplyBindingOverride(interactInput);
            UpdateInteractSprite();
        }
        if (randomizeBack && timeSpanBack <= 0)
        {
            backInput = Randomize(ref timeSpanBack, ref backQueue, minBack, maxBack);
            playerInput.Player.Back.ApplyBindingOverride(backInput);
            UpdateBackSprite();
        }
        if (randomizeForward && timeSpanForward <= 0)
        {
            forwardInput = Randomize(ref timeSpanForward, ref forwardQueue, minForward, maxForward);
            playerInput.Player.Forward.ApplyBindingOverride(forwardInput);
            UpdateForwardSprite();
        }
        if (randomizeJump && timeSpanJump <= 0)
        {
            jumpInput = Randomize(ref timeSpanJump, ref jumpQueue, minJump, maxJump);
            playerInput.Player.Jump.ApplyBindingOverride(jumpInput);
            UpdateJumpSprite();
        }   
    }

    string Randomize(ref float timeSpan, ref Queue<string> queue, float min, float max)
    {
        string newInput;
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

    string Randomize(ref float timeSpan, float min, float max)
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

    public string Randomize(Queue<string> queue, bool addToQueue)
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

    public string GetBack()
    {
        return  backInput ;
    }
    public string GetForward()
    {
        return forwardInput ;
    }
    public string GetJump()
    {
        return jumpInput ;
    }
    public string GetInteract()
    {
        return interactInput;
    }

    public void setTimer(bool timer)
    {
        timerActive = timer;
    }

    public Queue<string> GetInteractQueue()
    {
        return interactQueue;
    }

    public Queue<string> GetJumpQueue()
    {
        return jumpQueue;
    }

    public Queue<string> GetForwardQueue()
    {
        return forwardQueue;
    }

    public Queue<string> GetBackQueue()
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

    public void setJump(string jump)
    {
        jumpInput = jump;
        playerInput.Player.Jump.ApplyBindingOverride(jump);
        UpdateJumpSprite();
    }

    public void setForward(string forward)
    {
        forwardInput = forward;
        playerInput.Player.Forward.ApplyBindingOverride(forward);
        UpdateForwardSprite();
    }

    public void setBack(string back)
    {
        backInput = back;
        playerInput.Player.Back.ApplyBindingOverride(back);
        UpdateBackSprite();
    }

    public void setDefaultJump()
    {
        switch (currentDevice)
        {
            case devices.keyboard:
                jumpInput = "<Keyboard>/space";
                break;
            case devices.controller:
                jumpInput = "<Gamepad>/buttonSouth";
                break;
        }        
        playerInput.Player.Jump.ApplyBindingOverride(jumpInput);
        UpdateJumpSprite();
    }

    public void setDefaultForward()
    {
        switch (currentDevice)
        {
            case devices.keyboard:
                forwardInput = "<Keyboard>/d";
                break;
            case devices.controller:
                forwardInput = "<Gamepad>/leftStick/right";
                break;
        }
        playerInput.Player.Forward.ApplyBindingOverride(forwardInput);
        UpdateForwardSprite();
    }

    public void setDefaultBack()
    {
        switch (currentDevice)
        {
            case devices.keyboard:
                backInput = "<Keyboard>/a";
                break;
            case devices.controller:
                backInput = "<Gamepad>/leftStick/left";
                break;
        }
        playerInput.Player.Back.ApplyBindingOverride(backInput);
        UpdateBackSprite();
    }

    public void setDefaultInteract()
    {
        switch (currentDevice)
        {
            case devices.keyboard:
                backInput = "<Keyboard>/e";
                break;
            case devices.controller:
                backInput = "<Gamepad>/buttonNorth";
                break;
        }
        playerInput.Player.Interact.ApplyBindingOverride(interactInput);
        UpdateInteractSprite();
    }

    public void ChangeInputDevice(devices device)
    {

        switch(device)
        {
            case devices.keyboard:
                keys = new string[]{
                    "<Keyboard>/a", "<Keyboard>/b", "<Keyboard>/c", "<Keyboard>/d", "<Keyboard>/e",
                    "<Keyboard>/f", "<Keyboard>/g", "<Keyboard>/h", "<Keyboard>/i", "<Keyboard>/j",
                    "<Keyboard>/k", "<Keyboard>/l", "<Keyboard>/m", "<Keyboard>/n", "<Keyboard>/o",
                    "<Keyboard>/p", "<Keyboard>/q", "<Keyboard>/r", "<Keyboard>/s", "<Keyboard>/t",
                    "<Keyboard>/u", "<Keyboard>/v", "<Keyboard>/w", "<Keyboard>/x", "<Keyboard>/y", "<Keyboard>/z",
                    "<Keyboard>/1", "<Keyboard>/2", "<Keyboard>/3", "<Keyboard>/4", "<Keyboard>/5",
                    "<Keyboard>/6", "<Keyboard>/7", "<Keyboard>/8", "<Keyboard>/9", "<Keyboard>/0",
                    "<Keyboard>/space", "<Keyboard>/up", "<Keyboard>/down", "<Keyboard>/left", "<Keyboard>/right"
                };               
                currentDevice = devices.keyboard;
                break;
            case devices.controller:
                keys = new string[]
                {
                    "<Gamepad>/buttonSouth",    
                    "<Gamepad>/buttonEast",     
                    "<Gamepad>/buttonWest",     
                    "<Gamepad>/buttonNorth",    
                    "<Gamepad>/dpad/up",
                    "<Gamepad>/dpad/down",
                    "<Gamepad>/dpad/left",
                    "<Gamepad>/dpad/right",
                    "<Gamepad>/leftStick/up",
                    "<Gamepad>/leftStick/down",
                    "<Gamepad>/leftStick/left",
                    "<Gamepad>/leftStick/right",
                    "<Gamepad>/rightStick/up",
                    "<Gamepad>/rightStick/down",
                    "<Gamepad>/rightStick/left",
                    "<Gamepad>/rightStick/right",
                    "<Gamepad>/leftTrigger",
                    "<Gamepad>/rightTrigger",
                    "<Gamepad>/leftShoulder",
                    "<Gamepad>/rightShoulder",
                    "<Gamepad>/start",          
                    "<Gamepad>/select",         
                    "<Gamepad>/leftStickPress",
                    "<Gamepad>/rightStickPress"
                };
                
                currentDevice = devices.controller;
                break;
        }
        if (randomizeBack)
            Randomize(backQueue, true);
        else
        {
            setDefaultBack();
        }
        if (randomizeForward)
            Randomize(forwardQueue, true);
        else
        {
            setDefaultForward();
        }
        if (randomizeJump)
            Randomize(jumpQueue, true);
        else
        {
            setDefaultJump();
        }
        if (randomizeInteract)
            Randomize(interactQueue, true);
        else
        {
            setDefaultInteract();
        }
    }
}
