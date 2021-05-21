using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class RecordPlayer : MonoBehaviour
{
    [Header("UI elements")]
    public TextMeshProUGUI timeTMP;
    public GameObject playButton;
    public GameObject playerScreen;
    public GameObject errorText;
    public GameObject execButton;
    public RawImage rawImage;
    public Image background;

    [Header("Video settings")]
    public int resolutionWidth;
    public int resolutionHeight;
    public VideoPlayer videoPlayer;

    private void OnShoqQuest(string _filePath = "")
    {
        var filePath = Path.Combine(Application.persistentDataPath, "NG Quest", "quest_1.mp4");
        videoPlayer.url = filePath;
        rawImage.texture = videoPlayer.targetTexture;


        float ratio;
        float texWidth = rawImage.mainTexture.width;
        float texHeight = rawImage.mainTexture.height;
        ratio = (texWidth > texHeight)? 
                texWidth / texHeight:
                texHeight / texWidth;

        var fitter = videoPlayer.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = ratio;
        rawImage.GetComponent<AspectRatioFitter>().aspectRatio = ratio;

        videoPlayer.loopPointReached += source => {
            OnPlayStop();
            execButton.SetActive(true);
        };
    }

    public void OnPlayClick()
    {
        playButton.SetActive(false);
        videoPlayer.Play();
    }

    public void OnPlayStop()
    {
        playButton.SetActive(true);
    }
}
