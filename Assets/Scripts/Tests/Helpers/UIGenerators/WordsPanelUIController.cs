using System;
using System.Collections.Generic;
using UnityEngine;

public class WordsPanelUIController : MonoBehaviour
{
    public WordsPanel wordsPanel;
    public RectTransform ParentPanel;
    public GameObject ButtonWordPrefab;
    public GameObject HorizontalLayoutPrefab;

    public Dictionary<int, GameObject> Buttons { get; private set; }

    private void Awake()
    {
        wordsPanel = new WordsPanel();
        wordsPanel.ParentPanel = ParentPanel;
        wordsPanel.ButtonWordPrefab = ButtonWordPrefab;
        wordsPanel.HorizontalLayoutPrefab = HorizontalLayoutPrefab;
    }

    public void CreatePanel(Action<object> _onClick, Dictionary<int, string> _words)
    {
        Buttons = wordsPanel.GenerateWordsButtons(_onClick, _words);
    }

    public void ClearPanel()
    {
        wordsPanel.DestroyButtons();
    }
}
