using System.Collections.Generic;


namespace NewQuestionModel
{
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
            rightAnswers = 0;
            wrongAnswers = 0;
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
            rightAnswers++;
        }

        public override void PenaltieWrongAnswer()
        {
            wrongAnswers++;
            if (wrongAnswers > 4) wrongAnswers = 4;
        }

        public override int GetScore()
        {
            int maxScore = _questions.Count * PointsPerQuest;
            int result = rightAnswers * PointsPerQuest - wrongAnswers * 1/4 * maxScore;
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
