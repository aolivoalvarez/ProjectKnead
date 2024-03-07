// PARTS OF SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    public SceneField[] dungeons;
    static bool loadedFromDoor;
    SceneChangeDoorScript.DoorToSpawnAt doorToSpawnPlayer;
    Vector2 playerSpawnPosition;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public static void SwapSceneFromDoorUse(SceneField scene, SceneChangeDoorScript.DoorToSpawnAt doorToSpawnAt)
    {
        loadedFromDoor = true;
        instance.StartCoroutine(instance.FadeOutThenChangeScene(scene, doorToSpawnAt));
    }

    IEnumerator FadeOutThenChangeScene(SceneField scene, SceneChangeDoorScript.DoorToSpawnAt doorToSpawnAt = SceneChangeDoorScript.DoorToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        doorToSpawnPlayer = doorToSpawnAt;
        SceneManager.LoadScene(scene);
    }

    void OnSceneChanged(Scene lastScene, Scene currentScene)
    {
        SceneFadeManager.instance.StartFadeIn();

        foreach (SceneField s in dungeons)
        {
            if (s.SceneName == currentScene.name)
            {
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
            }
            else
            {
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
            }
        }

        if (loadedFromDoor)
        {
            FindDoor(doorToSpawnPlayer);
            PlayerController.instance.gameObject.transform.position = playerSpawnPosition;
            GameObject.FindGameObjectWithTag("Checkpoint").gameObject.transform.position = playerSpawnPosition;
            loadedFromDoor = false;
        }
        StartCoroutine(EnablePlayerInput());
    }

    void FindDoor(SceneChangeDoorScript.DoorToSpawnAt doorSpawnNumber)
    {
        SceneChangeDoorScript[] doors = FindObjectsOfType<SceneChangeDoorScript>();

        foreach (SceneChangeDoorScript d in doors)
        {
            if (d.currentDoorPosition == doorSpawnNumber)
            {
                playerSpawnPosition = d.playerSpawnPosition.position;
                return;
            }
        }
    }

    IEnumerator EnablePlayerInput()
    {
        while (SceneFadeManager.instance.isFadingIn || PlayerController.instance.pInput == null)
        {
            yield return null;
        }
        PlayerController.instance.pInput.Enable();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
