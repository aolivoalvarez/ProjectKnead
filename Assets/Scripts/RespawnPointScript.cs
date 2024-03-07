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
