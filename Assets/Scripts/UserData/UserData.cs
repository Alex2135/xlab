using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    public string userName;
    public List<TestWholeStats> generalStats;

    public UserData()
    {
        userName = "UserName";
        generalStats = new List<TestWholeStats>();
    }

    public bool CheckEmail(string email)
    {
        return email == "h@h.h";
    }

    public bool CheckPassword(string pass)
    {
        return pass == "111";
    }
}
/*
 Получать запрос от сервера есть ли такой емаил,
 если емаил
 */


[Serializable]
public class TestWholeStats
{
    static public int pointsPerLevel = 40;
    public string testName;
    public int testLevel;
    public List<TestResultStats> testScores;

    public TestWholeStats(string _testName = "")
    {
        testName = _testName;
        testLevel = 1;
        testScores = new List<TestResultStats>();
    }

    public int GetLastScore()
    {
        // Initial scores
        int lastScore = 40;
        if (testScores.Count > 0)
            lastScore = testScores[testScores.Count - 1].testScore;
        return lastScore;
    }

    public void AddNewScore(int _newScore, int _rightAnswers, int _worongAnswers)
    {
        int lastScore = GetLastScore();
        int testScore = _newScore + lastScore;
        if (testScore < 0) testScore = 0;
        if (testScore > pointsPerLevel * testLevel && testScore >= pointsPerLevel * (testLevel + 1))
            testLevel++;
        else 
        if (testScore < pointsPerLevel * testLevel && testLevel > 1)
            testLevel--;

        TestResultStats newTry = new TestResultStats() {
            testScore = testScore,
            rightAnswers = _rightAnswers,
            wrongAnswers = _worongAnswers
        };
        testScores.Add(newTry);
    }
}

public class TestResultStats
{
    public int rightAnswers;
    public int wrongAnswers;
    public int testScore;
}