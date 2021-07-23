using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumbersPanelCreator : MonoBehaviour
{
    public GameObject VerticalPanel;
    public GameObject HorizontalPanelPrefab;
    public GameObject InnerObjectPrefab;
    public List<GameObject> CreatedGameObjects { get; set; }

    public List<GameObject> CreatePanel(List<int> _data)
    {
        var result = new List<GameObject>();

        var tmp = InnerObjectPrefab.GetComponent<TextMeshProUGUI>();
        GameObject horizontalPanel = Instantiate(HorizontalPanelPrefab, VerticalPanel.transform);
        float maxWidth = 0f;
        float itemsSpacing = 32f;
        if (tmp != null)
        {
            GameObject visibleTempObject;
            visibleTempObject = Instantiate(InnerObjectPrefab, horizontalPanel.transform);
            tmp = visibleTempObject.GetComponent<TextMeshProUGUI>();
            maxWidth = 0f;

            Vector2 sizes;
            foreach (var number in _data)
            {
                sizes = tmp.GetTextSize(number.ToString());
                if (sizes.x > maxWidth)
                    maxWidth = sizes.x;
            }
            Destroy(visibleTempObject);
        }
        else
        {
            GameObject visibleTempObject;
            visibleTempObject = Instantiate(InnerObjectPrefab, horizontalPanel.transform);
            tmp = visibleTempObject.GetComponentInChildren<TextMeshProUGUI>();
            maxWidth = 0f;

            Vector2 sizes;
            int maxSymbols = 0;
            foreach (var number in _data)
            {
                sizes = tmp.GetTextSize(number.ToString());
                if (sizes.x > maxWidth)
                {
                    maxWidth = sizes.x;
                    maxSymbols = number.ToString().Length;
                }
            }
            if (maxSymbols > 2)
                maxWidth = maxWidth * (1f - 2f / maxSymbols);
            else
                maxWidth = 0;
            maxWidth += visibleTempObject.GetComponent<RectTransform>().rect.width;
            itemsSpacing = 8f;
            Destroy(visibleTempObject);
        }

        if (horizontalPanel == null) throw new Exception("horizontalPanel is null");

        float hpanelWidth = horizontalPanel.GetComponent<RectTransform>().rect.width;
        int hipoItems = Mathf.FloorToInt(hpanelWidth / (itemsSpacing + maxWidth));

        if (hipoItems > 6) hipoItems = 6;

        if ((hipoItems * (itemsSpacing + maxWidth) - itemsSpacing) > hpanelWidth)
            hipoItems--;

        int rows = (int) Mathf.Ceil((float)_data.Count / hipoItems);
        int wordIndex = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < hipoItems; j++)
            {
                var newObject = Instantiate(InnerObjectPrefab, horizontalPanel.transform);
                var rt = newObject.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(maxWidth, rt.sizeDelta.y);
                var newTMP = newObject.GetComponent<TextMeshProUGUI>();
                if (newTMP == null)
                    newTMP = newObject.GetComponentInChildren<TextMeshProUGUI>();
                newTMP.text = _data[wordIndex].ToString();
                //newTMP.enableAutoSizing = true;
                wordIndex++;
                result.Add(newObject);
                if (wordIndex >= _data.Count) break;
            }
            if (wordIndex < _data.Count)
                horizontalPanel = Instantiate(HorizontalPanelPrefab, VerticalPanel.transform);
        }
        CreatedGameObjects = result;

        return result;
    }
}
