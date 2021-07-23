using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Result - окно, которое должно отображать результат прохождения теста.
 * Результат должен отображаться в количестве балов, которое набрал
 * пользователь, его рейтинга и % правильно отвеченых вопросов.
 * 
 * У окон должна быть своя последовательность появления на экране. Система
 * должна знать какое окно за каким отображать. 
 * Когда окно которое загружается должен показываться загрузчик. после этого
 * само окно. Окно теста должно быть переведено в окно которое его вызывало
 * если тест обязательный для прохождения, то тест не будет считаться 
 * пройденым.
 */
public class TestStatsUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    public TextMeshProUGUI rateTMP;
    public TextMeshProUGUI triesTMP;
    public TextMeshProUGUI rightAnswersTMP;
    public TextMeshProUGUI testNameTMP;
    public TestsScreensUIController MainScreen;
    public Image Background;
    public Button GoOnButton;
    public TextMeshProUGUI GoOnButtonText;
    private Dictionary<string, string> _screenNameTestName = new Dictionary<string, string>();

    private IScreenController _nextScreen;
    public IScreenController NextScreen
    {
        get => _nextScreen;
        set
        {
            value.PrevScreen = this;
            _nextScreen = value;
        }
    }
    public IScreenController PrevScreen { get; set; }
    public string _screenName = "TestStatsScreen";
    public string ScreenName
    {
        get => _screenName;
        set => _screenName = value;
    }
    public Image GetBackground() => Background;

    void Awake()
    {
        _screenNameTestName.Add("MathTest", "Арифметика");
        _screenNameTestName.Add("FacesTest", "Лица");
        _screenNameTestName.Add("WordsTest", "Слова");
        _screenNameTestName.Add("SubjectsTest", "Предметы");
        _screenNameTestName.Add("NumbersTest", "Цифры");
        _screenNameTestName.Add("WordsColorTest", "Цвет слов");
        _screenNameTestName.Add("TongueTwistersTest", "Скороговорки");
        _screenNameTestName.Add("NeuroGymTest", "Нейрогимнастика");
    }

    private void OnEnable()
    {
        if (PrevScreen?.ScreenName == "MainScreen")
        {
            testNameTMP.text = _screenNameTestName[NextScreen.ScreenName];
            //var s = (NextScreen as IDecorableScreen).GetBackground().sprite;
            //Background.sprite = Sprite.Create(s.texture, s.textureRect, new Vector2(0.5f, 0.5f));
            //Background.color = (NextScreen as IDecorableScreen).GetBackground().color;

            //GoOnButtonText.text = "НАЧАТЬ";
        }
        else
        if (PrevScreen != null)
        {
            //Result result = (PrevScreen as IResultableScreen).GetResult() as Result;
            GoOnButtonText.text = "ЗАНОВО";
            //RateText.text = result?.Grade.ToString() ?? "";
            //float percent = (float)result?.TruePositive / result?.QuestsCount * 100 ?? 0;
            //RightAnswersText.text = Mathf.RoundToInt(percent).ToString() + "%";
            triesTMP.text = "1";
        }
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            var mainScreen = screensController.GetScreenByName("MainScreen");
            screensController.Activate(mainScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }

    public void OnGoOnButtonClick()
    {
        if (NextScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(NextScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }

}