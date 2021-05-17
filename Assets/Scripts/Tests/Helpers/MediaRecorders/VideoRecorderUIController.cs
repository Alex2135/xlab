using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Events;

public interface IRecorder
{
    bool isRecordClicked { get; set; }
    string FilePath { get; set; }
}

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


    private ARecorderState state;
    public bool IsRecordClicked { get; set; }

    private void OnEnable()
    {
        ChangeState(new ReadyRecordState(this));
    }

    private void StateRender() => state.Render();
    private void StateProcessData() => state.ProcessData();
    private void StateDispose() => state.Dispose();

    public void OnRecordClick()
    {
        if (!IsRecordClicked)
        {
            Debug.Log("Start record");
            IsRecordClicked = true;
            ChangeState(new RecordState(this));
            StateProcessData();
        }
        else
        {
            Debug.Log("Stop record");
            IsRecordClicked = false;
        }
    }

    public void OnPlayClick()
    {
        StateProcessData();
    }

    public void OnDeleteClick()
    {
        ChangeState(new DeleteRecord(this));
        StateProcessData();
        ChangeState(new ReadyRecordState(this));
    }

    public void SaveFile(string _path)
    {
        System.IO.File.Move(_path, FilePath);
        Debug.Log($"File was saved {FilePath}");
    }

    public void ChangeState(ARecorderState _newState)
    {
        if (state != null) StateDispose();
        state = _newState;
        if (state != null) StateRender();
    }

    private void Update()
    {
        // TODO: Use timer 
    }
}