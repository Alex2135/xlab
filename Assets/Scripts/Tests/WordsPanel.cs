using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordsPanel
{
    public RectTransform ParentPanel { get; set; }
    public List<string> Words { get; set; }
    public GameObject ButtonWordPrefab { get; set; }

    public List<GameObject> GenerateWordsButtons(Action _onButtonClick = null)
    {
        if (ParentPanel == null)
            throw new NullReferenceException("Parent panel not set.");
        if (Words == null)
            throw new NullReferenceException("Words list not set.");
        if (ButtonWordPrefab == null)
            throw new NullReferenceException("Word prefab not set.");

        List<GameObject> result = new List<GameObject>();
        var panelSize = ParentPanel.sizeDelta;

        foreach(var word in Words)
        {
            var go = UnityEngine.Object.Instantiate(ButtonWordPrefab, ParentPanel);
            var sizes = SetWordToPrefab(go, word);
        }

        return result;
    }

    private void SetPrefabToPanel()
    {

    }

    private Vector2 SetWordToPrefab(GameObject _buttonPrefab, string _word)
    {
        var buttonTMP = _buttonPrefab.GetComponentInChildren<TextMeshPro>();
        buttonTMP.text = _word;
        var sizes = buttonTMP.GetRenderedValues(true);

        return sizes;
    }
}