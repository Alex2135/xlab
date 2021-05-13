using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public RawImage rawImage;
    public Image background;
    
    [Header("Button states textures")]
    public Texture2D recordButtonState;
    public Texture2D stopRecordButtonState;

    [Header("Recording events")]
    public UnityEvent OnRecordStart;
    public UnityEvent OnRecordStop;

    [Header("Play events")]
    public UnityEvent OnPlayStart;
    public UnityEvent OnPlayStop;

    private bool _isRecord;
    private bool IsRecord
    {
        get => _isRecord;
        set => _isRecord = value;
    }

    private bool _isPlay;
    private bool IsPlay 
    {
        get => _isPlay;
        set
        {
            _isPlay = value;
            if (_isPlay) OnPlayStart?.Invoke();
            else OnPlayStop?.Invoke();
        }
    }

    private void OnEnable()
    {
        IsRecord = false;
        IsPlay = false;
        recordButton.SetActive(true);
        playButton.SetActive(false);
        deleteButton.SetActive(false);
        sendButton.SetActive(false);
        var bgColor = background.color;
        background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.1f);
        rawImage.gameObject.SetActive(true);

        var buttonImg = recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, recordButtonState);
    }

    public void OnRecordButtonClick()
    {
        var buttonImg = recordButton.GetComponent<Image>();
        if (IsRecord)
        {
            // Record stop
            OnRecordStop?.Invoke();
            LoadedImage.SetTextureToImage(ref buttonImg, recordButtonState);
            recordButton.SetActive(false);
            playButton.SetActive(true);
            deleteButton.SetActive(true);
            sendButton.SetActive(true);
            //var bgColor = background.color;
            //background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f);
        }
        else
        {
            // Record start
            OnRecordStart?.Invoke();
            LoadedImage.SetTextureToImage(ref buttonImg, stopRecordButtonState);
            recordButton.SetActive(true);
            playButton.SetActive(false);
            deleteButton.SetActive(false);
            sendButton.SetActive(false);
        }
        IsRecord = !IsRecord;
        IsPlay = false;
    }

    public void OnPlayButtonClick()
    {
        IsPlay = true;
        if (IsPlay)
        {
            playButton.SetActive(false);
            OnPlayStart?.Invoke();
        }
        else
        {
            playButton.SetActive(true);
        }
    }

    public void OnDeleteButtonClick()
    {
        OnPlayStop?.Invoke();
        recordButton.SetActive(true);
        playButton.SetActive(false);
        deleteButton.SetActive(false);
        sendButton.SetActive(false);
        IsPlay = false;
    }

    private void Update()
    {
        // TODO: Use timer 
    }
}
