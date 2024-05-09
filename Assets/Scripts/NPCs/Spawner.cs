/*-----------------------------------------
Creation Date: 3/31/2024 4:52:43 PM
Author: rodri
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{   public GameObject objectToSpawn;
    void Start()
    {
        Instantiate(objectToSpawn, transform.position,transform.rotation);
    }

    void Update()
    {
        
    }
    

}
