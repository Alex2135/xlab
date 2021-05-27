using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserProfileModel
{
    int GetUserTotalRate();
    int GetTopPercentage();
    Dictionary<string, int> GetPoints();
    Dictionary<string, int> GetSkills();
}

public class UserProfileModel : IUserProfileModel
{
    public Dictionary<string, int> GetPoints()
    {
        return null;
    }

    public Dictionary<string, int> GetSkills()
    {
        return null;
    }

    public int GetTopPercentage()
    {
        return 0;
    }

    public int GetUserTotalRate()
    {
        return 0;
    }
}
