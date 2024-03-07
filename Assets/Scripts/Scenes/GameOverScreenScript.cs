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
