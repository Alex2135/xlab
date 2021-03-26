using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Result - окно, которое должно отображать результат прохождения теста.
 * Результат должен отображаться в количестве балов, которое набрал
 * пользователь. Далее возможно добавление нового функционала с отображением
 * рейтинга, которого достиг пользователь.
 * 
 * У окон должна быть своя последовательность появления на экране. Система
 * должна знать какое окно за каким отображать. 
 * Когда окно которое загружается должен показываться загрузчик. после этого
 * само окно. Окно теста должно быть переведено в окно которое его вызывало
 * если тест обязательный для прохождения, то тест не будет считаться 
 * пройденым.
 */
public class ResultsUiController : MonoBehaviour, IScreenController
{
    public TextMeshProUGUI RateText;
    public TextMeshProUGUI TriesText;
    public TextMeshProUGUI RightAnswersText;
    public TextMeshProUGUI TestName;
    public TestsScreensUIController MainScreen;

    private IScreenController _nextScreen;
    public IScreenController NextScreen
    {
        get { return _nextScreen; }
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }
    public string _screenName;
    public string ScreenName 
    {
        get { return _screenName; }
        set { _screenName = value; } 
    }

    void Start()
    {
        ScreenName = "ResultScreen";
        _nextScreen = MainScreen;
    }

    private void OnEnable()
    {
        if (PrevScreen != null && PrevScreen.GetResult() is Result result)
        {
            RateText.text = result.Grade.ToString();
            float percent = (float)result.TruePositive / result.QuestsCount * 100;
            RightAnswersText.text = Mathf.RoundToInt(percent).ToString() + "%";
            TriesText.text = "1";
        }
    }

    public object GetResult()
    {
        throw new System.NotImplementedException();
    }

}