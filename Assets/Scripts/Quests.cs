using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class QuestionView
{
    public DataUI _quest;
    public List<DataUI> _answers;
}

[Serializable]
public class DataUI
{
    public TextMeshProUGUI _text;
    public Image _image;
}

[Serializable]
public class Question: ILinksExistable
{
    public string question;
    public File file;
    public Answer[] answers;

    public bool isLinksExist()
    {
        bool result = file != null && file.isLinksExist();

        foreach (var ans in answers)
        {
            result = result || (ans.file != null && ans.file.isLinksExist());
        }

        return result;
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
        return this.link != null && this.link != "";
    }
}

interface ILinksExistable
{
    bool isLinksExist();
}
