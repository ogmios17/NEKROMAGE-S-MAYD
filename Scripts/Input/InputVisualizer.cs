using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
    public InputMap[] inputSprites;
    
    public Sprite getSprite(KeyCode key)
    {
        foreach(InputMap i in inputSprites)
        {
            if (i.getInput() == key)
                return i.getSprite();
        }
        return null;
    }
}

[System.Serializable]
public class InputMap
{
    public KeyCode input;
    public Sprite sprite;

    public KeyCode getInput()
    {
        return input;
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}
