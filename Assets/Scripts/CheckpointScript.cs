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
