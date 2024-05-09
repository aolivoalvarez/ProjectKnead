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
using UnityEngine.U2D;

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
    SceneChangeDoorScript.DoorToSpawnAt doorToRespawnPlayer;
    Vector2 playerSpawnPosition;
    Vector3 dungeonCameraPosition;
    PolygonCollider2D camConfiner;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//

        doorToRespawnPlayer = SceneChangeDoorScript.DoorToSpawnAt.One;
    }

    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void Update()
    {
        if (camConfiner != null && FindObjectOfType<CinemachineConfiner2D>() != null && FindObjectOfType<CinemachineConfiner2D>().m_BoundingShape2D != camConfiner)
        {
            FindObjectOfType<CinemachineConfiner2D>().m_BoundingShape2D = camConfiner;
        }
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
        if (SceneFadeManager.instance.isFadingOut || SceneFadeManager.instance.isFadingIn)
            yield break;

        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        doorToSpawnPlayer = doorToSpawnAt;
        if (doorToSpawnPlayer != SceneChangeDoorScript.DoorToSpawnAt.None) doorToRespawnPlayer = doorToSpawnPlayer;
        SceneManager.LoadScene(scene);
    }
    IEnumerator FadeOutThenChangeScene(int sceneIndex, SceneChangeDoorScript.DoorToSpawnAt doorToSpawnAt = SceneChangeDoorScript.DoorToSpawnAt.None)
    {
        if (SceneFadeManager.instance.isFadingOut || SceneFadeManager.instance.isFadingIn)
            yield break;

        SceneFadeManager.instance.StartFadeOut();

        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }

        doorToSpawnPlayer = doorToSpawnAt;
        if (doorToSpawnPlayer != SceneChangeDoorScript.DoorToSpawnAt.None) doorToRespawnPlayer = doorToSpawnPlayer;
        SceneManager.LoadScene(sceneIndex);
    }

    void OnSceneChanged(Scene lastScene, Scene currentScene)
    {
        SceneFadeManager.instance.StartFadeIn(currentScene.name);

        currentSceneType = SceneType.Standard;
        for (int i = 0; i < dungeons.Length; i++)
        {
            if (dungeons[i].SceneName == currentScene.name)
            {
                currentSceneType = SceneType.Dungeon;
                DungeonManager.instance.currentDungeon = i;
                break;
            }
            else
            {
                DungeonManager.instance.currentDungeon = -1;
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
                Camera.main.GetComponent<DungeonCameraController>().dungeonBorder.SetActive(false);
                break;
            case SceneType.Dungeon:
                Camera.main.GetComponent<CinemachineBrain>().enabled = false;
                Camera.main.GetComponent<PixelPerfectCamera>().cropFrameX = true;
                Camera.main.GetComponent<PixelPerfectCamera>().cropFrameY = true;
                Camera.main.GetComponent<DungeonCameraController>().dungeonBorder.SetActive(true);
                PlayerController.instance.gameObject.SetActive(true);
                InventoryMenuScript.instance.iInput.Enable();
                GameManager.instance.UpdatePlayerHearts();
                break;
            default:
                Camera.main.GetComponent<CinemachineBrain>().enabled = true;
                PolygonCollider2D[] colliders = FindObjectsOfType<PolygonCollider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject.CompareTag("VCamConfiner"))
                    {
                        camConfiner = colliders[i];
                    }
                }
                Camera.main.GetComponent<PixelPerfectCamera>().cropFrameX = false;
                Camera.main.GetComponent<PixelPerfectCamera>().cropFrameY = false;
                Camera.main.GetComponent<DungeonCameraController>().dungeonBorder.SetActive(false);
                PlayerController.instance.gameObject.SetActive(true);
                InventoryMenuScript.instance.iInput.Enable();
                GameManager.instance.UpdatePlayerHearts();
                break;
        }

        if (currentScene.name == "NewPawTown")
        {
            PlayerController.instance.transform.position = Vector3.zero;
            PlayerController.instance.lookDirection = Vector2.down;
        }
        if (loadedFromDoor || currentSceneType == SceneType.Dungeon)
        {
            //Debug.Log(doorToSpawnPlayer);
            if (loadedFromDoor)
            {
                FindDoor(doorToSpawnPlayer);
            }
            else
                FindDoor(doorToRespawnPlayer);

            PlayerController.instance.gameObject.transform.position = playerSpawnPosition;
            CheckpointScript.instance.transform.position = playerSpawnPosition;
            RespawnPointScript.instance.transform.position = playerSpawnPosition;

            if (currentSceneType == SceneType.Dungeon)
            {
                Camera.main.transform.position = dungeonCameraPosition;
                var camControl = Camera.main.GetComponent<DungeonCameraController>();
                camControl.minPos = dungeonCameraPosition;
                camControl.maxPos = dungeonCameraPosition;

                DungeonManager.instance.InitializeDungeon();
            }
            
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
