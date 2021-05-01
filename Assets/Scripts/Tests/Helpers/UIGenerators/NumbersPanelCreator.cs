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

    public List<GameObject> CreatePanel(List<int> _data)
    {
        var result = new List<GameObject>();

        var tmp = InnerObjectPrefab.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            GameObject horizontalPanel = Instantiate(HorizontalPanelPrefab, VerticalPanel.transform);
            GameObject bufObject = Instantiate(InnerObjectPrefab, horizontalPanel.transform);
            tmp = bufObject.GetComponent<TextMeshProUGUI>();

            float maxWidth = 0f;
            Vector2 bufVector;
            foreach (var number in _data)
            {
                bufVector = tmp.GetTextSize(number.ToString());
                if (bufVector.x > maxWidth)
                {
                    maxWidth = bufVector.x;
                }
            }

            bool isNewPanel = false;
            float hpanelWidth = horizontalPanel.GetComponent<RectTransform>().rect.width;
            float itemsSpacing = 32f;
            int hipoItems = Mathf.FloorToInt(hpanelWidth / (itemsSpacing + maxWidth));
            if ( (hipoItems * (itemsSpacing + maxWidth) - itemsSpacing) > hpanelWidth )
                hipoItems--;

            int rows = Mathf.FloorToInt(_data.Count / hipoItems);
            int wordIndex = 0;
            Destroy(bufObject);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < hipoItems; j++)
                {
                    var newObject = Instantiate(InnerObjectPrefab, horizontalPanel.transform);
                    var rt = newObject.GetComponent<RectTransform>();
                    rt.sizeDelta = new Vector2(maxWidth, rt.sizeDelta.y);
                    var newTMP = newObject.GetComponent<TextMeshProUGUI>();
                    newTMP.text = _data[wordIndex].ToString();
                    newTMP.enableAutoSizing = true;
                    wordIndex++;
                    if (wordIndex >= _data.Count) break;
                }
                if (wordIndex < _data.Count) 
                    horizontalPanel = Instantiate(HorizontalPanelPrefab, VerticalPanel.transform);
            }
        }
        // TODO: Add numbers test inputs

        return result;
    }
}
