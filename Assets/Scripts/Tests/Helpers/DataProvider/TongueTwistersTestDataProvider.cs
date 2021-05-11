using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewQuestionModel;

public class TongueTwistersTestDataProvider : MonoBehaviour, IDataSource<TongueTwistersQuestModel>
{
    public List<string> tongueTwisters;

    public IEnumerable<TongueTwistersQuestModel> GetQuests(TestWholeStats _test)
    {
        if (_test == null) throw new ArgumentNullException("Argument _test is null");
        if (tongueTwisters == null ||
            tongueTwisters.Count == 0) throw new Exception("tongueTwisters list is not set");

        var result = new List<TongueTwistersQuestModel>();

        foreach (var questText in tongueTwisters)
        {
            var quest = new TongueTwistersQuestModel();
            quest.Quest.Add($"« {questText} »");
            result.Add(quest);
        }

        return result;
    }
}
