using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	//starts the game and locks cursor to the game
	public void Playgame()
	{
		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
		SceneManager.LoadSceneAsync(1);

	}

	// quits out of the application
	public void QuitGame(){
		Application.Quit();
	}

}
