using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfileView : MonoBehaviour, IScreenController
{
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    public void OnEnable()
    {
        
    }
}
