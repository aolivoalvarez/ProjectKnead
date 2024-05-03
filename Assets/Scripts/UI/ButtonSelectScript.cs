/*-----------------------------------------
Creation Date: 5/2/2024 6:07:03 PM
Author: theco
Description: For buttons that need a selection icon.
-----------------------------------------*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class ButtonSelectScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] GameObject selectIcon;

    void Start()
    {
        selectIcon.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        selectIcon.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selectIcon.SetActive(false);
    }
}
