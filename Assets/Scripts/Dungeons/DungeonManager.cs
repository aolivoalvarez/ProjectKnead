/*-----------------------------------------
Creation Date: 4/7/2024 6:22:40 PM
Author: theco
Description: Project Knead
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
        if (dungeons[currentDungeon].roomStates == null) dungeons[currentDungeon].roomStates = new DungeonRoomScript.RoomState[FindObjectsOfType<DungeonRoomScript>().Length];

        for (int i = 0; i < dungeons[currentDungeon].rooms.Length; i++)
        {
            foreach (var room in FindObjectsOfType<DungeonRoomScript>())
            {
                if (room.roomIndex == i) dungeons[currentDungeon].rooms[i] = room;
            }
        }
        currentRoom = CalculateCurrentRoom();
    }

    public void ChangeRoom()
    {
        if (currentRoom == CalculateCurrentRoom()) return;

        dungeons[currentDungeon].roomStates[currentRoom] = dungeons[currentDungeon].rooms[currentRoom].SaveToStateLists();
        dungeons[currentDungeon].rooms[currentRoom].gameObject.SetActive(false);

        currentRoom = CalculateCurrentRoom();

        dungeons[currentDungeon].rooms[currentRoom].gameObject.SetActive(true);
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
}
