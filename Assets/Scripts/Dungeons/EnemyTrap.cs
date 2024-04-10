/*-----------------------------------------
Creation Date: 4/10/2024 5:30:03 PM
Author: theco
Description: Enables and disables certain objects until the enemies are killed.
-----------------------------------------*/

using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesToKill;
    [SerializeField] GameObject[] enabledWhileActive;
    [SerializeField] GameObject[] disabledWhileActive;
    int enemiesLeft;

    void Start()
    {
        foreach (var obj in enabledWhileActive)
            obj.SetActive(true);
        foreach (var obj in disabledWhileActive)
            obj.SetActive(false);
    }

    void Update()
    {
        enemiesLeft = enemiesToKill.Length;
        foreach (var obj in enemiesToKill)
        {
            if (obj != null && obj.activeSelf)
                break;
            enemiesLeft--;
        }

        if (enemiesLeft <= 0)
        {
            foreach (var obj in enabledWhileActive)
                obj.SetActive(false);
            foreach (var obj in disabledWhileActive)
                obj.SetActive(true);
            Destroy(gameObject);
        }
    }
}
