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
        int GradQuest(TA _userAnswers);
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
     * Создать приложение для тестов, у него будет разные
     * источники данных, как внешние файлы, сгенерированные
     * данные, так и данные полученные из сети. Эти данные
     * будут вопросами тестов. Тесты должны отображаться 
     * на экране, при этом у каждого теста должно быть свое 
     * представление на экране. В тестах может содержаться
     * как один, так и множество вопросов. За каждый вопрос
     * теста назначается одинаковое кол-во очков. Некоторые
     * тесты должны быть на время. При этом итоговое кол-во 
     * очков может зависить от времени за которое тест был
     * пройден.
     */

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