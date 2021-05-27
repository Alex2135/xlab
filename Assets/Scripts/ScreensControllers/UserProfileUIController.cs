using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfileUIController : MonoBehaviour, IScreenController
{
    public UserProfileView userProfileView;
    public string screenName;

    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    private ScreensUIController _controller;

    public void OnEnable()
    {
        _controller = ScreensUIController.GetInstance();
    }
}
