using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoRecorderController : MonoBehaviour
{
    public TextMeshProUGUI timeTMP;
    
    public GameObject recordButton;
    public GameObject playButton;
    public GameObject deleteButton;
    public GameObject sendButton;
    public GameObject playerScreen;
    
    public Texture2D recordButtonState;
    public Texture2D stopRecordButtonState;

    private bool _isRecord;
    private bool _isPlay;

    private void OnEnable()
    {
        _isRecord = false;
        _isPlay = false;
        recordButton.SetActive(true);
        playButton.SetActive(false);
        deleteButton.SetActive(false);
        sendButton.SetActive(false);

        var buttonImg = recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, recordButtonState);
    }

    public void OnRecordButtonClick()
    {
        var buttonImg = recordButton.GetComponent<Image>();
        if (_isRecord)
        {
            // Record stop
            LoadedImage.SetTextureToImage(ref buttonImg, recordButtonState);
            recordButton.SetActive(false);
            playButton.SetActive(true);
            deleteButton.SetActive(true);
            sendButton.SetActive(true);
        }
        else
        {
            // Record start
            LoadedImage.SetTextureToImage(ref buttonImg, stopRecordButtonState);
            recordButton.SetActive(true);
            playButton.SetActive(false);
            deleteButton.SetActive(false);
            sendButton.SetActive(false);
        }
        _isRecord = !_isRecord;
    }

    public void OnPlayButtonClick()
    {
        if (_isPlay)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
        _isPlay = !_isPlay;
    }

    public void OnDeleteButtonClick()
    {
        recordButton.SetActive(true);
        playButton.SetActive(false);
        deleteButton.SetActive(false);
        sendButton.SetActive(false);
    }

    private void Update()
    {
        // TODO: Use timer 
    }
}
