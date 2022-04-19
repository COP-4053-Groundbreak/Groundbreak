using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemHoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;
    GameObject itemGameObject;

    public UIConsumableInventory uIConsumableInventory;
    public UIPassiveInventory uIPassiveInventory;
    public ToolTipController toolTipController;
    [SerializeField] UIConsumableInventoryController uIInventoryControler;

    private void Awake()
    {
        if (uIInventoryControler == null)
        {
            uIInventoryControler = GameObject.FindObjectOfType<UIConsumableInventoryController>();
        }

        rectTransform = GetComponent<RectTransform>();
        canvas = FindObjectOfType<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        toolTipController = FindObjectOfType<ToolTipController>();
    }

    // When a pointer hovers over the item
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        itemGameObject = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
        if (itemGameObject.GetComponent<PassiveItemClickable>() != null)
        {
            toolTipController.ShowToolTip(transform.position, itemGameObject.GetComponent<PassiveItemClickable>().passiveItem);
        }
        else if (itemGameObject.GetComponent<ConsumableItemClickable>() != null)
        {
            toolTipController.ShowToolTip(transform.position, itemGameObject.GetComponent<ConsumableItemClickable>().consumableItem);
        }
        else if (itemGameObject.GetComponent<ActiveItemClickable>() != null) 
        {
            toolTipController.ShowToolTip(transform.position, itemGameObject.GetComponent<ActiveItemClickable>().activeItem);
        }
    }

    // When pointer leaves the item
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        toolTipController.HideToolTip();
    }

}
