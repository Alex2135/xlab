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
    public List<FacesImage> imagesForFaceByName;
    public RememberFacesTestUIController rememberFacesUIC;
    public NameByFaceTestUIController nameByFaceUIC;
    public FaceByNameTestUIController faceByNameUIC;
    public ResultsUiController testResultView;
    public IScreenController NextScreen { get; set; }
    public IScreenController PrevScreen { get; set; }
    public string ScreenName { get => _screenName; set => _screenName = value; }
    private ScreensUIController _screensController;

    /*
     * В FacesTestUIController происходит загрузка данных
     * изображений названий лиц людей, которых нужно 
     * отгадывать. После этого данные должны передаваться
     * контроллерам каждого из подраздела тестов для его 
     * проведения.
     */

    void OnEnable()
    {
        NextScreen = rememberFacesUIC;
        rememberFacesUIC.NextScreen = nameByFaceUIC;
        nameByFaceUIC.NextScreen = faceByNameUIC;
        faceByNameUIC.NextScreen = testResultView;

        var castedImages = loadedImages.ConvertAll(img => img as LoadedImage);
        rememberFacesUIC.loadedImages = new List<LoadedImage>(castedImages);
        nameByFaceUIC.loadedImages = new List<LoadedImage>(castedImages);
        faceByNameUIC.loadedImages = new List<LoadedImage>(castedImages);
        var castedAdditionalImages = imagesForFaceByName.ConvertAll(img => img as LoadedImage);
        faceByNameUIC.additionalImages = new List<LoadedImage>(castedAdditionalImages);

        _screensController = ScreensUIController.GetInstance();
        _screensController.Add(rememberFacesUIC);
        _screensController.Add(nameByFaceUIC);
        _screensController.Add(faceByNameUIC);
        _screensController.Activate(rememberFacesUIC, null, false);
    }

    public void OnBackClick()
    {
        if (PrevScreen != null)
        {
            _screensController.DiactivateScreens();
            _screensController.Activate(PrevScreen);
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
