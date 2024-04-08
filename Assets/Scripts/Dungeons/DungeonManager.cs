/*-----------------------------------------
Creation Date: 4/7/2024 6:22:40 PM
Author: theco
Description: Initializes and keeps track of each dungeon and its rooms.
-----------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

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
        while (dungeons.Count <= currentDungeon) dungeons.Add(new Dungeon());
        if (dungeons[currentDungeon].rooms == null) dungeons[currentDungeon].rooms = new DungeonRoomScript[FindObjectsOfType<DungeonRoomScript>().Length];
        if (dungeons[currentDungeon].roomStates == null) dungeons[currentDungeon].roomStates = new RoomState[FindObjectsOfType<DungeonRoomScript>().Length];

        for (int i = 0; i < dungeons[currentDungeon].rooms.Length; i++)
        {
            foreach (var room in FindObjectsOfType<DungeonRoomScript>())
            {
                if (room.roomIndex == i) dungeons[currentDungeon].rooms[i] = room;
            }
        }
        currentRoom = CalculateCurrentRoom();

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
        currentRoom = -1;
    }

    void ReEnterDungeon()
    {
        Destroy(dungeons[currentDungeon].rooms[currentRoom].gameObject);
        dungeons[currentDungeon].rooms[currentRoom] = Instantiate(dungeonsInfo[currentDungeon].roomPrefabs[currentRoom].GetComponent<DungeonRoomScript>());
        dungeons[currentDungeon].rooms[currentRoom].currentState = dungeons[currentDungeon].roomStates[currentRoom];
    }
}
