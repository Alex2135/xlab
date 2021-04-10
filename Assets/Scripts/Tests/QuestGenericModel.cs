using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewQuestionModel
{
    /// <summary>
    /// IGenericQuestModel - generic interface for one question
    /// </summary>
    /// <typeparam name="TQ">Question type</typeparam>
    /// <typeparam name="TA">Answers type</typeparam>
    interface IGenericQuestModel<TQ, TA>
        where TQ : class, IEnumerable // IEnumerable for quest collections, class for null result
        where TA : class, IEnumerable
    {
        // One question may be dictionary or list type.
        // Same with answers.
        TQ Quest { get; set; } 
        TA RightAnswers { get; set; }
        TA AdditionalAnswers { get; set; }
    }

    /// <summary>
    /// IDataSource - generic model for data sources, prefer json object
    /// </summary>
    /// <typeparam name="TQ"></typeparam>
    /// <typeparam name="TA"></typeparam>
    interface IDataSource<QuestModel> 
    {
        IEnumerable<QuestModel> GetQuests();
    }

    interface ITestModel<QuestModel> : IDataSource<QuestModel>
    {
        IDataSource<QuestModel> DataSource { set; }
        (QuestModel, int) GetCurrentQuestion();

    }


    /*
     * События происходящие во всех TestView:
     * - Отображение вопросов на основе View;
     * - Ответ пользователя на задание;
     * - Отображение реакции на ответ пользователя;
     * - Сброс отображения View.
     */
    interface ITestQuestionsView
    {
        void ShowQuestion();
        void OnAnswerClick(object answerData);
        void ShowQuestResult();// int selectedAnswerId, int rightAnswerId);
        void UpdateTestData(); // Обновление результатов пользователя после ответов на вопрос
        void ResetViewState();
        void DisableInput();
        void EnableInput();
    }


    /*
     * События происходящие во всех TestPresenter:
     * 
     */
    interface ITestPresenter
    {

    }


    // Тест слова

    class WordsQuestModel : IGenericQuestModel<List<string>, List<string>>
    {
        public List<string> Quest { get => null; set { } } // no question in word test
        public List<string> RightAnswers { get; set; }
        public List<string> AdditionalAnswers { get; set; }
    }

    class WordsTestDataSource : IDataSource<WordsQuestModel>
    {
        IEnumerable<WordsQuestModel> IDataSource<WordsQuestModel>.GetQuests()
        {
            // Delete after words quest data model getter
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
            // Delete after words quest data model getter

            return result;
        }
    }

    class WordsTestModel : ITestModel<WordsQuestModel>
    {
        private IDataSource<WordsQuestModel> _dataSource;
        public IDataSource<WordsQuestModel> DataSource { set => _dataSource = value; }

        WordsTestModel(IDataSource<WordsQuestModel> _dataSource)
        {
            DataSource = _dataSource;
        }

        public IEnumerable<WordsQuestModel> GetQuests()
        {
            return _dataSource.GetQuests();
        }

        public (WordsQuestModel, int) GetCurrentQuestion()
        {
            WordsQuestModel wqm = new WordsQuestModel();
            int idx = 0;

            return (wqm, idx);
        }
    }

}