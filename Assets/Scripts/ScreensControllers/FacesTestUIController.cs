using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacesTestUIController : MonoBehaviour, IScreenController
{
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string _screenName;
    public string ScreenName 
    { 
        get { return _screenName; }
        set { _screenName = value; }
    }

    void Start()
    {

    }

    public object GetResult()
    {
        return null;
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            var screensController = ScreensUIController.GetInstance();
            screensController.DiactivateScreens();
            screensController.Activate(PrevScreen);
        }
        else
        {
            Debug.Log("Prev screen not set!");
        }
    }
}
