using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
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
}

[System.Serializable]
public class InputMap
{
    public string input;
    public Sprite sprite;

    public string getInput()
    {
        return input;
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}
