/*-----------------------------------------
Creation Date: 4/9/2024 4:57:13 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using UnityEditor;
using UnityEngine;

public class DungeonStairsScript : MonoBehaviour
{
    public enum StairsID
    {
        A, B, C, D, E, F, G, H
    }

    [Header("This Stairs")]
    public StairsID thisStairs;
    public Transform playerSpawnPosition;
    public Transform initialCameraPosition;
    public bool isRespawnPoint = false;

    [Header("Connected Stairs")]
    [SerializeField] StairsID stairsToSpawnAt;
    DungeonStairsScript connectedStairs;

    DungeonCameraController camControl;
    DungeonManager dungeonManager;

    void Start()
    {
        camControl = Camera.main.GetComponent<DungeonCameraController>();
        dungeonManager = DungeonManager.instance;
        FindOtherStairs();
    }

    void FindOtherStairs()
    {
        foreach (var stairs in FindObjectsOfType<DungeonStairsScript>())
        {
            if (stairs != this && stairs.thisStairs == thisStairs)
            {
                connectedStairs = stairs;
                return;
            }
        }
    }

    IEnumerator StairsRoutine()
    {
        GameManager.instance.DisablePlayerInput();
        yield return new WaitForSeconds(1f);
        GameManager.instance.EnablePlayerInput();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            camControl.minPos = connectedStairs.initialCameraPosition.position;
            camControl.maxPos = connectedStairs.initialCameraPosition.position;
            camControl.transform.position = connectedStairs.initialCameraPosition.position;

            StartCoroutine(StairsRoutine());
            other.transform.position = connectedStairs.playerSpawnPosition.position;
            other.GetComponent<PlayerController>().lookDirection = (other.transform.position - connectedStairs.transform.position).normalized;
            CheckpointScript.instance.transform.position = other.transform.position;
            if (isRespawnPoint) RespawnPointScript.instance.transform.position = other.transform.position;

            dungeonManager.ChangeFloor();
            dungeonManager.ChangeRoom();
        }
    }

    void OnDrawGizmos()
    {
        var labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, thisStairs.ToString(), labelStyle);
    }
}
