using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NameByFaceTestUIController : MonoBehaviour, IScreenController
{
    public string _screenName;
    public GameObject nameButtonPrefab;
    public GameObject horizontalLayoutPrefab;
    public RectTransform wordsPanel;
    public TextMeshProUGUI questNumber;
    public Image faceImage;
    public TextMeshProUGUI hiddenName;
    public ShowQuestResult questResultView;
    public QuestionView questionView;
    public int questIndex;
    public List<LoadedImage> loadedImages;
    public List<string> names;
    public NameByFaceTestView testView;

    // TODO: Fetch test data from source (web, file) and throw it to testview

    public string ScreenName { get => _screenName; set => _screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void OnEnable()
    {
        questNumber.text = "1 из 4";
        questIndex = 0;
        questionView = new QuestionView();
        questionView._quest._image = faceImage;
        questionView._quest._text = hiddenName;

        testView = new NameByFaceTestView();
        testView.onButtonClick = OnNameButtonClick;
        testView.CurrentQuestionView = questionView;
        testView.wordsPanelCreator = new WordsPanel();
        testView.wordsPanelCreator.Words = names;
        testView.wordsPanelCreator.ParentPanel = wordsPanel;
        testView.wordsPanelCreator.ButtonWordPrefab = nameButtonPrefab;
        testView.wordsPanelCreator.HorizontalLayoutPrefab = horizontalLayoutPrefab;
        testView.SetDataToQuestionView(loadedImages);

        questResultView.SetQuestionView(questionView);
        loadedImages.ShuffleItems();
    }

    private void OnNameButtonClick(string _name)
    { 
        if (questionView._quest._text.text.StartsWith(_name))
        {
            Debug.Log("Yes");
        }
        else
        {
            Debug.Log("No");
        }

        testView.GetNextQuestion();
    }

    // TODO: Complete method and call it in OnNameButtonClick
    private void ResetQuestionView()
    {

    }
}

public class NameByFaceTestView : ITestView, ITest
{
    public WordsPanel wordsPanelCreator;
    public Action<string> onButtonClick;
    public List<string> Names { get; set; }
    public ITest test { get; set; }
    public QuestionView CurrentQuestionView { get; set; }
    private List<LoadedImage> loadedImages { get; set; }
    public Result ResultScore { get => test.ResultScore; set => test.ResultScore = value; }
    public Question CurrentQuestion { get => test.CurrentQuestion; }
    private int _questIndex;
    private QuestImagesMapper _questImagesMapper;

    public NameByFaceTestView()
    {
        wordsPanelCreator = new WordsPanel();
        _questImagesMapper = new QuestImagesMapper();
        _questIndex = 0;
        Names = new List<string>();
    }

    // TODO: Delete after deploy faces test data
    private void SetupTest()
    {
        test = new Test();
        List<Answer> answers = new List<Answer>();

        foreach (var img in loadedImages)
        {
            answers.Add(new Answer() { content = img._name });
            Names.Add(img._name);
        }

        int i = 0;
        foreach (var img in loadedImages)
        {
            var quest = new Question();

            quest.file = new File() { name=img._name };
            quest.answers = new Answer[answers.Count];
            answers.CopyTo(quest.answers);
            (test as Test).quests.Add(quest);
            img._name += $"_{i}";
            i++;
        }

        _questImagesMapper.MapQuestsAndImages(test, loadedImages);
        loadedImages.ShuffleItems();
    }

    public void SetDataToQuestionView(List<LoadedImage> _netImages)
    {
        if (_netImages == null) throw new Exception("Images are not set");
        if (CurrentQuestionView == null) throw new Exception("Question view is null");
        loadedImages = _netImages;
        SetupTest();
        SetQuestText();
        SetQuestImages(loadedImages);
    }
    public void SetQuestImages(List<LoadedImage> _images)
    {
        var buttons = wordsPanelCreator.GenerateWordsButtons(onButtonClick);
        CurrentQuestionView._answers = buttons;
        if (CurrentQuestionView._quest == null) throw new System.Exception("Quest UI elements for question view ware not set!");
        SetQuestFace();
    }

    private bool SetQuestFace()
    {
        if (_questIndex < 0 || _questIndex >= loadedImages.Count) return false;

        var image = loadedImages[_questIndex];
        LoadedImage.SetTextureToImage(ref CurrentQuestionView._quest._image, image._image);
        CurrentQuestionView._quest._text.text = image._name;
        return true;
    }

    public void SetQuestText()
    {
        // { "Vasya Pupkin", "Masha Kalatushkina", "Kuzya Vinnik", "UUU ska", "Suka Blyad" };

        
        wordsPanelCreator.Words = Names;
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