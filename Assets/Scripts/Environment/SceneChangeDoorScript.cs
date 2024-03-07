// PARTS OF SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using System.Collections;
using UnityEngine;

public class SceneChangeDoorScript : MonoBehaviour
{
    public enum DoorToSpawnAt
    {
        None,
        One,
        Two,
        Three,
        Four
    }

    [Header("This Door")]
    public DoorToSpawnAt currentDoorPosition;
    public Transform playerSpawnPosition;

    [Header("Connected Door")]
    [SerializeField] SceneField sceneToLoad;
    public DoorToSpawnAt doorToSpawnAt;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EnterDoorRoutine());
        }
    }

    IEnumerator EnterDoorRoutine()
    {
        PlayerController.instance.pInput.Disable();
        yield return new WaitForSeconds(0f);
        SceneManagerScript.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnAt);
    }
}
