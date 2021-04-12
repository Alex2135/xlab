using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NewQuestionModel
{
    // Question model for Test raw data
    class WordsQuestModel : IGenericQuestModel<List<string>, List<string>>
    {
        public List<string> Quest { get => null; set { } } // no question in word test
        public List<string> RightAnswers { get; set; }
        public List<string> AdditionalAnswers { get; set; }
    }

    // Question model for test presenter
    class WordsAdaptedQuestModel : IAdaptedQuestModel<List<string>, List<string>>
    {
        public Dictionary<int, List<string>> Quest { get => null; set { } }
        public Dictionary<int, List<string>> RightAnswers { get; set; }
        public Dictionary<int, List<string>> AdditionalAnswers { get; set; }
    }

    // Question model for test view
    class WordsQuestView : IAdaptedQuestToView
    {
        public Dictionary<int, GameObject> Quest { get => null; set { } }
        public Dictionary<int, GameObject> RightAnswers { get; set; }
        public Dictionary<int, GameObject> AdditionalAnswers { get; set; }
    }

    // 
    class WordsTestGeneratedDataSource : IDataSource<WordsQuestModel>
    {
        IEnumerable<WordsQuestModel> IDataSource<WordsQuestModel>.GetQuests()
        {
            var result = new List<WordsQuestModel>();
            var quest1 = new WordsQuestModel();
            quest1.RightAnswers = new List<string>()
            {
                "Вор", "Клубника", "Шишка", "Спам"
            };
            quest1.AdditionalAnswers = new List<string>()
            {
                "Дубина", "Лысый", "Волк", "Манежь"
            };
            result.Add(quest1);

            return result;
        }
    }

    class WordsTestModel : ATestModel<WordsQuestModel>
    {
        private IDataSource<WordsQuestModel> _dataSource;
        private List<WordsQuestModel> _questions;
        public int QuestionCount { get => _questions.Count; }
        public int PointsPerQuest { get; set; }
        public IDataSource<WordsQuestModel> DataSource { set => _dataSource = value; }

        WordsTestModel(IDataSource<WordsQuestModel> _dataSource)
        {
            DataSource = _dataSource;
            _questions = _dataSource.GetQuests() as List<WordsQuestModel>;
            questionIndex = -1;
            rightQuestions = 0;
            wrongQuestions = 0;
            PointsPerQuest = 10;
        }

        public override (WordsQuestModel, int) GetCurrentQuestion()
        {
            WordsQuestModel wqm = new WordsQuestModel();
            return (wqm, questionIndex);
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
    }
}
