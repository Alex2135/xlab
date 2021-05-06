using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class UserModel
{
    private static UserModel _instance;
    public UserData Data { get; set; }
    private IUserDataSource _dataSource;

    private UserModel(IUserDataSource _source = null) 
    {
        Data = _source?.LoadUserModel() ?? new UserData();
        _dataSource = _source;
    }

    public static UserModel GetInstance(IUserDataSource _dataSource = null)
    {
        if (_instance == null)
        {
            if (_dataSource == null) throw new ArgumentNullException("_dataSource is null");
            _instance = new UserModel(_dataSource);
        }
        return _instance;
    }

    public static int GetLastScore(string _testName)
    {
        var user = GetInstance();
        var score = user.GetTestData(_testName) ?? throw new ArgumentNullException("Invalid _testName");

        return score.GetLastScore();
    }

    public void SaveData()
    {
        _dataSource.SaveUserModel(Data);
    }

    public bool IsTestDataExists(string _testName)
    {
        if (Data.generalStats == null)
            return false;
        return Data.generalStats.Any(test => test.testName == _testName);
    }

    public TestWholeStats GetTestData(string _testName)
    {
        TestWholeStats result;
        result = Data.generalStats.Find(test => test.testName == _testName);
        return result;
    }

    public void AddTestStats(string _testName)
    {
        if (!IsTestDataExists(_testName))
            Data.generalStats.Add(new TestWholeStats(_testName));
    }

    public void AddNewScore(string _testName, int _newScore, int _rightAnswers, int _worongAnswers)
    {
        TestWholeStats selectedTestStats = null;
        foreach (var testStats in Data.generalStats)
        {
            if (testStats.testName == _testName)
            {
                selectedTestStats = testStats;
                break;
            }
        }
        if (selectedTestStats == null) throw new ArgumentException("Invalid _testName argument");
        selectedTestStats.AddNewScore(_newScore, _rightAnswers, _worongAnswers);
    }
}