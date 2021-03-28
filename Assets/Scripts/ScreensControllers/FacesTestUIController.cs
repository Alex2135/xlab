using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacesTestUIController : MonoBehaviour, IScreenController
{
    public Image Background;
    public string _screenName;

    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName 
    { 
        get { return _screenName; }
        set { _screenName = value; }
    }

    /*
     * В FacesTestUIController происходит загрузка данных
     * изображений названий лиц людей, которых нужно 
     * отгадывать. После этого данные должны передаваться
     * контроллерам каждого из подраздела тестов для его 
     * проведения.
     */

    void Start()
    {
        var screenController = ScreensUIController.GetInstance();
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

    public Image GetBackground()
    {
        return Background;
    }
}
