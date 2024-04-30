/*-----------------------------------------
Creation Date: 3/25/2024 11:49:51 PM
Author: theco
Description: Manager script that holds all animation curves used in the game.
-----------------------------------------*/

using UnityEngine;

public class AnimationCurvesScript : MonoBehaviour
{
    public static AnimationCurvesScript instance;

    public AnimationCurve fallingItem;
    public AnimationCurve droppedPickup;

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
