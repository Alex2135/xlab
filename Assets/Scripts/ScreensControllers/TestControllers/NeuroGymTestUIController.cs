using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuroGymTestUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    public Image background;
    public string screenName;
    public NeuroGymTestView view;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public Image GetBackground() => background;
    private ScreensUIController _screensController;

    void OnEnable()
    {
        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(view);
        view.PrevScreen = PrevScreen;
        //view.NextScreen = _screensController.GetScreenByName("TestResultScreen");

        _screensController.Activate(view, null, false);
    }


}
