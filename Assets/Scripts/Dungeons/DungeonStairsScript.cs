/*-----------------------------------------
Creation Date: 4/9/2024 4:57:13 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    AudioSource audi;

    DungeonCameraController camControl;
    DungeonManager dungeonManager;

    void Start()
    {
        audi = GetComponent<AudioSource>();
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

    IEnumerator StairsRoutine(Collider2D other)
    {
        GameManager.instance.DisablePlayerInput();
        audi.clip = AudioManager.instance.soundFX[30];
        audi.loop = true;
        audi.Play();
        yield return new WaitForSeconds(0.5f);
        camControl.minPos = connectedStairs.initialCameraPosition.position;
        camControl.maxPos = connectedStairs.initialCameraPosition.position;
        camControl.transform.position = connectedStairs.initialCameraPosition.position;
        other.transform.position = connectedStairs.playerSpawnPosition.position;
        other.GetComponent<PlayerController>().lookDirection = (other.transform.position - connectedStairs.transform.position).normalized;
        CheckpointScript.instance.transform.position = other.transform.position;
        if (isRespawnPoint) RespawnPointScript.instance.transform.position = other.transform.position;
        dungeonManager.ChangeFloor();
        dungeonManager.ChangeRoom();
        yield return new WaitForSeconds(0.5f);
        audi.Stop();
        GameManager.instance.EnablePlayerInput();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StairsRoutine(other));
            
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        var labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, thisStairs.ToString(), labelStyle);
    }
#endif
}
