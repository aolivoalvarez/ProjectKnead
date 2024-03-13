/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a GameObject in the TitleScreen scene. Contains relevant functions to assign UI buttons to.
-----------------------------------------*/

using UnityEngine;

public class TitleScreenScript : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] SceneField startGameScene;
    [SerializeField] SceneField gameOverScene;

    void Start()
    {
        optionsMenu.SetActive(false);
    }

    public void ToggleOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void StartGame()
    {
        SceneManagerScript.SwapScene(startGameScene);
    }

    public void GameOver()
    {
        SceneManagerScript.SwapScene(gameOverScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
