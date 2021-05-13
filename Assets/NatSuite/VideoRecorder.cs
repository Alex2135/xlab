using System;
using NatSuite.Devices;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using System.IO;
using TMPro;
using NatSuite.Examples.Components;
using System.Collections;

namespace Maps.Video
{
    public class VideoRecorder : MonoBehaviour
    {
        public static bool cameraIsActive = false;
        public TextMeshProUGUI _errorText;
        public RawImage rawImage;
        public ICameraDevice display;
        public int resolutionWidth;
        public int resolutionHeight;
        public CameraPreview cameraPreview;

        public event Action<string> FileSaved;

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
        private (int, int) Resolution => (resolutionWidth, resolutionHeight);

        private void Awake()
        {
            FilePath = "1.mp4";
            FileSaved += _path => Debug.Log($"Saved path {_path}");
        }

        private AspectRatioFitter aspectFitter;
        public WebCamTexture cameraTexture { get; private set; }
        public VideoRecorder recorder;
        IEnumerator Start()
        {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
            // Request camera permission
            if (Application.platform == RuntimePlatform.Android)
            {
                if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
                {
                    Permission.RequestUserPermission(Permission.Camera);
                    yield return new WaitUntil(() => Permission.HasUserAuthorizedPermission(Permission.Camera));
                }
            }
            else
            {
                yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
                if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
                    yield break;
            }
            // Start the WebCamTexture
            cameraTexture = new WebCamTexture(null, recorder.resolutionWidth, recorder.resolutionHeight, 30);
            cameraTexture.Play();
            yield return new WaitUntil(() => cameraTexture.width != 16 && cameraTexture.height != 16); // Workaround for weird bug on macOS
            // Setup preview shader with correct orientation
            rawImage.texture = cameraTexture;
            rawImage.material.SetFloat("_Rotation", cameraTexture.videoRotationAngle * Mathf.PI / 180f);
            rawImage.material.SetFloat("_Scale", cameraTexture.videoVerticallyMirrored ? -1 : 1);
            // Scale the preview panel
            if (cameraTexture.videoRotationAngle == 90 || cameraTexture.videoRotationAngle == 270)
                aspectFitter.aspectRatio = (float)cameraTexture.height / cameraTexture.width;
            else
                aspectFitter.aspectRatio = (float)cameraTexture.width / cameraTexture.height;
        }

        public async void Display()
        {
            var criterion = MediaDeviceQuery.Criteria.FrontFacing;
            var query = new MediaDeviceQuery(criterion);
            if (query.currentDevice != null)
            {
                cameraIsActive = true;
                display = query.currentDevice as ICameraDevice;
                display.previewResolution = Resolution;
                try
                {
                    var previewTexture = await display.StartRunning();
                    rawImage.texture = previewTexture;
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

        public void StopDisplay()
        {
            if (display != null && display.running)
            {
                display.StopRunning();
                rawImage.color = Color.white;
            }
        }

        async void Record()
        {
            var acriterion = MediaDeviceQuery.Criteria.AudioDevice;
            var aquery = new MediaDeviceQuery(acriterion);
            var adevice = aquery.currentDevice as AudioDevice;

            var criterion = MediaDeviceQuery.Criteria.FrontFacing;
            var query = new MediaDeviceQuery(criterion);
            var device = query.currentDevice as ICameraDevice;
            device.previewResolution = Resolution;

            cameraPreview.cameraTexture.Stop();
            var previewTexture = await device.StartRunning();
            rawImage.texture = previewTexture;
            var recorder = new MP4Recorder(previewTexture.width, previewTexture.height, 15, adevice.sampleRate, adevice.channelCount, 2250000, 3);
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
            cameraPreview.cameraTexture.Play();

            var path = await recorder.FinishWriting();
            System.IO.File.Move(path, FilePath);
            FileSaved?.Invoke(FilePath);
        }
    }
}