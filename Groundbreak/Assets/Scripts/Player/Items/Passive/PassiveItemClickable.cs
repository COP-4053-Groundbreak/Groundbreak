using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PassiveItemClickable : MonoBehaviour
{
    public Action onLeftClick;
    public Action onRightClick;
    public PassiveItem passiveItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == 0)
        {
            onLeftClick();
        }
        else
        {
            onRightClick();
        }
    }
}
