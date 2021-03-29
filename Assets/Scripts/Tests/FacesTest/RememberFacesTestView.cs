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

    void OnEnable()
    {
        GameObject newGO = Instantiate(ImageButtonPrefab);
        Rect rect = (newGO.transform as RectTransform).rect;
        int imagesCount = loadedImages?.Count ?? throw new Exception("No loaded images!");
        int currentOffset = 32;


        if (imagesCount == 0)
        {
            ContentView.sizeDelta = new Vector2(Screen.width, ContentView.sizeDelta.y);
        }
        if (imagesCount > 0)
        {
            float totalWidth = imagesCount * (rect.width + 16) + 24 * 2;
            ContentView.sizeDelta = new Vector2(totalWidth, ContentView.sizeDelta.y);
            for (int i = 0; i < imagesCount; i++)
            {
                GameObject _newImageButton = Instantiate(ImageButtonPrefab, ContentView);
                ProcessImageButton(_newImageButton, i, ref currentOffset);
            }
        }
    }

    private void ProcessImageButton(GameObject _go, int _index, ref int _offset)
    {
        var rt = _go.transform as RectTransform;
        var img = loadedImages[_index];
        var prefabImage = _go.GetComponent<Image>() ?? throw new Exception($"No image #{_index}");
        var prefabButton = _go.GetComponent<Button>();

        rt.localPosition = new Vector3(_offset, 0, 0f);
        prefabImage.sprite = Sprite.Create(
            img._image, 
            new Rect(0, 0, img._image.width, img._image.height), 
            new Vector2(0.5f, 0.5f));
        _offset = _offset + (int)rt.rect.width + 16;
        prefabButton.onClick.AddListener(() => OnImageClick(_index));
    }

    public void OnImageClick(int _index)
    {
        Debug.Log($"Image number: {_index}");
    }
}
