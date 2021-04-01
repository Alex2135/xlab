using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * 
 */
public class MathTestView : ITestView, ITest, IRewarder
{
    private Test _test;
    private QuestImagesMapper _questImagesMapper;
    public QuestionView CurrentQuestionView { get; set; }
    public ITest test { get => _test; set => _test = value as Test; }
    public Result ResultScore { get => test.ResultScore; set => test.ResultScore = value; }
    public Question CurrentQuestion { get => test.CurrentQuestion; }

    public MathTestView()
    {
        _questImagesMapper = new QuestImagesMapper();
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

        Question quest = test.CurrentQuestion ?? throw new Exception("No question to set");
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
        Question quest = test.CurrentQuestion ?? throw new Exception("No question to set");
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