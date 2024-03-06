using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Transform checkpoint;

    void Awake()
    {
        //---------- Make this script a singleton ----------//
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        //--------------------------------------------------//
    }

    void Update()
    {
        
    }

    public void RespawnAtCheckpoint(int damageToInflict = 0)
    {
        PlayerController.instance.DecreaseHealth(damageToInflict);
        PlayerController.instance.transform.position = checkpoint.position;
    }
}
