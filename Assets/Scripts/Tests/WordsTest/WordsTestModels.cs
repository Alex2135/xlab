﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewQuestionModel
{
    // Question model for Test raw data
    public class WordsQuestModel : IGenericQuestModel<List<string>, List<string>>
    {
        public List<string> Quest { get => null; set { } } // no question in word test
        public List<string> RightAnswers { get; set; }
        public List<string> AdditionalAnswers { get; set; }

        public WordsQuestModel()
        {
            RightAnswers = new List<string>();
            AdditionalAnswers = new List<string>();
        }
    }

    // Question model for test presenter
    public class WordsAdaptedQuestModel : IAdaptedQuestModel<string, string>
    {
        public Dictionary<int, string> Quest { get => null; set { } }
        public Dictionary<int, string> RightAnswers { get; set; }
        public Dictionary<int, string> AdditionalAnswers { get; set; }

        public WordsAdaptedQuestModel()
        {
            RightAnswers = new Dictionary<int, string>();
            AdditionalAnswers = new Dictionary<int, string>();
        }

        public Dictionary<int, string> GetAllQuests()
        {
            if (RightAnswers.Count == 0 || AdditionalAnswers.Count == 0)
                throw new System.Exception("Answers must added to WordsAdaptedQuestModel");

            var result = new Dictionary<int, string>();
            foreach (var ans in RightAnswers)
                result.Add(ans.Key, ans.Value);
            foreach (var ans in AdditionalAnswers)
                result.Add(ans.Key, ans.Value);
            
            return result;
        }
    }

    // Question model for test view
    public class WordsQuestView : IAdaptedQuestToView
    {
        public Dictionary<int, GameObject> Quest { get => null; set { } }
        public Dictionary<int, GameObject> RightAnswers { get; set; }
        public Dictionary<int, GameObject> AdditionalAnswers { get; set; }

        public WordsQuestView()
        {
            RightAnswers = new Dictionary<int, GameObject>();
            AdditionalAnswers = new Dictionary<int, GameObject>();
        }
    }


    public class WordsTestGeneratedDataSource : IDataSource<WordsQuestModel>
    {
        IEnumerable<WordsQuestModel> IDataSource<WordsQuestModel>.GetQuests()
        {
            var result = new List<WordsQuestModel>();
            var quest1 = new WordsQuestModel();
            quest1.RightAnswers = new List<string>()
            {
                "Вор", "Клубника", "Шишка", "Спам", "Перевод"
            };
            quest1.AdditionalAnswers = new List<string>()
            {
                "Дубина", "Букет", "Волк", "Елка", "Роса"
            };
            result.Add(quest1);

            return result;
        }
    }

    public class WordsTestModel : ATestModel<WordsQuestModel>
    {
        private IDataSource<WordsQuestModel> _dataSource;
        private List<WordsQuestModel> _questions;
        public int PointsPerQuest { get; set; }
        public IDataSource<WordsQuestModel> DataSource { set => _dataSource = value; }

        public WordsTestModel(IDataSource<WordsQuestModel> _dataSource)
        {
            DataSource = _dataSource;
            _questions = _dataSource.GetQuests() as List<WordsQuestModel>;
            questionIndex = -1;
            rightQuestions = 0;
            wrongQuestions = 0;
            PointsPerQuest = 10;
        }

        public override (WordsQuestModel, int)? GetNextQuestion()
        {
            questionIndex++;
            if (questionIndex < _questions.Count)
                return (_questions[questionIndex], questionIndex);
            else
                return null;
        }

        public override void RewardRightAnswer()
        {
            rightQuestions++;
        }

        public override void PenaltieWrongAnswer()
        {
            wrongQuestions++;
            if (wrongQuestions > 4) wrongQuestions = 4;
        }

        public override int GetScore()
        {
            int maxScore = _questions.Count * PointsPerQuest;
            int result = rightQuestions * PointsPerQuest - wrongQuestions * 1/4 * maxScore;
            return result;
        }

        public override int GetQuestsCount()
        {
            return _questions.Count;
        }

        public override (WordsQuestModel, int)? GetCurrentQuestion()
        {
            if (questionIndex < _questions.Count && questionIndex >= 0)
                return (_questions[questionIndex], questionIndex);
            else
                return null;
        }

        public override float GetTestTime()
        {
            // TODO: Change time
            return 10f;
        }
    }
}
