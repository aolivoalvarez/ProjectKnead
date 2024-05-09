/*-----------------------------------------
Creation Date: 4/24/2024 7:47:37 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(CircleCollider2D))]
public class EnemyShieldScript : MonoBehaviour
{
    [SerializeField] string[] tagsThatDamageShield;
    HenchmanScript thisEnemy;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        thisEnemy = GetComponentInParent<HenchmanScript>();
        if (thisEnemy == null)
            Destroy(gameObject);
        thisEnemy.isShielded = true;
    }

    void DamageShield()
    {
        thisEnemy.isShielded = false;
        AudioManager.instance.PlaySound(36);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string currentTag in tagsThatDamageShield)
        {
            if (other.gameObject.CompareTag(currentTag))
            {
                DamageShield();
            }
        }
    }
}
