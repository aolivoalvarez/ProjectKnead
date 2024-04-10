/*-----------------------------------------
Creation Date: 4/7/2024 6:22:40 PM
Author: theco
Description: Initializes and keeps track of each dungeon and its rooms.
-----------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public enum SwitchColor
    {
        Red,
        Blue
    }

    public DungeonRoomScript[] rooms;
    public RoomState[] roomStates;
    public SwitchColor currentColor;
}

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public DungeonInfo[] dungeonsInfo;
    public List<Dungeon> dungeons;
    public int currentDungeon = -1;
    public int currentRoom = -1;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    public void InitializeDungeon()
    {
        if (dungeons == null) dungeons = new();
        while (dungeons.Count <= currentDungeon)
        {
            dungeons.Add(new Dungeon());
            dungeonsInfo[currentDungeon].ResetValues();
        }
        if (dungeons[currentDungeon].rooms == null) dungeons[currentDungeon].rooms = new DungeonRoomScript[FindObjectsOfType<DungeonRoomScript>().Length];
        if (dungeons[currentDungeon].roomStates == null) dungeons[currentDungeon].roomStates = new RoomState[FindObjectsOfType<DungeonRoomScript>().Length];

        for (int i = 0; i < dungeons[currentDungeon].rooms.Length; i++)
        {
            foreach (var room in FindObjectsOfType<DungeonRoomScript>())
            {
                if (room.roomIndex == i) 
                {
                    dungeons[currentDungeon].rooms[i] = room;
                    dungeons[currentDungeon].rooms[i].gameObject.SetActive(false);
                }
            }
        }
        currentRoom = CalculateCurrentRoom();
        dungeons[currentDungeon].rooms[currentRoom].gameObject.SetActive(true);

        if (dungeons[currentDungeon].roomStates[currentRoom] != null) ReEnterDungeon();
    }

    public void ChangeRoom()
    {
        if (currentRoom == CalculateCurrentRoom()) return;

        dungeons[currentDungeon].roomStates[currentRoom] = dungeons[currentDungeon].rooms[currentRoom].SaveToStateLists();
        dungeons[currentDungeon].rooms[currentRoom].gameObject.SetActive(false);

        currentRoom = CalculateCurrentRoom();

        Destroy(dungeons[currentDungeon].rooms[currentRoom].gameObject);
        dungeons[currentDungeon].rooms[currentRoom] = Instantiate(dungeonsInfo[currentDungeon].roomPrefabs[currentRoom].GetComponent<DungeonRoomScript>());
        dungeons[currentDungeon].rooms[currentRoom].currentState = dungeons[currentDungeon].roomStates[currentRoom];
    }

    int CalculateCurrentRoom()
    {
        float shortestDistance = -1.0f;
        int closestRoom = 0;

        foreach (var room in dungeons[currentDungeon].rooms)
        {
            float distance = Vector2.Distance(room.transform.position, PlayerController.instance.transform.position);
            if (shortestDistance == -1.0f || shortestDistance > distance)
            {
                shortestDistance = distance;
                closestRoom = room.roomIndex;
            }
        }

        return closestRoom;
    }

    public void LeaveDungeon()
    {
        dungeons[currentDungeon].roomStates[currentRoom] = dungeons[currentDungeon].rooms[currentRoom].SaveToStateLists();
        for (int i = 0; i < dungeons[currentDungeon].roomStates.Length; i++)
        {
            if (dungeons[currentDungeon].roomStates[i] != null)
            {
                for (int n = 0; n < dungeons[currentDungeon].roomStates[i].respawnOnLeave.Count; n++)
                {
                    dungeons[currentDungeon].roomStates[i].respawnOnLeave[n] = true;
                }
            }
        }
        ChangeFloor();
        dungeons[currentDungeon].currentColor = Dungeon.SwitchColor.Red;
        currentRoom = -1;
    }

    public void ChangeFloor()
    {
        dungeons[currentDungeon].roomStates[currentRoom] = dungeons[currentDungeon].rooms[currentRoom].SaveToStateLists();
        for (int i = 0; i < dungeons[currentDungeon].roomStates.Length; i++)
        {
            if (dungeons[currentDungeon].roomStates[i] != null)
            {
                for (int n = 0; n < dungeons[currentDungeon].roomStates[i].respawnOnFloorChange.Count; n++)
                {
                    dungeons[currentDungeon].roomStates[i].respawnOnFloorChange[n] = true;
                }
            }
        }
    }

    void ReEnterDungeon()
    {
        Destroy(dungeons[currentDungeon].rooms[currentRoom].gameObject);
        dungeons[currentDungeon].rooms[currentRoom] = Instantiate(dungeonsInfo[currentDungeon].roomPrefabs[currentRoom].GetComponent<DungeonRoomScript>());
        dungeons[currentDungeon].rooms[currentRoom].currentState = dungeons[currentDungeon].roomStates[currentRoom];
    }
}
