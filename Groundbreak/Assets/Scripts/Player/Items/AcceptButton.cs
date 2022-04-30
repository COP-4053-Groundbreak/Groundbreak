using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class AcceptButton : MonoBehaviour, IPointerDownHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (gameObject.name == "Accept") 
        {
            FindObjectOfType<PassiveItemManager>().Accept();
        }
        {
            FindObjectOfType<PassiveItemManager>().Reject();
        }

    }
}
