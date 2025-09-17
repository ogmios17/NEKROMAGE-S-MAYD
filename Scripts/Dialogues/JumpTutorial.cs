using System;
using UnityEngine;

public class JumpTutorial : MonoBehaviour
{
    public InputRandomizer randomizer;
    private TriggeredDialogue[] dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogue = gameObject.GetComponents<TriggeredDialogue>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setDialogueToPlay(int option)
    {
        for (int i = 0; i < dialogue.Length; i++){
            if (i != option)
            {
                dialogue[i].isActive=false;
            }
            else dialogue[i].isActive=true;
        }
    }

    public void RandomizeJump()
    {
        randomizer.jumpInput = randomizer.Randomize(randomizer.GetJumpQueue(), false);
        randomizer.UpdateJumpSprite();
    }


}
