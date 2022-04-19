using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ActiveItemClickable : MonoBehaviour
{
    public Action onLeftClick;
    public Action onRightClick;
    public ActiveItem activeItem;

    [SerializeField] HoldPlayerStats playerStats;

    private void Update()
    {
        activeItem = playerStats.playerActiveItem;
    }


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
