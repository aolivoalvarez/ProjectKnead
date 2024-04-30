/*-----------------------------------------
Creation Date: 4/25/2024 1:27:11 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnableTrigger : MonoBehaviour
{
    [SerializeField] GameObject objectToEnable;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        objectToEnable.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == PlayerController.instance.gameObject)
        {
            objectToEnable.SetActive(true);
            Destroy(gameObject);
        }
    }
}
