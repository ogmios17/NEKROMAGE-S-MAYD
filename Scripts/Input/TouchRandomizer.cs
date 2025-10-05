using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchRandomizer : MonoBehaviour
{
    public InputRandomizer randomizer;
    public bool changeJump;
    public bool changeForward;
    public bool changeBack;
    private Queue<KeyCode> backQueue;
    private Queue<KeyCode> forwardQueue;
    private Queue<KeyCode> jumpQueue;
    private bool charged = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(randomizer == null)
        {
            randomizer = GameObject.FindWithTag("SceneHandler").GetComponent<InputRandomizer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && charged)
        {
            if (changeBack)
            {
                randomizer.setBack(randomizer.Randomize(randomizer.GetBackQueue(), false));
            }
            if (changeForward)
            {
                randomizer.setForward(randomizer.Randomize(randomizer.GetForwardQueue(), false));
            }
            if (changeJump)
            {
                randomizer.setJump(randomizer.Randomize(randomizer.GetJumpQueue(), false));
            }

            charged = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) charged = true;
    }
}
