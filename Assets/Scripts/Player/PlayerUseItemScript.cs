/*-----------------------------------------
Creation Date: 3/28/2024 2:28:36 PM
Author: theco
Description: Project Knead
-----------------------------------------*/

using UnityEngine;

public class PlayerUseItemScript : MonoBehaviour
{
    [SerializeField] GameObject bombPrefab;
    PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.instance;
    }

    void Update()
    {
        if (playerController.pInput.Player.Item.triggered)
        {
            Instantiate(bombPrefab, new Vector3(transform.position.x + playerController.simpleLookDirection.x, transform.position.y +
                playerController.simpleLookDirection.y, transform.position.z), Quaternion.identity);
        }
    }
}
