using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserModel
{
    string userName;
    List<TestWholeStats> generalStats;
}

[Serializable]
public class TestWholeStats
{
    static public int pointsPerLevel = 40;
    public string testName;
    public int testLevel;
    public List<TestResultStats> testScores;

    public TestWholeStats()
    {
        testScores = new List<TestResultStats>();
    }

    public void AddNewScore(int _newScore, int _rightAnswers, int _worongAnswers)
    {
        int testScore = _newScore;
        if (testScore > pointsPerLevel * testLevel
            && testScore >= pointsPerLevel * (testLevel + 1))
        {
            testLevel++;
        }
        else 
        if ( testScore < pointsPerLevel * testLevel 
            && testLevel > 0)
        {
            testLevel--;
            if (testScore < 0) testScore = 0;
        }
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