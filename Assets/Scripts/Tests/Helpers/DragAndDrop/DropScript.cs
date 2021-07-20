using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Скрипт для площадок перемещаемых объектов
/// </summary>
public class DropScript : MonoBehaviour, IDropHandler
{
    public Action OnUnseccessDrop;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null &&
            eventData.pointerDrag.GetComponent<ScrollRect>() == null)
        {
            var buttonIMG = gameObject.ChildByName("ButtonIMG");
            if (buttonIMG != null) Destroy(buttonIMG);
            var rt = eventData.pointerDrag.transform as RectTransform;

            rt.SetParent(transform);
            rt.anchoredPosition = Vector3.zero;
        }
    }
}
