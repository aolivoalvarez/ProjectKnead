/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a GameObject in the GameOverScreen scene. Contains relevant functions to assign UI buttons to.
-----------------------------------------*/

using UnityEngine;

public class GameOverScreenScript : MonoBehaviour
{
    [SerializeField] SceneField titleScene;
    
    public void ContinueGame()
    {
        GameManager.instance.RespawnAtRespawnPoint();
    }

    public void ReturnToTitle()
    {
        SceneManagerScript.SwapScene(titleScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
