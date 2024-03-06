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
