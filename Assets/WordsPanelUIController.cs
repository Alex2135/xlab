using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordsPanelUIController : MonoBehaviour
{
    public WordsPanel wordsPanel;
    public RectTransform ParentPanel;
    public GameObject ButtonWordPrefab;
    public GameObject HorizontalLayoutPrefab;

    public List<DataUI> Buttons { get; private set; }

    private void CreatePanel(List<string> _words, Action<string> _action)
    {
        wordsPanel = new WordsPanel();
        wordsPanel.ParentPanel = ParentPanel;
        wordsPanel.ButtonWordPrefab = ButtonWordPrefab;
        wordsPanel.HorizontalLayoutPrefab = HorizontalLayoutPrefab;
        wordsPanel.Words = _words;
        Buttons = wordsPanel.GenerateWordsButtons(_action);
    }
}
