/*-----------------------------------------
Creation Date: 3/31/2024 1:47:34 PM
Author: rodri
Description: Project Knead
-----------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Npc1 : MonoBehaviour
{
    [SerializeField] Transform[] points;
    public int targetPoints;
    public float speed;
    bool Out;
   
  
    
    void Start()
    {
       
        targetPoints = 0;
        Out = true;
    }

    void Update()
    {
        if(transform.position == points[targetPoints].position)
       {
        NextTarget();
       }
       transform.position = Vector3.MoveTowards(transform.position, points[targetPoints].position, speed * Time.deltaTime);
       
       
      
    }
    void NextTarget()
    {
        if(Out == true)
        {
            targetPoints++;
        }
        
        if( targetPoints >= points.Length )
        {
            Out = false;
            
        }
        
        if(Out == false)
        {
            targetPoints--;
        }

        if(targetPoints <= 0 )
        {
            Out = true;
        }

        
       
        
    }
    
}
