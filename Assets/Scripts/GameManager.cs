using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform checkpoint;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        
    }

    public void RespawnAtCheckpoint(int damageToInflict = 0)
    {
        player.DecreaseHealth(damageToInflict);
        player.roughPosition = checkpoint.position;
    }
}
