using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject uiGameOverWindow;
    public GameObject uiPauseWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle escape button
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiPauseWindow.SetActive(!uiPauseWindow.activeSelf);
        }
    }
    //return the playing game scene 
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
    // returns the playing game scene
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // load game scene
    }
    // returns the main menu screen
    public void MainMenuButton()
    {
        SceneManager.LoadScene(0); // load game scene
    }
    // quits the game ( doesn't work)
    public void QuitGame()
    {
        Application.Quit();
    }
}
