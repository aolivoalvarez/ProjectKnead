/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For the Door prefab. Keeps track of this door's number, as well as the scene and door number it will move the player to.
-----------------------------------------*/

// PARTS OF SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using System.Collections;
using UnityEngine;
using CustomAttributes;

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
    [SerializeField] bool isDungeonEntrance = false;
    [ShowIf(ActionOnConditionFail.DontDraw, ConditionOperator.And, nameof(isDungeonEntrance))]
    public Transform initialCameraPosition;

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
