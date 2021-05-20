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
    public RawImage rawImage;
    public Image background;

    [Header("Video settings")]
    public int resolutionWidth;
    public int resolutionHeight;
    public VideoPlayer videoPlayer;

    public string FilePath { get; set; }

    private void OnEnable()
    {
        FilePath = Path.Combine(Application.persistentDataPath, "NG Quest", "quest_1.mp4"); 
        var fitter = videoPlayer.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = rawImage.GetComponent<AspectRatioFitter>().aspectRatio;

        videoPlayer.url = FilePath;
        rawImage.texture = videoPlayer.targetTexture;
        videoPlayer.loopPointReached += source => OnPlayStop();
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
