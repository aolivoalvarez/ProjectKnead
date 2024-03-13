/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: To be attached to a Checkpoint GameObject, a simple Transform used to keep track of where the player should respawn (not after Game Over).
-----------------------------------------*/

using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public static CheckpointScript instance;

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
