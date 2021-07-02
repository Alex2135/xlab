using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumbersTestUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    public Image background;
    public NumbersTestView view;

    public string screenName;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    private ScreensUIController _screensController;

    public Image GetBackground()
    {
        return background;
    }

    void OnEnable()
    {
        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(view);
        view.PrevScreen = PrevScreen;
        view.NextScreen = _screensController.GetScreenByName("TestResultScreen");

        _screensController.Activate(view, null, false);
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            _screensController.DiactivateScreens();
            _screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }
}
