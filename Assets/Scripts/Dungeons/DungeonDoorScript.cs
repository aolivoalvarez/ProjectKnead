/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: When attached to a door transition trigger, tells DungeonCameraController when to move the camera in a dungeon.
-----------------------------------------*/

// SCRIPT TAKEN FROM A VIDEO BY EXPAT STUDIOS
using UnityEngine;

public class DungeonDoorScript : MonoBehaviour
{
    [SerializeField] Vector3 newCamPos, newPlayerPos;
    DungeonCameraController camControl;

    void Start()
    {
        camControl = Camera.main.GetComponent<DungeonCameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            camControl.minPos += newCamPos;
            camControl.maxPos += newCamPos;

            other.transform.position += newPlayerPos;
            CheckpointScript.instance.transform.position = other.transform.position;
        }
    }
}
