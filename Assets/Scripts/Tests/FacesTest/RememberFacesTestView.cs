using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberFacesTestView : MonoBehaviour, IScreenController
{
    [Serializable]
    public class FacesImage : LoadedImage
    {
        public FacesImage(Texture2D _t = null, string _n = null) : base(_t, _n) { }
    }

    public Button RememberButton;
    public GameObject ImageButtonPrefab;
    public RectTransform ContentView;
    public List<FacesImage> loadedImages;
    public string ScreenName { get; set; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void Start()
    {
        GameObject newGO = Instantiate(ImageButtonPrefab, ContentView);
        Rect rect = (newGO.transform as RectTransform).rect;
        int imagesCount = loadedImages.Count;
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

    public Image GetBackground()
    {
        return null;
    }

    public object GetResult()
    {
        return null;
    }   
}
