/*-----------------------------------------
Creation Date: 5/2/2024 6:07:03 PM
Author: theco
Description: For buttons that need a selection icon.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSelectScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject selectIcon;

    public void OnSelect(BaseEventData eventData)
    {
        selectIcon.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectIcon.SetActive(false);
    }
}
