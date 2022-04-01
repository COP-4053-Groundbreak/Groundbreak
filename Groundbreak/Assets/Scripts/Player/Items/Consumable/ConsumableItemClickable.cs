using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ConsumableItemClickable : MonoBehaviour, IPointerClickHandler
{
    public Action onLeftClick;
    public Action onRightClick;
    public ConsumableItem consumableItem;

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
