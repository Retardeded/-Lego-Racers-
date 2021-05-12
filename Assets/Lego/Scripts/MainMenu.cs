using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        SceneManager.LoadScene(GameManager.sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetDifficulty(int difficultyIndex)
    {
        GameManager.SetDifficultyOptions(difficultyIndex);
    }

    public void SetGameMode(int gameModeIndex)
    {
        GameManager.sceneToLoad = gameModeIndex+1;
    }
}
