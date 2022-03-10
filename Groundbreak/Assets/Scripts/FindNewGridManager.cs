using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FindNewGridManager : MonoBehaviour
{
    public event EventHandler OnGridChanged;
    public void ChangedRoom() 
    {
        if (OnGridChanged != null)
        {
            OnGridChanged(this, EventArgs.Empty);
        }
    }
}
