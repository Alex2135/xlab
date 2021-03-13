using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * TestView предоставляет 
 */
public class TestView : Test
{
    /*
     * QuestionView - изображения и текст вопроса и ответов
     * List<LoadedImage> - набор изображений всех вопросов и ответов на них
     */
    private Dictionary<Question, List<LoadedImage>> _questsWithImgs = new Dictionary<Question, List<LoadedImage>>();

    private void SetQuestText(ref QuestionView _view)
    {
        Question quest = currentQuestion ?? throw new Exception("No question to set");
        _view.SetQuestText(quest.question);
        var answers = quest.answers;
        for (int i = 0; i < answers.Length; i++)
            _view.SetAnswerText(i, answers[i].content);
    }

    private void SetQuestImages(ref QuestionView _view, List<LoadedImage> _images)
    {
        Question quest = currentQuestion ?? throw new Exception("No question to set");

        _view._quest._image.gameObject.SetActive( quest.isQuestionImageExist );

        bool isAnswers = quest.isAnswersImagesExists;
        foreach (var ans in _view._answers)
        {
            ans._image.gameObject.SetActive( isAnswers );
            ans._image.sprite = null;
        }

        foreach (var img in _images)
        {
            var splitedName = img._name.Split('_');
            switch (splitedName[0])
            {
                case "quest":
                    LoadedImage.SetTextureToImage(ref _view._quest._image, img._image);
                    break;
            
                case "answer":
                    int answerIdx = Convert.ToInt32(splitedName[1]);
                    LoadedImage.SetTextureToImage(ref _view._answers[answerIdx]._image, img._image);
                    break;

                default: throw new Exception("Invalid image name");
            }
        }
    }

    public void SetQuestionView(ref QuestionView _questView, List<LoadedImage> _netImages)
    {
        int idx = this.quesitonIdx;
        List<LoadedImage> questImages = _netImages.Where(
                questImage =>
                {
                    var qIdx = questImage?._name?.Split('_')?.Last() ?? "-1";
                    bool result = Convert.ToInt32(qIdx) == idx;
                    return result;
                }
            ).ToList();

        foreach (var img in questImages)
        {
            Debug.Log($"Selected images {img._name}");
        }

        SetQuestImages(ref _questView, questImages);
        SetQuestText(ref _questView);
    }
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