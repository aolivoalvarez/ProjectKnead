/*-----------------------------------------
Creation Date: 4/7/2024 6:47:15 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomScript : MonoBehaviour
{
    public struct RoomState
    {
        public List<bool> chests;
        public List<bool> enemies;
        public List<bool> respawnOnLeave;
        public List<bool> dontRespawn;
    }

    public int roomIndex = 0;

    [SerializeField] GameObject chestsParent;
    [SerializeField] GameObject enemiesParent;
    [SerializeField] GameObject respawnOnLeaveParent;
    [SerializeField] GameObject dontRespawnParent;

    public List<GameObject> chests { get; private set; }
    public List<GameObject> enemies { get; private set; }
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
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) currentState.enemies[i] = false;
            else currentState.enemies[i] = enemies[i].activeSelf;
        }
        for (int i = 0; i < respawnOnLeave.Count; i++)
        {
            if (respawnOnLeave[i] == null) currentState.respawnOnLeave[i] = false;
            else currentState.respawnOnLeave[i] = respawnOnLeave[i].activeSelf;
        }
        for (int i = 0; i < currentState.dontRespawn.Count; i++)
        {
            if (dontRespawn[i] == null) currentState.dontRespawn[i] = false;
            else currentState.dontRespawn[i] = dontRespawn[i].activeSelf;
        }

        return currentState;
    }

    void LoadFromStateLists()
    {
        for (int i = 0; i < chests.Count; i++)
        {
            if (!currentState.chests[i]) chests[i].GetComponent<ChooseChestPickup>().OpenChest();
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!currentState.enemies[i]) enemies[i].SetActive(false);
        }
        for (int i = 0; i < respawnOnLeave.Count; i++)
        {
            if (!currentState.respawnOnLeave[i]) respawnOnLeave[i].SetActive(false);
        }
        for (int i = 0; i < currentState.dontRespawn.Count; i++)
        {
            if (!currentState.dontRespawn[i]) dontRespawn[i].SetActive(false);
        }
    }

    void InitializeObjectLists()
    {
        chests = new();
        enemies = new();
        respawnOnLeave = new();
        dontRespawn = new();

        foreach (var c in chestsParent.GetComponentsInChildren<ChooseChestPickup>())
            chests.Add(c.gameObject);
        foreach (var e in enemiesParent.GetComponentsInChildren<HenchmanScript>())
            enemies.Add(e.gameObject);
        foreach (var r in respawnOnLeaveParent.GetComponentsInChildren<Transform>())
            if (r != respawnOnLeaveParent) respawnOnLeave.Add(r.gameObject);
        foreach (var d in dontRespawnParent.GetComponentsInChildren<Transform>())
            if (d != dontRespawnParent) dontRespawn.Add(d.gameObject);
    }

    void InitializeStateLists()
    {
        if (currentState.Equals(null)) currentState = new();

        if (currentState.chests == null)
        {
            currentState.chests = new();
            foreach (var c in chests)
                currentState.chests.Add(true);
        }
        if (currentState.enemies == null)
        {
            currentState.enemies = new();
            foreach (var e in enemies)
                currentState.enemies.Add(true);
        }
        if (currentState.respawnOnLeave == null)
        {
            currentState.respawnOnLeave = new();
            foreach (var r in respawnOnLeave)
                currentState.respawnOnLeave.Add(true);
        }
        if (currentState.dontRespawn == null)
        {
            currentState.dontRespawn = new();
            foreach (var d in dontRespawn)
                currentState.dontRespawn.Add(true);
        }
    }
}
