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

    public List<GameObject> Buttons { get; private set; }

    public void CreatePanel(List<string> _words)
    {
        wordsPanel = new WordsPanel();
        wordsPanel.ParentPanel = ParentPanel;
        wordsPanel.ButtonWordPrefab = ButtonWordPrefab;
        wordsPanel.HorizontalLayoutPrefab = HorizontalLayoutPrefab;
        wordsPanel.Words = _words;
        Buttons = wordsPanel.GenerateWordsButtons(OnWordsButtonClick);
    }

    private void OnWordsButtonClick(object _someWord)
    {
        string word = (string)_someWord;
        Debug.Log( word );
    }
}
