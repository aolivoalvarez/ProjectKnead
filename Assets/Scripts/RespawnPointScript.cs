/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a RespawnPoint GameObject, a simple Transform used to keep track of where the player should respawn after a Game Over.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnPointScript : MonoBehaviour
{
    public static RespawnPointScript instance;
    public int respawnSceneIndex { get; set; }

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }
}
