using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
    
{
    public GameObject pauseMenu;
    public Actions menu;
    private bool isPaused = false;
    public Canvas rebindCanvas;
    public Canvas standardCanvas;
    private InputActionRebindingExtensions.RebindingOperation operation;
    public InputRandomizer randomizer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rebindCanvas.enabled = false;
        if (menu == null)
            menu = new Actions();
        menu.Player.Enable();
    }
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (menu.Player.OpenMenu.triggered){
            if(isPaused){
                standardCanvas.enabled = true;
                rebindCanvas.enabled = false;
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenRebindMenu()
    {
        standardCanvas.enabled = false;
        rebindCanvas.enabled = true;
        
    }

    public void Rebind(string action)
    {
        var currentAction = menu.FindAction(action);
        currentAction.Disable();
        operation = menu.FindAction(action).PerformInteractiveRebinding().Start();
        currentAction.Enable();
    }

}
