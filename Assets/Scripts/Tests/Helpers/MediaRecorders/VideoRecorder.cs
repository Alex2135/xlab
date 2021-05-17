using System;
using System.IO;
using System.Collections;
using System.Threading.Tasks;

using NatSuite.Devices;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Android;
using TMPro;

public class VideoRecorder : MonoBehaviour
{
    public static bool cameraIsActive = false;
    public TextMeshProUGUI _errorText;
    public RawImage rawImage;
    public ICameraDevice display;
    public VideoPlayer videoPlayer;

    public static event Action<string> FileSaved;

    private bool recording;
    public bool Recording
    {
        get => recording;
        set
        {
            recording = value;
            if (recording) Record();
        }
    }

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

    public int resolutionWidth;
    public int resolutionHeight;
    private (int, int) Resolution => (resolutionWidth, resolutionHeight);

    private bool isRecordSave;

    private void Awake()
    {
        //Debug.Log("Random hash: " + Guid.NewGuid().ToString());
        FilePath = "1.mp4";
        FileSaved += _path => Debug.Log($"Saved path {_path}");
    }

    void Start()
    {
        DisplayPreview();
    }

    public async void StartReplay()
    {
        //StopDisplayPreview();
        var fitter = videoPlayer.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = rawImage.GetComponent<AspectRatioFitter>().aspectRatio;

        StartCoroutine(ShowVideoFromFile());
    }

    IEnumerator ShowVideoFromFile()
    {
        while (isRecordSave == false) 
            yield return null;
        Debug.Log("Display video from file");
        videoPlayer.url = FilePath;
        rawImage.texture = videoPlayer.targetTexture;

        videoPlayer.Play();
    }

    public void StopReplay()
    {
        videoPlayer.Stop();
        // DisplayPreview(); // Some wierd bug with save video and replay
    }

    public async void DisplayPreview()
    {
        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        if (query.currentDevice != null)
        {
            cameraIsActive = true;
            display = query.currentDevice as ICameraDevice;
            display.previewResolution = Resolution;
            var aspectFitter = rawImage.GetComponent<AspectRatioFitter>();

            try
            {
                var previewTexture = await display.StartRunning();
                rawImage.texture = previewTexture;
                if (rawImage.mainTexture.width > rawImage.mainTexture.height)
                    aspectFitter.aspectRatio = (float)rawImage.mainTexture.width / rawImage.mainTexture.height;
                else
                    aspectFitter.aspectRatio = (float)rawImage.mainTexture.height / rawImage.mainTexture.width;
            }
            catch (Exception ex)
            {
                _errorText.gameObject.SetActive(true);
                Debug.Log($"{ex.Message}");
            }
        }
        else
        {
            cameraIsActive = false;
        }
    }

    public void StopDisplayPreview()
    {
        if (display != null && display.running)
        {
            display.StopRunning();
            rawImage.color = Color.white;
        }
    }

    async void Record()
    {
        StopDisplayPreview();
        var acriterion = MediaDeviceQuery.Criteria.AudioDevice;
        var aquery = new MediaDeviceQuery(acriterion);
        var adevice = aquery.currentDevice as AudioDevice;

        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        var device = query.currentDevice as ICameraDevice;
        device.previewResolution = Resolution;

        var previewTexture = await device.StartRunning();
        rawImage.texture = previewTexture;
        var recorder = new MP4Recorder(previewTexture.width, previewTexture.height, 15, adevice.sampleRate, adevice.channelCount, 2_250_000, 3);
        var clock = new RealtimeClock();

        adevice.StartRunning((sampleBuffer, timestamp) =>
            recorder.CommitSamples(sampleBuffer, clock.timestamp)
        );
        while (recording)
        {
            recorder.CommitFrame(previewTexture.GetPixels32(), clock.timestamp);
            await Task.Delay(20);
        }

        if (adevice.running) adevice.StopRunning();
        if (device.running) device.StopRunning();

        isRecordSave = false;
        await recorder.FinishWriting().ContinueWith(
            (_path, _state) =>
            {
                Debug.Log("File was saved!");
                string path = _path.Result;
                isRecordSave = true;
                System.IO.File.Move(path, FilePath);
                FileSaved?.Invoke(FilePath);
            }, 
        true);
    }
}