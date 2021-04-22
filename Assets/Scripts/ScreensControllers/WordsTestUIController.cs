using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsTestUIController : MonoBehaviour, IScreenController, IDecorableScreen, IResultableScreen
{
    public Button BackButton;
    public Image Background;
    public string _screenName;
    public RememberWords rememberWordsUIC;
    public TestStatsUIController testResultView;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName 
    {
        get { return _screenName; }
        set { _screenName = value; }
    }

    public Image GetBackground()
    {
        return Background;
    }

    public object GetResult()
    {
        return "result";
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

    void Awake()
    {
        rememberWordsUIC.NextScreen = testResultView;

        var screensController = ScreensUIController.GetInstance();
        screensController.Add(rememberWordsUIC);
        screensController.Activate(rememberWordsUIC, null, false);
    }
}
