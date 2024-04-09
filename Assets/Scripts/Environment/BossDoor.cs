/*-----------------------------------------
Creation Date: 4/4/2024 6:24:25 PM
Author: rodri
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    

    public void OnTriggerEnter2D(Collider2D other){
        if((other.tag == "Door" && playerController.bossKeyCollected == true)){
            
            Destroy(other.gameObject);
            Debug.Log("Door Destoryed");

        }
    }

    
}
