using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NameByFaceTestUIController : MonoBehaviour, IScreenController
{
    // Screen objects
    public Image faceImage;
    public string _screenName;
    public GameObject nameButtonPrefab;
    public GameObject horizontalLayoutPrefab;
    public RectTransform wordsPanel;
    public TextMeshProUGUI questNumber;
    public TextMeshProUGUI hiddenName;

    // Screen logic objects
    public QuestionView questionView;
    public ShowQuestResult questResultView;
    public List<LoadedImage> loadedImages;
    public NameByFaceTestView testView;

    // TODO: Fetch test data from source (web, file) and throw it to testview
    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void Awake()
    {
        questionView = new QuestionView();
        testView = new NameByFaceTestView();
        testView.CurrentQuestionView = questionView;

        // Set screens objects to words panel creator
        testView.onNameButtonClick = OnNameButtonClick;
        testView.wordsPanelCreator = new WordsPanel();
        testView.wordsPanelCreator.ParentPanel = wordsPanel;
        testView.wordsPanelCreator.ButtonWordPrefab = nameButtonPrefab;
        testView.wordsPanelCreator.HorizontalLayoutPrefab = horizontalLayoutPrefab;

        // Mark loaded images 
        for (int i = 0; i < loadedImages.Count; i++)
        {
            loadedImages[i]._name += $"_{i}";
        }

        // Emulate test data
        testView.test = EmulateTestData();
    }

    private Test EmulateTestData()
    {
        Test result = new Test();

        // Генерация одинаковых ответов на все вопросы
        List<Answer> answers = new List<Answer>();
        foreach (var img in loadedImages)
        {
            var nameLastname = img._name.Split('_')?[0];
            var ans = new Answer() {
                content = nameLastname,
                isRight = false
            };
            answers.Add(ans);
        }
        var names = new List<string>(){ "Vasya Pupkin", "Masha Kalatushkina", "Kuzya Vinnik", "UUU ska", "Suka Blyad" };

        return result;
    }

    void OnEnable()
    {
        questNumber.text = "1 из 4";
        questionView._quest._image = faceImage;
        questionView._quest._text = hiddenName;

        testView.RefreshQuestDataOnQuestionView(loadedImages);
        questResultView.SetQuestionView(questionView);
    }

    private void OnNameButtonClick(string _name)
    { 
        if (questionView._quest._text.text.StartsWith(_name))
        {
            Debug.Log("Yes");
            testView.GetNextQuestion();
            
        }
        else
        {
            Debug.Log("No");
        }

        testView.GetNextQuestion();
    }

    void OnDisable()
    {
        ResetQuestionView();
    }

    // TODO: Complete method and call it in OnNameButtonClick
    private void ResetQuestionView()
    {
        questionView._quest.ResetImageAndText();
        foreach (var ans in questionView._answers)
        {
            ans.ResetImageAndText();
        }
        loadedImages.Clear();
        testView.wordsPanelCreator.Words.Clear();
    }
}

public class NameByFaceTestView : ITestView, ITest
{
    // Test info
    public ITest test { get; set; } // Casted test object
    public int QuestIndex { get => (test as Test).quesitonIdx; } 
    public Question CurrentQuestion { get => test.CurrentQuestion; }

    // Screen view of current question
    public QuestionView CurrentQuestionView { get; set; }
    private QuestImagesMapper _questImagesMapper; // Для сопоставления скачаных изображений с квестами в тестах

    // Screen view words panel
    public WordsPanel wordsPanelCreator;
    public Action<string> onNameButtonClick;

    public Result ResultScore { get => test.ResultScore; set => test.ResultScore = value; }

    public NameByFaceTestView()
    {
        wordsPanelCreator = new WordsPanel();
        _questImagesMapper = new QuestImagesMapper();
    }

    public void RefreshQuestDataOnQuestionView(List<LoadedImage> _netImages)
    {
        if (_netImages == null) throw new Exception("Images are not set");
        if (CurrentQuestionView == null) throw new Exception("Question view is null");

        // Generate quests in test corresponding _netImages

        SetQuestText(_netImages);
        SetQuestImages(_netImages);
    }
    public void SetQuestImages(List<LoadedImage> _images)
    {
        if (CurrentQuestionView._quest == null) throw new Exception("Quest UI elements for question view ware not set!");

        var faceView = CurrentQuestionView._quest._image;
        var nameView = CurrentQuestionView._quest._text;
        var faceImage = _images[QuestIndex]._image;
        nameView.text = _images[QuestIndex]._name;
        LoadedImage.SetTextureToImage(ref faceView, faceImage);
    }

    public void SetQuestText(List<LoadedImage> _images)
    {
        // { "Vasya Pupkin", "Masha Kalatushkina", "Kuzya Vinnik", "UUU ska", "Suka Blyad" };

        wordsPanelCreator.Words = GetFacesNamesFromTest();
        var buttons = wordsPanelCreator.GenerateWordsButtons(onNameButtonClick);
        CurrentQuestionView._answers = buttons;
    }

    private List<string> GetFacesNamesFromTest()
    {
        var result = new List<string>();
        var testData = (test as Test);
        var quest = testData.CurrentQuestion;
        foreach (var ans in quest.answers)
            result.Add(ans.content);

        return result;
    }

    public Question GetNextQuestion()
    {
        return test.GetNextQuestion();
    }

    public int GetPenaltie()
    {
        return (test as IRewarder).GetPenaltie();
    }

    public int GetReward()
    {
        return (test as IRewarder).GetReward();
    }

    public float GetTime()
    {
        return test.GetTime();
    }
}