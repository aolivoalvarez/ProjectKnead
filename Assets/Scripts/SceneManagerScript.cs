using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject); // This gameObject will persist between scene changes

        // If there is another SceneManagerScript in the scene, destroy self
        if (FindObjectsOfType<SceneManagerScript>().Length > 1)
            Destroy(gameObject);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
