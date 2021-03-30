using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameByFaceTestView : MonoBehaviour, IScreenController
{
    public string _screenName;
    public GameObject nameButtonPrefab;
    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public List<LoadedImage> loadedImages;
    public List<string> names;
    public RectTransform wordsPanel;
    private List<GameObject> nameButtons;

    void OnEnable()
    {
        names = names ?? new List<string>(){ "Vasya Pupkin", "Masha Kalatushkina", "Kuzya Vinnik" };
        foreach(var img in loadedImages)
        {
            names.Add(img._name);
        }

    }


}
