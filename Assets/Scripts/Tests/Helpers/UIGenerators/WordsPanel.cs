using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordsPanel
{
    public RectTransform ParentPanel { get; set; }
    public List<string> Words { get; set; }
    public GameObject ButtonWordPrefab { get; set; }
    public GameObject HorizontalLayoutPrefab { get; set; }

    public WordsPanel() { }

    public WordsPanel(RectTransform _parentPanel, 
                      List<string> _words, 
                      GameObject _buttonWordPrefab)
    {
        ParentPanel = _parentPanel;
        Words = _words;
        ButtonWordPrefab = _buttonWordPrefab;
    }

    // For words test
    public Dictionary<int, GameObject> GenerateWordsButtons(Action<object> _onButtonClick, Dictionary<int, string> _words)
    {
        if (ParentPanel == null)
            throw new NullReferenceException("Parent panel not set.");
        if (Words == null && _words == null)
            throw new NullReferenceException("Words list not set.");
        if (ButtonWordPrefab == null)
            throw new NullReferenceException("Word prefab not set.");
        if (_onButtonClick == null)
            throw new ArgumentNullException("_onButtonClick is null");
        HorizontalLayoutPrefab = ParentPanel.GetComponentInChildren<HorizontalLayoutGroup>().gameObject;

        var result = new Dictionary<int, GameObject>();
        var horizontalLayout = HorizontalLayoutPrefab;
        var HLwidth = (HorizontalLayoutPrefab.transform as RectTransform).sizeDelta.x;
        var buttonsPerRow = 0;
        var rowWidth = 0f;
        var rowNextWidth = 0f;
        var buttonsSpacing = 8f;

        foreach (var word in _words)
        {
            var buttonGO = UnityEngine.Object.Instantiate(ButtonWordPrefab, horizontalLayout.transform);
            var buttonRT = buttonGO.transform as RectTransform;
            var buttonSizes = SetWordToPrefab(ref buttonGO, word.Value);
            buttonRT.sizeDelta = new Vector2(buttonSizes.x, buttonRT.sizeDelta.y);
            buttonRT.pivot = new Vector2(0f, 1f);
            buttonsPerRow++;
            rowNextWidth = rowWidth + buttonRT.sizeDelta.x;

            if (rowNextWidth > HLwidth)
            {
                horizontalLayout = UnityEngine.Object.Instantiate(HorizontalLayoutPrefab, ParentPanel);
                var children = horizontalLayout.GetComponentsInChildren<Transform>();
                foreach (var child in children)
                {
                    if (child.GetComponent<Button>() != null)
                        UnityEngine.Object.Destroy(child.gameObject);
                }
                buttonGO.transform.SetParent(horizontalLayout.transform);
                buttonsPerRow = 0;
                rowWidth = 0;
            }
            else
            {
                rowWidth += rowNextWidth + ((buttonsPerRow > 0)? buttonsSpacing : 0);
            }

            buttonGO.transform.SetParent(horizontalLayout.transform);
            buttonGO.GetComponent<Button>().onClick.AddListener(() => _onButtonClick(word));
            result.Add(word.Key, buttonGO);
        }

        return result;
    }

    // For faces test
    public List<DataUI> GenerateWordsButtons(Action<string> _onButtonClick)
    {
        if (ParentPanel == null)
            throw new NullReferenceException("Parent panel not set.");
        if (Words == null)
            throw new NullReferenceException("Words list not set.");
        if (ButtonWordPrefab == null)
            throw new NullReferenceException("Word prefab not set.");
        if (_onButtonClick == null)
            throw new ArgumentNullException("_onButtonClick is null");


        List<DataUI> result = new List<DataUI>();
        var panelSize = ParentPanel.rect;
        var xPos = 0f;
        var yPos = 0f;
        var xMargin = 8f;
        var yMargin = 8f;

        foreach (var word in Words)
        {
            var go = UnityEngine.Object.Instantiate(ButtonWordPrefab, ParentPanel);
            var rt = go.transform as RectTransform;
            var buttonSizes = SetWordToPrefab(ref go, word);
            rt.sizeDelta = new Vector2(buttonSizes.x, rt.sizeDelta.y);
            rt.pivot = new Vector2(0f, 1f);
            var nextX = xPos + rt.sizeDelta.x;

            // If next button out from horizontal panel
            if (nextX > panelSize.width)
            {
                // Set new button on new row
                yPos -= (rt.sizeDelta.y + yMargin);
                xPos = 0;
                rt.localPosition = new Vector3(xPos, yPos, 0f);
                xPos = rt.sizeDelta.x + xMargin;
            }
            else
            {
                // Set new button on the same row
                rt.localPosition = new Vector3(xPos, yPos, 0f);
                xPos += nextX + xMargin;
            }

            go.GetComponent<Button>().onClick.AddListener(() => _onButtonClick(word));
            var dataUI = new DataUI()
            {
                _image = go.GetComponent<Image>(),
                _text = go.GetComponentInChildren<TextMeshProUGUI>()
            };
            result.Add(dataUI);
        }

        return result;
    }

    private Vector2 SetWordToPrefab(ref GameObject _buttonPrefab, string _word)
    {
        var buttonTMP = _buttonPrefab.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonTMP == null) throw new Exception("TMP not in prefab!");
        buttonTMP.text = _word;
        // Get size of text
        var sizes = buttonTMP.GetTextSize(_word);
        var hPadding = 16f;
        var vPadding = 8f;

        return new Vector2(sizes.x + hPadding * 2, sizes.y + vPadding * 2);
    }

    private Vector2 GetTextSizes(TMP_TextInfo _TMP_TextInfo)
    {
        var result = new Vector2();
        var size = _TMP_TextInfo.characterCount;
        for (int i = 0; i < size; i++)
        {
            var character = _TMP_TextInfo.characterInfo[i];
            result.x += Math.Abs(character.bottomRight.x - character.bottomLeft.x);
            result.y += Math.Abs(character.topLeft.y - character.bottomLeft.y);
        }

        return result;
    }

    public void DestroyButtons()
    {
        var childs = ParentPanel.GetComponentsInChildren<Transform>();
        foreach (var child in childs)
        {
            if (!child.Equals(ParentPanel) && !child.Equals(HorizontalLayoutPrefab.transform as RectTransform))
                UnityEngine.Object.Destroy(child.gameObject);
        }
        if (Words != null && Words.Count > 0)
            Words.Clear();
    }
}

public static class TMPExtension
{ 
    public static Vector2 GetTextSize(this TextMeshProUGUI _tmp, string _text)
    {
        Vector2 result = new Vector2(0f, 0f);

        var textInfo = _tmp.GetTextInfo(_text);
        var size = textInfo.characterCount;
        for (int i = 0; i < size; i++)
        {
            var character = textInfo.characterInfo[i];
            result.x += Math.Abs(character.bottomRight.x - character.bottomLeft.x);
            result.y += Math.Abs(character.topLeft.x - character.bottomLeft.x);
        }

        return result;
    }
}
