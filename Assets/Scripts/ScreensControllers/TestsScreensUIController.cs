using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestsScreensUIController : MonoBehaviour, IScreenController
{
    [Header("Tests UI Controllers")]
    public MathTestUIController mathTestUIC;
    public FacesTestUIController facesTestUIC;
    public WordsTestUIController wordsTestUIC;
    public SubjectsTestUIController subjectsTestUIC;
    public NumbersTestUIController numbersTestUIC;
    public WordsColorTestUIController wordsColorTestUIC;
    public TongueTwistersTestUIController tongueTwistersUIC;
    public NeuroGymTestUIController neuroGymUIC;

    [Header("Tests UI scores")]
    public TextMeshProUGUI mathScoreTMP;
    public TextMeshProUGUI facesScoreTMP;
    public TextMeshProUGUI wordsScoreTMP;
    public TextMeshProUGUI subjectsScoreTMP;
    public TextMeshProUGUI numbersScoreTMP;
    public TextMeshProUGUI wordsColorScoreTMP;
    public TextMeshProUGUI tongueTwistersScoreTMP;
    public TextMeshProUGUI neuroGymScoreTMP;

    [Header("Test results views")]
    public TestResultController testResultUIC;
    public TestStatsUIController testsStatsUIC;

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
        user.AddTestStats("WordsColor");
        user.AddTestStats("TongueTwisters");
        user.AddTestStats("NeuroGym");
        user.SaveData();

        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(testsStatsUIC);
        _screensController.Add(testResultUIC);

        _screensController.Add(mathTestUIC);
        _screensController.Add(facesTestUIC);
        _screensController.Add(wordsTestUIC);
        _screensController.Add(subjectsTestUIC);
        _screensController.Add(numbersTestUIC);
        _screensController.Add(wordsColorTestUIC);
        _screensController.Add(tongueTwistersUIC);
        _screensController.Add(neuroGymUIC);

        _screensController.DiactivateScreens();
    }

    void OnEnable()
    {
        mathScoreTMP.text = $"{UserModel.GetLastScore("Math")}";
        facesScoreTMP.text = $"{UserModel.GetLastScore("Faces")}";
        wordsScoreTMP.text = $"{UserModel.GetLastScore("Words")}";
        subjectsScoreTMP.text = $"{UserModel.GetLastScore("Subjects")}";
        numbersScoreTMP.text = $"{UserModel.GetLastScore("Numbers")}";
        wordsColorScoreTMP.text = $"{UserModel.GetLastScore("WordsColor")}";
        tongueTwistersScoreTMP.text = $"{UserModel.GetLastScore("TongueTwisters")}";
        tongueTwistersScoreTMP.text = $"{UserModel.GetLastScore("NeuroGym")}";
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
