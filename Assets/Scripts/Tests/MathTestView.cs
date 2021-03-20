using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * TestView предоставляет 
 */
public class MathTestView : ITestView, ITest, IRewarder
{
    private Test _test;
    private QuestImagesMapper _questImagesMapper;
    public QuestionView CurrentQuestionView { get; set; }

    public MathTestView()
    {
        _questImagesMapper = new QuestImagesMapper();
    }

    public ITest test { get { return _test; } set { _test = value as Test; } }

    public Result ResultScore 
    { 
        get { return test.ResultScore; }
        set { test.ResultScore = value; }
    }

    public Question currentQuestion 
    {
        get
        {
            return test.currentQuestion;
        }
    }
    
    /*
    * QuestionView - изображения и текст вопроса и ответов на холсте
    * List<LoadedImage> - набор данных изображений всех вопросов и ответов на них
    */
    public void SetDataToQuestionView(List<LoadedImage> _netImages)
    {
        if (CurrentQuestionView == null) throw new NullReferenceException("Quest view is null");
        if (_netImages == null) throw new ArgumentNullException("_netImages view is null");

        if (_questImagesMapper.mapQuestAndImages.Count == 0)
            _questImagesMapper.MapQuestsAndImages(test, _netImages);
        int idx = (test as Test).quesitonIdx;

        if (_netImages.Count != 0)
        {
            List<LoadedImage> questImages = QuestImagesMapper.GetImagesFromListByIndex(_netImages, idx);
            SetQuestImages(questImages);
        }
        SetQuestText();
    }

    public void SetQuestImages(List<LoadedImage> _images)
    {
        if (_images == null) throw new ArgumentNullException("List of images is null");

        Question quest = test.currentQuestion ?? throw new Exception("No question to set");
        if (quest.isQuestionImageExist)
            CurrentQuestionView._quest._image.gameObject.SetActive(true);

        // If answers images exists set them active
        //bool isAnswers = quest.isAnswersImagesExists;
        foreach (var ans in CurrentQuestionView._answers)
        {
            //ans._image.gameObject.SetActive(isAnswers);
            ans._image.sprite = null;
        }

        // Set image to quest view and images to answers view
        foreach (var img in _images)
        {
            var splitedName = img._name.Split('_');
            switch (splitedName[0])
            {
                case "quest":
                    LoadedImage.SetTextureToImage(ref CurrentQuestionView._quest._image, img._image);
                    break;

                case "answer":
                    int answerIdx = Convert.ToInt32(splitedName[1]);
                    LoadedImage.SetTextureToImage(ref CurrentQuestionView._answers[answerIdx]._image, img._image);
                    break;

                default: throw new Exception("Invalid image name");
            }
        }
    }

    public void SetQuestText()
    {
        Question quest = test.currentQuestion ?? throw new Exception("No question to set");
        CurrentQuestionView.SetQuestText(quest.question);
        var answers = quest.answers;
        for (int i = 0; i < answers.Length; i++)
            CurrentQuestionView.SetAnswerText(i, answers[i].content);
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

class QuestImagesMapper
{
    public Dictionary<Question, List<LoadedImage>> mapQuestAndImages;

    public QuestImagesMapper()
    {
        mapQuestAndImages = new Dictionary<Question, List<LoadedImage>>();
    }

    public void MapQuestsAndImages(ITest test, List<LoadedImage> _netImages)
    {
        var testQuestions = (test as Test).quests;
        if (testQuestions == null) throw new NullReferenceException("Test not set");

        var size = testQuestions.Count;

        for (int i = 0; i < size; i++)
        {
            var key = testQuestions[i];
            List<LoadedImage> val = null;
            if (_netImages != null &&
                _netImages.Count != 0) 
                val = GetImagesFromListByIndex(_netImages, i);
            mapQuestAndImages.Add(key, val);
        }
    }

    public static List<LoadedImage> GetImagesFromListByIndex(List<LoadedImage> _images, int _index)
    {
        if (_images.Count == 0) 
            throw new Exception("GetImagesFromListByIndex: images list is empty");

        var result = _images.Where(
            questImage =>
            {
                var qIdx = questImage?._name?.Split('_')?.Last() ?? "-1";
                return Convert.ToInt32(qIdx) == _index;
            }
        );

        return result.ToList();
    }
}


public interface ITestView
{
    ITest test { get; set; }
    void SetQuestImages(List<LoadedImage> _images);
    void SetDataToQuestionView(List<LoadedImage> _netImages);
    void SetQuestText();
}

[Serializable]
public class QuestionView
{
    public DataUI _quest;
    public List<DataUI> _answers;

    public void SetQuestText(string _text)
    {
        _quest._text.text = _text;
    }

    public void SetAnswerText(int _id, string _text)
    {
        if (_id < 0 || _id >= _answers.Count)
            throw new ArgumentOutOfRangeException("Id out of range");

        _answers[_id]._text.text = _text;
    }
}

[Serializable]
public class DataUI
{
    public TextMeshProUGUI _text;
    public Image _image;
}