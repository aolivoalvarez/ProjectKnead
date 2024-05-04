/*-----------------------------------------
Creation Date: 3/21/2024 3:16:58 PM
Author: theco
Description: For pickups that are collected from chests.
-----------------------------------------*/

using UnityEditor;
using UnityEngine;

public class ChooseChestPickup : ChoosePickup
{
    [SerializeField] Sprite chestClosed;
    [SerializeField] Sprite chestOpen;
    [SerializeField] SpriteRenderer graphic;

    public bool chestOpened = false;

    public GameObject OpenChest()
    {
        if (!chestOpened)
        {
            chestOpened = true;
            graphic.sprite = chestOpen;
            return pickupPrefabs[thisPickup];
        }
        return null;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        var labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.yellow;
        labelStyle.alignment = TextAnchor.MiddleCenter;

        Handles.Label(transform.position, thisPickup.ToString(), labelStyle);
    }
#endif
}
