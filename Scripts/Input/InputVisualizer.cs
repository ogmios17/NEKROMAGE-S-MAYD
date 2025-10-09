
using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
    public InputRandomizer randomizer;
    public InputMap[] inputSprites;
    
    public Sprite getSprite(string key)
    {
        foreach(InputMap i in inputSprites)
        {
            if (i.getInput().Equals(key))
                return i.getSprite();
        }
        return null;
    }

    /*private void OnValidate()
    {
        inputSprites= new InputMap[randomizer.inputPool.Length];
        for(int i =0; i<inputSprites.Length; i++)
        {
            inputSprites[i]= new InputMap(randomizer.inputPool[i]);
        }
    }*/
}

[System.Serializable]
public class InputMap
{
    public string input;
    public Sprite sprite;

    public InputMap(string input)
    {
        this.input = input;
    }

    public string getInput()
    {
        return input;
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}
