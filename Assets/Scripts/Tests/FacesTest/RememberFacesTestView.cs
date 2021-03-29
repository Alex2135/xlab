using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberFacesTestView : MonoBehaviour, IScreenController
{
    public Button RememberButton;
    public GameObject ImageButtonPrefab;
    public RectTransform ContentView;
    public List<LoadedImage> loadedImages;
    public string _screenName;
    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void Start()
    {
        GameObject newGO = Instantiate(ImageButtonPrefab, ContentView);
        Rect rect = (newGO.transform as RectTransform).rect;
        int imagesCount = loadedImages?.Count ?? throw new Exception("No loaded images!");
        int currentOffset = 32;


        if (imagesCount == 0)
        {
            ContentView.sizeDelta = new Vector2(Screen.width, ContentView.sizeDelta.y);
        }
        if (imagesCount > 0)
        {
            float totalWidth = (imagesCount + 16) * rect.width + 24 * 2;
            ContentView.sizeDelta = new Vector2(rect.height, totalWidth);
            for (int i = 0; i < imagesCount; i++)
            {
                GameObject _newImageButton = Instantiate(ImageButtonPrefab, ContentView);
                ProcessImageButton(_newImageButton, i, ref currentOffset);
            }
        }
    }

    private void ProcessImageButton(GameObject _go, int _index, ref int _offset)
    {
        RectTransform rt = _go.transform as RectTransform;
        _offset = _offset + (int)rt.rect.width;
    }
}
