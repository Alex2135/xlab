using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class NameByFaceTestUIController : MonoBehaviour, IResetableScreenController
{
    // Screen objects
    public Image faceImage;
    public string _screenName;
    public GameObject nameButtonPrefab;
    public GameObject horizontalLayoutPrefab;
    public RectTransform wordsPanel;
    public TextMeshProUGUI questNumber;
    public TextMeshProUGUI scoreTMP;
    public TextMeshProUGUI hiddenName;

    // Screen logic objects
    public QuestionView questionView;
    public ShowQuestResult questResultView;
    public List<LoadedImage> loadedImages;
    public NameByFaceTestView testView;
    private bool isButtonPressed;

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

        // Emulate test data
        testView.test = SimulateTestData();
    }

    private Test SimulateTestData()
    {
        Test result = new Test();

        // Mark loaded images 
        for (int i = 0; i < loadedImages.Count; i++)
        {
            loadedImages[i]._name += $"_{i}";
        }

        List<Question> questions = new List<Question>();
        // Images was downloaded with indexes in suffix.
        // Generation sameles answers for questions.
        List<Answer> answers = new List<Answer>();
        foreach (var img in loadedImages)
        {
            var nameLastname = img._name.Split('_')?[0];
            
            var ans = new Answer();
            ans.content = nameLastname;
            ans.isRight = false;
            answers.Add(ans);

            var quest = new Question();
            quest.question = nameLastname;
            quest.file = null;
            quest.answers = null;
            questions.Add(quest);
        }

        // Set additional names to answers
        var names = new List<string>(){ "Vasya Pupkin", "Masha Kalatushkina", "Kuzya Vinnik"};
        foreach (var name in names)
        {
            var ans = new Answer();
            ans.content = name;
            ans.isRight = false;
            answers.Add(ans);
        }

        // Set answers to questions
        foreach (var q in questions)
        {
            q.answers = new Answer[answers.Count];
            answers.Shuffle();
            for (int i = 0; i < answers.Count; i++)
            {
                q.answers[i] = answers[i].Clone() as Answer;
            }
            // Mark right answers
            foreach (var ans in q.answers)
            {
                if (q.question.StartsWith(ans.content))
                {
                    ans.isRight = true;
                    break;
                }
            }
        }

        //questions.ShuffleItems();
        result.quests = questions;
        result.name = "FaceByName";
        result.time = 0;
        result.reward = 10;
        result.penaltie = 0;

        return result;
    }

    void OnEnable()
    {
        isButtonPressed = false;
        questNumber.text = $"1 из {(testView.test as Test).quests.Count}";
        questionView._quest._image = faceImage;
        questionView._quest._text = hiddenName;

        testView.GetNextQuestion();
        testView.RefreshQuestDataOnQuestionView(loadedImages);
        questResultView.SetQuestionView(questionView);
        //questResultView.greenBG = new Color(0x63/255f, 0xCA/255f, 0x85/255f);
    }

    private void OnNameButtonClick(string _name)
    {
        if (_name != null && _name != "" && !isButtonPressed)
        {
            int rightButtonIdx = 0;
            int selectedButtonIdx = 0;

            var answers = testView.CurrentQuestion.answers;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i].isRight)
                    rightButtonIdx = i;
                if (answers[i].content.StartsWith(_name))
                    selectedButtonIdx = i;
            }

            //Debug.Log("Actial: " + questionView._quest._text.text);
            //Debug.Log("Right: " + answers[rightButtonIdx].content);
            //Debug.Log("Selected: " + answers[selectedButtonIdx].content);

            StartCoroutine(ShowResult(selectedButtonIdx, rightButtonIdx));
        }
    }

    IEnumerator ShowResult(int _selectedId, int _rightId)
    {
        isButtonPressed = true;
        float delay = 1f;

        questResultView.ShowQuestionResult(_selectedId, _rightId);
        if (_selectedId == _rightId) testView.GetReward();
        else testView.GetPenaltie();
        scoreTMP.text = testView.ResultScore.Grade.ToString();
        yield return new WaitForSeconds(delay);
        ResetQuestionView();
        testView.GetNextQuestion();
        if (testView.CurrentQuestion != null)
        {
            testView.RefreshQuestDataOnQuestionView(loadedImages);
        }
        else
        {
            gameObject.SetActive(false);
            (NextScreen as MonoBehaviour).gameObject.SetActive(true);
        }
        if (testView.QuestIndex + 1 <= (testView.test as Test).quests.Count)
            questNumber.text = $"{testView.QuestIndex + 1} из {(testView.test as Test).quests.Count}";
        isButtonPressed = false;
    }

    void OnDisable()
    {
        ResetQuestionView();
    }

    private void ResetQuestionView()
    {
        questionView._quest.ResetImageAndText();
        foreach (var ans in questionView._answers)
            ans.ResetImageAndText();
        testView.wordsPanelCreator.DestroyButtons();
    }

    public void ResetScreenState()
    {
        throw new NotImplementedException();
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

    private bool isShuffle;

    public NameByFaceTestView()
    {
        wordsPanelCreator = new WordsPanel();
        _questImagesMapper = new QuestImagesMapper();
        isShuffle = false;
    }

    public void RefreshQuestDataOnQuestionView(List<LoadedImage> _netImages)
    {
        if (_netImages == null) throw new Exception("Images are not set");
        if (CurrentQuestionView == null) throw new Exception("Question view is null");

        if (!isShuffle)
        {
            _questImagesMapper.MapQuestsAndImages(test, _netImages);
            _questImagesMapper.mapQuestAndImages.Shuffle();
            isShuffle = true;
        }

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

    public void ResetTestAndQuestView()
    {
        throw new NotImplementedException();
    }
}
