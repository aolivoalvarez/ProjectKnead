using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; //gets pause menu panel
    private bool menuToggle = true; //toggle bool for if the menu is on or off

    void Start()
    {
        pauseMenu.SetActive(menuToggle); //disables pause menu on start
    }

    private void Update()
    {
       // PauseMenuInput
    }

    public void TogglePauseMenu() //toggles pause menu on or off
    {
        pauseMenu.SetActive(!menuToggle); //turns menu on or off based on toggle value
    }

    public void pressStart() //function for start button -- goes back to title screen
    {
        SceneManager.LoadSceneAsync("TitleScreen"); //opens TitleSreen scene
    }
    
    public void pressContinue() //function for continue button -- toggles pause menu to take it off
    {
        TogglePauseMenu();
    }

    public void pressExit() //function for exit button -- goes to game over screen
    {
        SceneManager.LoadSceneAsync("GameOverScreen"); //opens GameOverScreen scene
    }
}
