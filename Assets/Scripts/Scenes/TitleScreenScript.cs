/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a GameObject in the TitleScreen scene. Contains relevant functions to assign UI buttons to.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.UI;

public class TitleScreenScript : MonoBehaviour
{
    [SerializeField] GameObject optionsMenu;
    [SerializeField] SceneField startGameScene;
    [SerializeField] SceneField gameOverScene;

    void Start()
    {
        optionsMenu.SetActive(false);
        GameObject.Find("Button_Start").GetComponent<Button>().Select();
    }

    public void ToggleOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        if (optionsMenu.activeSelf) GameObject.Find("Button_Back").GetComponent<Button>().Select();
        else GameObject.Find("Button_Credits").GetComponent<Button>().Select();
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
