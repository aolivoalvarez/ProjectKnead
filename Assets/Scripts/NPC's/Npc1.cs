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
    // [Variables]
    [SerializeField , Tooltip("Destination points")] Transform[] points;
    public int targetPoints;
    [SerializeField]public float speed;
    bool Outwalk;
    [SerializeField, Tooltip("Pause time after arriving at destination points")]float waitTime =  2.0f;
    float elapsedTime = 0.0f;
   
  
    
    void Start(){
       
        targetPoints = 0;
        Outwalk = true;
    }

    void Update(){
        // Move 
        transform.position = Vector3.MoveTowards(transform.position, points[targetPoints].position, speed * Time.deltaTime);

        // Upon reaching destination move to next 
        if(transform.position == points[targetPoints].position){
        
            // Integer Countdown
            elapsedTime += Time.deltaTime;

            if(elapsedTime >= waitTime){

                    elapsedTime = 0;
                

                if(Outwalk == true){   
                    targetPoints++;
                }

                
                if( targetPoints >= points.Length){
                    Outwalk = false;
                }

            
                if(Outwalk == false){
                    targetPoints--;
                }


                if(targetPoints <= 0 ){
                    Outwalk = true;
                }
            

            }
        
        }
       
    }

      
       
       
      
}
