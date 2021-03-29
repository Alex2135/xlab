using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacesTestUIController : MonoBehaviour, IScreenController, IDecorableScreen
{
    [Serializable]
    public class FacesImage : LoadedImage
    {
        public FacesImage(Texture2D _t = null, string _n = null) : base(_t, _n) { }
    }

    public Image Background;
    public string _screenName;
    public List<FacesImage> loadedImages;
    public RememberFacesTestView rememberFacesTV;
    public NameByFaceTestView nameByFaceTV;
    public FaceByNameTestView faceByNameTV;
    public ResultsUiController testResultView;
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName { get => _screenName; set => _screenName = value; }

    /*
     * В FacesTestUIController происходит загрузка данных
     * изображений названий лиц людей, которых нужно 
     * отгадывать. После этого данные должны передаваться
     * контроллерам каждого из подраздела тестов для его 
     * проведения.
     */

    void Start()
    {
        NextScreen = rememberFacesTV;
        rememberFacesTV.NextScreen = nameByFaceTV;
        faceByNameTV.NextScreen = testResultView;
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
