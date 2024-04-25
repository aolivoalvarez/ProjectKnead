/*-----------------------------------------
Creation Date: 4/8/2024 5:20:03 PM
Author: theco
Description: Stores the stats of the player's currently equipped sword.
-----------------------------------------*/

using System.Collections.Generic;
using UnityEngine;

public class SwordHitboxScript : MonoBehaviour
{
    public int attackDamage = 1;
    public float knockbackStrength = 1.0f;
    List<GameObject> objectsHitThisSwing;

    void OnEnable()
    {
        if (objectsHitThisSwing == null) objectsHitThisSwing = new List<GameObject>();
        objectsHitThisSwing.Clear();
        objectsHitThisSwing.TrimExcess();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (objectsHitThisSwing.Count == 0) objectsHitThisSwing.Add(other.gameObject);
        else
        {
            foreach (GameObject obj in objectsHitThisSwing)
            {
                //A single sword swing shouldn't be able to hit the same enemy more than once
                if (obj == other.gameObject)
                    return;
            }   
        }

        if (other.gameObject.GetComponent<HenchmanScript>() != null)
        {
            Vector2 direction = other.transform.position - PlayerController.instance.transform.position;
            other.gameObject.GetComponent<HenchmanScript>().TakeDamage(attackDamage, knockbackStrength, direction);
        }
        if (other.gameObject.GetComponent<BasicEnemyScript>() != null)
        {
            Vector2 direction = other.transform.position - PlayerController.instance.transform.position;
            other.gameObject.GetComponent<BasicEnemyScript>().TakeDamage(attackDamage, knockbackStrength, direction);
        }
        objectsHitThisSwing.Add(other.gameObject);
    }
}
