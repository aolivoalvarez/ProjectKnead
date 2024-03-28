/*-----------------------------------------
Creation Date: 3/28/2024 3:14:05 PM
Author: theco
Description: Attached to Bomb's child GameObject, Graphic. Contains functions to call from animation events.
-----------------------------------------*/

using UnityEngine;

public class BombAnimationEvents : MonoBehaviour
{
    void BombExplode()
    {
        Destroy(transform.root.gameObject);
    }
}
