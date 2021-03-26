using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestsScreensUIController : MonoBehaviour, IScreenController
{
    public MathTestUIController mathTestUIC;
    public FacesTestUIController facesTestUIC;
    public WordsTestUIController wordsTestUIC;

    private ScreensUIController _screensController;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName { get; set; }

    private void Awake()
    {
        ScreenName = "MainScreen";
        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(mathTestUIC);
        _screensController.Add(facesTestUIC);
        _screensController.Add(wordsTestUIC);
        _screensController.DiactivateScreens();
    }

    public void OnAssignmentTestButtonClick(string _screen)
    {
        if (_screen != null)
        {
            var screen = _screensController.GetScreenByName(_screen);
            _screensController.Activate(screen, this);
        }
        else
        {
            throw new Exception("Wrong screen id!");
        }
    }

    public object GetResult()
    {
        return null;
    }
}
