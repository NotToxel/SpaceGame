//PauseMenu inspired by Test Subject Gaming on Youtube
//https://www.youtube.com/watch?v=3zEpfMbE30s



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{

    // Handles player input
    private PlayerControls playerControls;
    private InputAction menu;

    [SerializeField] private GameObject pauseUI;
    private bool isPaused; // tracks whether game is paused

    // This is called when the script is loaded
    void Awake()
    {
        playerControls = new PlayerControls();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Runs when the objectt is active
    private void OnEnable()
    {
        menu = playerControls.Menu.Escape;
        menu.Enable();
        menu.performed += Pause;
    }
    
    // Runs when the object is no longer active
    private void OnDisable()
    {
        menu.Disable();
    }
    
    // Called when escape[enter] key is pressed 
    void Pause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }

    // Pull up the pause menu and stop the gameworlds time
    private void ActivateMenu()
    {
        Time.timeScale = 0;
        AudioListener.pause = true;
        pauseUI.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // close the pause menu and resume gameworlds time
    public void DeactivateMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
    }
    
    // Load the main menu scene
    public void MainScreen()
	{
 	Time.timeScale = 1;
        AudioListener.pause = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
	SceneManager.LoadSceneAsync(0);
    }

    // Quit the game
    public void QuitGame(){
		Application.Quit();
	}
}
