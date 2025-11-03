
using UnityEngine;

public class InputVisualizer : MonoBehaviour
{
    public InputRandomizer randomizer;
    public InputMap[] keyboardSprites;
    public InputMap[] sonySprites;
    public InputMap[] xboxSprites;
    public InputMap[] nintendoSprites;
    private InputMap[] activeSprites;
    private int controllerIndex;
    string[] joystickNames;

    public enum controllers { Sony, Xbox, Nintendo, Keyboard };

    public void Start()
    {

    }
    public Sprite getSprite(string key)
    {
        controllers currentController = getControllerType();

        foreach (InputMap i in activeSprites)
        {
            if (i.getInput().Equals(key))
                return i.getSprite();
        }
        return null;
    }

    public controllers getControllerType()
    {
        joystickNames = Input.GetJoystickNames();
        if(randomizer.currentDevice != InputRandomizer.devices.keyboard) { 
            foreach(string name in joystickNames)
            {
                if(name.Contains("ps4") || name.Contains("ps5"))
                {
                    activeSprites = sonySprites;
                    return controllers.Sony;
                }
                else if (name.Contains("Xbox"))
                {
                    activeSprites = xboxSprites;
                    return controllers.Xbox;
                }
                else if (name.Contains("nintendo"))
                {
                    activeSprites = nintendoSprites;
                    return controllers.Nintendo;
                }

            }
        }
        activeSprites = keyboardSprites;
        return controllers.Keyboard;
    }

    /*private void OnValidate()
    {
        nintendoSprites= new InputMap[23];
        for(int i =0; i<nintendoSprites.Length; i++)
        {
            nintendoSprites[i]= new InputMap(randomizer.inputPool[i+41]);
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
