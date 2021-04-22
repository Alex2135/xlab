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
    public TextMeshProUGUI RateText;
    public TextMeshProUGUI TriesText;
    public TextMeshProUGUI RightAnswersText;
    public TextMeshProUGUI TestName;
    public TestsScreensUIController MainScreen;
    public Image Background;
    public Button GoOnButton;
    public TextMeshProUGUI GoOnButtonText;
    private Dictionary<string, string> _screenNameTestName = new Dictionary<string, string>();

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

    void Awake()
    {
        _screenNameTestName.Add("MathTest", "Арифметика");
        _screenNameTestName.Add("FacesTest", "Лица");
        _screenNameTestName.Add("WordsTest", "Слова");
        _screenNameTestName.Add("SubjectsTest", "Предметы");
    }

    void Start()
    {
        ScreenName = "TestStatsScreen";
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


    private void OnEnable()
    {
        if (PrevScreen?.ScreenName == "MainScreen")
        {
            TestName.text = _screenNameTestName[NextScreen.ScreenName];
            var s = (NextScreen as IDecorableScreen).GetBackground().sprite;
            Background.sprite = Sprite.Create(s.texture, s.textureRect, new Vector2(0.5f, 0.5f));
            GoOnButtonText.text = "НАЧАТЬ";
        }
        else 
        if (PrevScreen != null)
            //&& (PrevScreen as IResultableScreen).GetResult() is Result result)
        {
            Result result = (PrevScreen as IResultableScreen).GetResult() as Result;
            GoOnButtonText.text = "ЗАНОВО";
            RateText.text = result?.Grade.ToString() ?? "";
            //var s = (NextScreen as IDecorableScreen).GetBackground().sprite;
            //Background.sprite = Sprite.Create(s.texture, s.textureRect, new Vector2(0.5f, 0.5f));
            float percent = (float)result?.TruePositive / result?.QuestsCount * 100 ?? 0;
            RightAnswersText.text = Mathf.RoundToInt(percent).ToString() + "%";
            TriesText.text = "1";
        }
    }

    public Image GetBackground()
    {
        return Background;
    }
}