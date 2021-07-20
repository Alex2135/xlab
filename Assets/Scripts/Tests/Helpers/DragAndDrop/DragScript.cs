using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  Скрипт для перемещаемых объектов 
///  /
///  Script for dragable objects
/// </summary>
public class DragScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    public Action<object> OnEndDragEvent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private GameObject goCopy;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        goCopy = Instantiate(gameObject, transform.parent);
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (eventData.pointerDrag.GetComponentInParent<DropScript>() == null)
        {
            Debug.Log("OnEndDrag");
            Destroy(goCopy);
            (transform as RectTransform).anchoredPosition = Vector3.zero;
        }
    }
}
