/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Handles most scene change functions.
-----------------------------------------*/

// PARTS OF SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    enum SceneType
    {
        Standard,
        Dungeon,
        NonPlayer
    }

    public static SceneManagerScript instance;

    public SceneField[] dungeons, nonPlayerScenes;
    SceneType currentSceneType;
    static bool loadedFromDoor;
    SceneChangeDoorScript.DoorToSpawnAt doorToSpawnPlayer;
    Vector2 playerSpawnPosition;
    Vector3 dungeonCameraPosition;

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

    public static void SwapScene(SceneField scene)
    {
        instance.StartCoroutine(instance.FadeOutThenChangeScene(scene));
    }
    public static void SwapScene(int sceneIndex)
    {
        instance.StartCoroutine(instance.FadeOutThenChangeScene(sceneIndex));
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
    IEnumerator FadeOutThenChangeScene(int sceneIndex, SceneChangeDoorScript.DoorToSpawnAt doorToSpawnAt = SceneChangeDoorScript.DoorToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        doorToSpawnPlayer = doorToSpawnAt;
        SceneManager.LoadScene(sceneIndex);
    }

    void OnSceneChanged(Scene lastScene, Scene currentScene)
    {
        SceneFadeManager.instance.StartFadeIn();

        currentSceneType = SceneType.Standard;
        foreach (SceneField s in dungeons)
        {
            if (s.SceneName == currentScene.name)
            {
                currentSceneType = SceneType.Dungeon;
                break;
            }
        }
        foreach (SceneField s in nonPlayerScenes)
        {
            if (s.SceneName == currentScene.name)
            {
                currentSceneType = SceneType.NonPlayer;
                break;
            }
        }

        switch (currentSceneType)
        {
            case SceneType.NonPlayer:
                PlayerController.instance.gameObject.SetActive(false);
                InventoryMenuScript.instance.iInput.Disable();
                break;
            case SceneType.Dungeon:
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                PlayerController.instance.gameObject.SetActive(true);
                InventoryMenuScript.instance.iInput.Enable();
                GameManager.instance.UpdatePlayerHearts();
                break;
            default:
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                PlayerController.instance.gameObject.SetActive(true);
                InventoryMenuScript.instance.iInput.Enable();
                GameManager.instance.UpdatePlayerHearts();
                break;
        }

        if (loadedFromDoor)
        {
            FindDoor(doorToSpawnPlayer);
            if (currentSceneType == SceneType.Dungeon)
            {
                Camera.main.transform.position = dungeonCameraPosition;
                var camControl = Camera.main.GetComponent<DungeonCameraController>();
                camControl.minPos = dungeonCameraPosition;
                camControl.maxPos = dungeonCameraPosition;
            }
            PlayerController.instance.gameObject.transform.position = playerSpawnPosition;
            CheckpointScript.instance.transform.position = playerSpawnPosition;
            RespawnPointScript.instance.transform.position = playerSpawnPosition;
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
                if (currentSceneType == SceneType.Dungeon)
                    dungeonCameraPosition = d.initialCameraPosition.position;
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
}
