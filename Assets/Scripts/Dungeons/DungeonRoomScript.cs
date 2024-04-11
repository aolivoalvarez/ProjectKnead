/*-----------------------------------------
Creation Date: 4/7/2024 6:47:15 PM
Author: theco
Description: Saves/loads all relevant objects and their states in a given room.
-----------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

public class RoomState
{
    public List<bool> chests;
    public List<bool> respawnOnFloorChange;
    public List<bool> respawnOnLeave;
    public List<bool> dontRespawn;
}

public class DungeonRoomScript : MonoBehaviour
{
    public int roomIndex = 0;

    [SerializeField] GameObject chestsParent;
    [SerializeField] GameObject respawnOnFloorChangeParent;
    [SerializeField] GameObject respawnOnLeaveParent;
    [SerializeField] GameObject dontRespawnParent;

    public List<GameObject> chests { get; private set; }
    public List<GameObject> respawnOnFloorChange { get; private set; }
    public List<GameObject> respawnOnLeave { get; private set; }
    public List<GameObject> dontRespawn { get; private set; }

    public RoomState currentState;

    void Start()
    {
        InitializeObjectLists();
        InitializeStateLists();
        LoadFromStateLists();
    }

    public RoomState SaveToStateLists()
    {
        for (int i = 0; i < chests.Count; i++)
        {
            if (chests[i].GetComponent<ChooseChestPickup>().chestOpened) currentState.chests[i] = false;
        }
        for (int i = 0; i < respawnOnFloorChange.Count; i++)
        {
            if (respawnOnFloorChange[i] == null) currentState.respawnOnFloorChange[i] = false;
            else currentState.respawnOnFloorChange[i] = respawnOnFloorChange[i].activeSelf;
        }
        for (int i = 0; i < respawnOnLeave.Count; i++)
        {
            if (respawnOnLeave[i] == null) currentState.respawnOnLeave[i] = false;
            else currentState.respawnOnLeave[i] = respawnOnLeave[i].activeSelf;
        }
        for (int i = 0; i < currentState.dontRespawn.Count; i++)
        {
            if (dontRespawn[i] == null) currentState.dontRespawn[i] = false;
            //else currentState.dontRespawn[i] = dontRespawn[i].activeSelf;
        }

        return currentState;
    }

    void LoadFromStateLists()
    {
        for (int i = 0; i < chests.Count; i++)
        {
            if (!currentState.chests[i]) chests[i].GetComponent<ChooseChestPickup>().OpenChest();
        }
        for (int i = 0; i < respawnOnFloorChange.Count; i++)
        {
            if (!currentState.respawnOnFloorChange[i]) respawnOnFloorChange[i].SetActive(false);
        }
        for (int i = 0; i < respawnOnLeave.Count; i++)
        {
            if (!currentState.respawnOnLeave[i]) respawnOnLeave[i].SetActive(false);
        }
        for (int i = 0; i < currentState.dontRespawn.Count; i++)
        {
            if (!currentState.dontRespawn[i]) Destroy(dontRespawn[i].gameObject);
        }
    }

    void InitializeObjectLists()
    {
        chests = new();
        respawnOnFloorChange = new();
        respawnOnLeave = new();
        dontRespawn = new();

        foreach (var chest in chestsParent.GetComponentsInChildren<ChooseChestPickup>(true))
            chests.Add(chest.gameObject);
        foreach (var tf in respawnOnFloorChangeParent.GetComponentsInChildren<Transform>(true))
            if (tf != respawnOnFloorChangeParent) respawnOnFloorChange.Add(tf.gameObject);
        foreach (var tf in respawnOnLeaveParent.GetComponentsInChildren<Transform>(true))
            if (tf != respawnOnLeaveParent) respawnOnLeave.Add(tf.gameObject);
        foreach (var tf in dontRespawnParent.GetComponentsInChildren<Transform>(true))
            if (tf != dontRespawnParent) dontRespawn.Add(tf.gameObject);
    }

    void InitializeStateLists()
    {
        if (currentState == null) currentState = new();

        if (currentState.chests == null)
        {
            currentState.chests = new();
            foreach (var obj in chests)
                currentState.chests.Add(true);
        }
        if (currentState.respawnOnFloorChange == null)
        {
            currentState.respawnOnFloorChange = new();
            foreach (var obj in respawnOnFloorChange)
                currentState.respawnOnFloorChange.Add(true);
        }
        if (currentState.respawnOnLeave == null)
        {
            currentState.respawnOnLeave = new();
            foreach (var obj in respawnOnLeave)
                currentState.respawnOnLeave.Add(true);
        }
        if (currentState.dontRespawn == null)
        {
            currentState.dontRespawn = new();
            foreach (var obj in dontRespawn)
                currentState.dontRespawn.Add(true);
        }
    }
}
