using System.Collections.Generic;
using UnityEngine;

public class TouchRandomizer : MonoBehaviour
{
    public InputRandomizer randomizer;
    public bool changeJump;
    public bool changeForward;
    public bool changeBack;
    private Queue<KeyCode> backQueue;
    private Queue<KeyCode> forwardQueue;
    private Queue<KeyCode> jumpQueue;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            if (changeBack)
            {
                randomizer.backInput = randomizer.Randomize(randomizer.GetBackQueue(), false);
                randomizer.UpdateBackSprite();
            }
            if (changeForward)
            {
                randomizer.forwardInput=randomizer.Randomize(randomizer.GetForwardQueue(), false); 
                randomizer.UpdateForwardSprite();
            }
            if (changeJump)
            {
                randomizer.jumpInput=randomizer.Randomize(randomizer.GetJumpQueue(), false);
                randomizer.UpdateJumpSprite();
            }
        }
    }
}
