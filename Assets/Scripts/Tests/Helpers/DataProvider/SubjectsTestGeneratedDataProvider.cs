using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsTestGeneratedDataProvider : MonoBehaviour, IDataSource<SubjectsQuestModel>
{
    public List<Texture2D> answers;
    private List<Texture2D> rightAnswers;
    private List<Texture2D> additionAnswers;

    private void InitializeAnswers()
    {
        rightAnswers = new List<Texture2D>();
        additionAnswers = new List<Texture2D>();

        int rightAnswersCount = 4;

        answers = answers.Shuffle();
        
        rightAnswers.AddRange(
            answers.GetRange(0, rightAnswersCount));

        additionAnswers.AddRange(
            answers.GetRange(rightAnswersCount, answers.Count - rightAnswersCount));
    }

    public IEnumerable<SubjectsQuestModel> GetQuests()
    {
        InitializeAnswers();
        var result = new List<SubjectsQuestModel>();
        var quest = new SubjectsQuestModel();

        quest.RightAnswers = rightAnswers.Shuffle();
        quest.AdditionalAnswers = additionAnswers;

        var quests = new Texture2D[rightAnswers.Count];
        int nullsCount = 0;
        int nullImagesCount = (int)(rightAnswers.Count * 3f / 4f);
        rightAnswers.CopyTo(quests);
        
        while (nullsCount != nullImagesCount)
        {
            var idx = Random.Range(0, rightAnswers.Count);
            if (quests[idx] != null)
            {
                quests[idx] = null;
                nullsCount++;
            }
        }

        quest.Quest = new List<Texture2D>(quests);
        result.Add(quest);
        return result;
    }
}
