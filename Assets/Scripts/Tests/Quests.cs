using System;

[Serializable]
public class Question : ILinksExistable
{
    public string question;
    public File file;
    public Answer[] answers;

    public bool isLinksExist()
    {
        return isQuestionImageExist || isAnswersImagesExists;
    }

    public bool isQuestionImageExist { get { return file != null && file.isLinksExist(); } }
    public bool isAnswersImagesExists
    {
        get
        {
            bool result = false;

            foreach (var ans in answers)
            {
                result = result || (ans.file != null && ans.file.isLinksExist());
            }
            return result;
        }
    }
}

[Serializable]
public class Answer
{
    public string content;
    public File file;
    public bool isRight;
}

[Serializable]
public class File: ILinksExistable
{
    public string link;
    public string name;

    public bool isLinksExist()
    {
        return !string.IsNullOrEmpty(link);
    }
}

interface ILinksExistable
{
    bool isLinksExist();
}
