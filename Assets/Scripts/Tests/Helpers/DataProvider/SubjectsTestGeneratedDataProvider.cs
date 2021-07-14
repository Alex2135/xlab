using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class SubjectsTestGeneratedDataProvider : MonoBehaviour, IDataSource<SubjectsQuestModel>
{
    public int rightAnswersCount;
    public List<Texture2D> answers;

    public IEnumerable<SubjectsQuestModel> GetQuests(TestWholeStats test)
    {
        List<Texture2D> rightAnswers = new List<Texture2D>();
        List<Texture2D> additionAnswers = new List<Texture2D>();
        SubjectsQuestModel quest = new SubjectsQuestModel();
        List<SubjectsQuestModel> result = new List<SubjectsQuestModel>();

        answers = answers.Shuffle();

        rightAnswers.AddRange(
            answers.GetRange(0, rightAnswersCount));

        additionAnswers.AddRange(
            answers.GetRange(rightAnswersCount, answers.Count - rightAnswersCount));

        quest.RightAnswers = rightAnswers.Shuffle();
        quest.AdditionalAnswers = additionAnswers;

        var quests = new Texture2D[rightAnswers.Count];
        int nullsCount = 0;
        int nullImagesCount = rightAnswers.Count;
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
