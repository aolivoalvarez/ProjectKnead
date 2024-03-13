/*-----------------------------------------
Creation Date: N/A
Author: theco
Description: Creates a persistent singleton of whatever it is attached to.
             To be attached to the PERSISTING OBJECTS GameObject, which is a parent to all objects that should persist across all scenes.
-----------------------------------------*/

using UnityEngine;

public class PersistingObjectsScript : MonoBehaviour
{
    public static PersistingObjectsScript instance;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        //--------------------------------------------------//
    }
}
