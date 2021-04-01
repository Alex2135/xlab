using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RememberFacesTestUIController : MonoBehaviour, IScreenController
{
    public Button RememberButton;
    public GameObject ImageButtonPrefab;
    public RectTransform ContentView;
    public ScrollRect ScrollRect;
    public TextMeshProUGUI NameAndLastname;
    public TextMeshProUGUI ImageCount;
    public List<LoadedImage> loadedImages;
    public string _screenName;
    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    private List<GameObject> _generatedImages;

    void OnEnable()
    {
        _generatedImages = new List<GameObject>();
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
                _generatedImages.Add(_newImageButton);
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
            new Vector2(0.5f, 0.5f)
        );
        _offset = _offset + (int)rt.rect.width + 16;
        prefabButton.onClick.AddListener(() => OnImageClick(_index));
    }

    public void OnImageClick(int _index)
    {
        ImageCount.text = $"{_index + 1} из {_generatedImages.Count}";
        var rt = _generatedImages[_index].transform as RectTransform;
        NameAndLastname.text = loadedImages[_index]._name;
        ScrollRect.ScrollToCenter(rt, RectTransform.Axis.Horizontal);
        if (_index == _generatedImages.Count - 1) RememberButton.gameObject.SetActive(true);
    }

    public void OnRememberButtonClick()
    {
        var sc = ScreensUIController.GetInstance();
        this.gameObject.SetActive(false);
        sc.Activate(NextScreen, null, false);
    }

    // TODO: Реализовать метод сброса состояния окна
    private void ResetScreenState()
    { 

    }
}

public static class UIExtensions
{
    // Shared array used to receive result of RectTransform.GetWorldCorners
    static Vector3[] corners = new Vector3[4];

    /// <summary>
    /// Transform the bounds of the current rect transform to the space of another transform.
    /// </summary>
    /// <param name="source">The rect to transform</param>
    /// <param name="target">The target space to transform to</param>
    /// <returns>The transformed bounds</returns>
    public static Bounds TransformBoundsTo(this RectTransform source, Transform target)
    {
        // Based on code in ScrollRect's internal GetBounds and InternalGetBounds methods
        var bounds = new Bounds();
        if (source != null)
        {
            source.GetWorldCorners(corners);

            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var matrix = target.worldToLocalMatrix;
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = matrix.MultiplyPoint3x4(corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
        }
        return bounds;
    }

    /// <summary>
    /// Normalize a distance to be used in verticalNormalizedPosition or horizontalNormalizedPosition.
    /// </summary>
    /// <param name="axis">Scroll axis, 0 = horizontal, 1 = vertical</param>
    /// <param name="distance">The distance in the scroll rect's view's coordiante space</param>
    /// <returns>The normalized scoll distance</returns>
    public static float NormalizeScrollDistance(this ScrollRect scrollRect, int axis, float distance)
    {
        // Based on code in ScrollRect's internal SetNormalizedPosition method
        var viewport = scrollRect.viewport;
        var viewRect = viewport != null ? viewport : scrollRect.GetComponent<RectTransform>();
        var viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);

        var content = scrollRect.content;
        var contentBounds = content != null ? content.TransformBoundsTo(viewRect) : new Bounds();

        var hiddenLength = contentBounds.size[axis] - viewBounds.size[axis];
        return distance / hiddenLength;
    }

    /// <summary>
    /// Scroll the target element to the vertical center of the scroll rect's viewport.
    /// Assumes the target element is part of the scroll rect's contents.
    /// </summary>
    /// <param name="scrollRect">Scroll rect to scroll</param>
    /// <param name="target">Element of the scroll rect's content to center vertically</param>
    public static void ScrollToCenter(this ScrollRect scrollRect, RectTransform target, RectTransform.Axis axis = RectTransform.Axis.Vertical)
    {
        // The scroll rect's view's space is used to calculate scroll position
        var view = scrollRect.viewport ?? scrollRect.GetComponent<RectTransform>();

        // Calcualte the scroll offset in the view's space
        var viewRect = view.rect;
        var elementBounds = target.TransformBoundsTo(view);

        // Normalize and apply the calculated offset
        if (axis == RectTransform.Axis.Vertical)
        {
            var offset = viewRect.center.y - elementBounds.center.y;
            var scrollPos = scrollRect.verticalNormalizedPosition - scrollRect.NormalizeScrollDistance(1, offset);
            scrollRect.verticalNormalizedPosition = Mathf.Clamp(scrollPos, 0, 1);
        }
        else
        {
            var offset = viewRect.center.x - elementBounds.center.x;
            var scrollPos = scrollRect.horizontalNormalizedPosition - scrollRect.NormalizeScrollDistance(0, offset);
            scrollRect.horizontalNormalizedPosition = Mathf.Clamp(scrollPos, 0, 1);
        }
    }

}