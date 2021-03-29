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
    public ResultsUiController testsResultUIC;

    private ScreensUIController _screensController;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName { get; set; }

    private void Awake()
    {
        ScreenName = "MainScreen";
        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(testsResultUIC);
        _screensController.Add(mathTestUIC);
        _screensController.Add(facesTestUIC);
        _screensController.Add(wordsTestUIC);
        _screensController.DiactivateScreens();
    }

    public void OnAssignmentTestButtonClick(string _screenName)
    {
        if (_screenName != null)
        {
            var resultScreen = _screensController.GetScreenByName("ResultScreen");
            var screen = _screensController.GetScreenByName(_screenName);
            resultScreen.NextScreen = screen;
            _screensController.Activate(resultScreen, this);
        }
        else
        {
            throw new Exception("Wrong screen id!");
        }
    }
}
