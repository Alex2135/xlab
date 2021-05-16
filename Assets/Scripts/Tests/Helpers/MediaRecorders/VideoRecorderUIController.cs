using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Events;

public class VideoRecorderUIController: MonoBehaviour
{
    [Header("UI elements")]
    public TextMeshProUGUI timeTMP;
    public GameObject recordButton;
    public GameObject playButton;
    public GameObject deleteButton;
    public GameObject sendButton;
    public GameObject playerScreen;
    public GameObject errorText;
    public RawImage rawImage;
    public Image background;
    
    [Header("Button states textures")]
    public Texture2D recordButtonState;
    public Texture2D stopRecordButtonState;

    [Header("Video settings")]
    public int resolutionWidth;
    public int resolutionHeight;
    public VideoPlayer videoPlayer;
    public (int, int) Resolution => (resolutionWidth, resolutionHeight);

    private ARecorderState state;

    private string _filePath;
    public string FilePath
    {
        get => _filePath;
        set
        {
            _filePath = Path.Combine(Application.persistentDataPath, value);
            if (System.IO.File.Exists(_filePath))
                System.IO.File.Delete(_filePath);
        }
    }

    public bool IsRecordSave { get; set; } = false;

    public string GetRecordPath => _videoFilePath;
    private string _videoFilePath = "";

    private void OnEnable()
    {
        var bgColor = background.color;
        background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.1f);
        rawImage.gameObject.SetActive(true);

        var buttonImg = recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, recordButtonState);
        VideoRecorder.FileSaved += path => _videoFilePath = path;
    }

    public void OnRecordClick()
    {

    }

    public void OnPlayClick()
    {

    }

    public void OnDeleteClick()
    {

    }

    public void ChangeState(ARecorderState _newState)
    {
        state = _newState;
        state.Render();
    }

    private void Update()
    {
        // TODO: Use timer 
    }
}