using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NewQuestionModel
{

    /// <summary>
    /// IGenericQuestModel - generic interface for one question
    /// </summary>
    /// <typeparam name="QuestType">Question type</typeparam>
    /// <typeparam name="AnswerType">Answers type</typeparam>
    interface IGenericQuestModel<QuestType, AnswerType>
        where QuestType : class, IEnumerable // IEnumerable for quest collections, class for null result
        where AnswerType : class, IEnumerable
    {
        // One question may be dictionary or list type.
        // Same with answers.
        QuestType Quest { get; set; } 
        AnswerType RightAnswers { get; set; }
        AnswerType AdditionalAnswers { get; set; }
    }

    /// <summary>
    /// IAdaptedQuestModel - interface provide model data quest to presenters
    /// </summary>
    /// <typeparam name="QuestType"></typeparam>
    /// <typeparam name="AnswerType"></typeparam>
    interface IAdaptedQuestModel<QuestType, AnswerType>
        where QuestType: class
        where AnswerType: class
    {
        Dictionary<int, QuestType> Quest { get; set; }
        Dictionary<int, AnswerType> RightAnswers { get; set; }
        Dictionary<int, AnswerType> AdditionalAnswers { get; set; }
    }

    /// <summary>
    /// IAdaptedQuestView - interface provide IAdaptedQuestModel to view
    /// </summary>
    interface IAdaptedQuestToView
    {
        Dictionary<int, GameObject> Quest { get; set; }
        Dictionary<int, GameObject> RightAnswers { get; set; }
        Dictionary<int, GameObject> AdditionalAnswers { get; set; }
    }

    /// <summary>
    /// IDataSource - generic model for data sources, prefer json object
    /// </summary>
    /// <typeparam name="QuestModel">Child from IGenericQuestModel</typeparam>
    interface IDataSource<QuestModel> 
    {
        IEnumerable<QuestModel> GetQuests();
    }

    /// <summary>
    /// ATestModel - model for every test 
    /// </summary>
    /// <typeparam name="QuestModel">Child from IGenericQuestModel</typeparam>
    abstract class ATestModel<QuestModel>
    {
        protected int rightQuestions;
        protected int wrongQuestions;
        protected int questionIndex;
        protected IDataSource<QuestModel> _dataSource;

        public abstract void RewardRightAnswer();
        public abstract void PenaltieWrongAnswer();
        public abstract (QuestModel, int) GetCurrentQuestion();
        public abstract int GetScore();
    }

    interface ITestPresenter<QuestForView>
    {
        QuestForView GetAdaptedQuests();
        int view_OnAnswering(int _userAnswer);
        void view_OnAnswerDid();
    }

    /*
     * События происходящие во всех TestPresenter:
     * - перевод модели данных вопросов в промежуточную модель;
     * - отправка данных из модели к представлению;
     * - проеврять ответы пользователя;
     */
    abstract class ATestPresenter<QuestModel, AdaptedQuestModel>
    {
        protected ATestModel<QuestModel> testModel;
        protected ITestView testQuestionsView;

        protected abstract void GenerateAnswersId();
        protected abstract List<T> GetAdaptedQuestsForView<T>();
        public abstract int view_OnAnswering(int _userAnswer); // Получаем идентификаторы ответов пользователя
        public abstract Dictionary<int, AdaptedQuestModel> AdaptedQuestionData { get; set; }
    }

    /*
     * События происходящие во всех TestView:
     * - Отображение вопросов на основе View;
     * - Ответ пользователя на задание;
     * - Отображение реакции на ответ пользователя;
     * - Сброс отображения View.
     */
    interface ITestView
    {
        void ShowQuestion();
        void ShowQuestResult();
        void ResetView();

        event Action<int> OnAnswering;
        event Action OnAnswerDid;
    }
}