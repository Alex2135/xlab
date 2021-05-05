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
    public SubjectsTestUIController subjectsTestUIC;
    public NumbersTestUIController numbersTestUIC;

    public TestStatsUIController testsResultUIC;

    private ScreensUIController _screensController;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName { get; set; }

    private void Awake()
    {
        ScreenName = "MainScreen";
        var user = UserModel.GetInstance(new FileUserDataSource("user.netxt"));
        user.AddTestStats("Math");
        user.AddTestStats("Faces");
        user.AddTestStats("Numbers");
        user.AddTestStats("Subjects");
        user.AddTestStats("Words");
        user.SaveData();

        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(testsResultUIC);
        _screensController.Add(mathTestUIC);
        _screensController.Add(facesTestUIC);
        _screensController.Add(wordsTestUIC);
        _screensController.Add(subjectsTestUIC);
        _screensController.Add(numbersTestUIC);
        _screensController.DiactivateScreens();
    }

    public void OnAssignmentTestButtonClick(string _screenName)
    {
        if (_screenName != null)
        {
            var TestStatsScreen = _screensController.GetScreenByName("TestStatsScreen");
            var screen = _screensController.GetScreenByName(_screenName);
            TestStatsScreen.NextScreen = screen;
            _screensController.Activate(TestStatsScreen, this);
        }
        else
        {
            throw new Exception("Wrong screen id!");
        }
    }
}
