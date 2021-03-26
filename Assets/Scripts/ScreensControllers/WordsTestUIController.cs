using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsTestUIController : MonoBehaviour, IScreenController
{
    public Button BackButton;
    public string _screenName;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName 
    {
        get { return _screenName; }
        set { _screenName = value; }
    }

    public object GetResult()
    {
        throw new System.NotImplementedException();
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }

    void Start()
    {
        ScreenName = "WordsTest";
    }
}
