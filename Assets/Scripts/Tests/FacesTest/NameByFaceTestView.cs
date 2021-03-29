using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameByFaceTestView : MonoBehaviour, IScreenController
{
    public string _screenName;
    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public List<LoadedImage> loadedImages;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
