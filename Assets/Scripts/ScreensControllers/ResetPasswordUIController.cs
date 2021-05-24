using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPasswordUIController : MonoBehaviour, IScreenController
{
    public TestsScreensUIController testScreen;
    public string screenName;
    public string ScreenName { get => screenName; set => screenName = value; }
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }

    void OnEnable()
    {

    }
}
