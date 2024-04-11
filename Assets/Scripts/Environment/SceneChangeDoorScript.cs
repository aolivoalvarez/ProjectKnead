/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: For the Door prefab. Keeps track of this door's number, as well as the scene and door number it will move the player to.
-----------------------------------------*/

// PARTS OF SCRIPT TAKEN FROM A VIDEO BY SASQUATCH B STUDIOS
using System.Collections;
using UnityEngine;
using CustomAttributes;
using UnityEditor;

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
        if (isDungeonEntrance)
        {
            DungeonManager.instance.LeaveDungeon();
        }
        SceneManagerScript.SwapSceneFromDoorUse(sceneToLoad, doorToSpawnAt);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        var labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, currentDoorPosition.ToString(), labelStyle);
    }
#endif
}
