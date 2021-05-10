using NatSuite.Devices;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace Maps.Video
{
    public class VideoRecorder : MonoBehaviour
    {
        public TextMeshProUGUI _errorText;
        public static bool cameraIsActive = false;

        public bool Recording
        {
            get { return recording; }
            set
            {
                if (value)
                {
                    Record();
                }
                recording = value;
            }
        }
        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = Path.Combine(Application.persistentDataPath, value);
                if (System.IO.File.Exists(_filePath))
                {
                    System.IO.File.Delete(_filePath);
                }
            }
        }

        private bool recording;
        public RawImage rawImage;
        public delegate void OnFileSave(string path);
        public event OnFileSave FileSaved;

        public ICameraDevice display;

        public async void Display()
        {
            var criterion = MediaDeviceQuery.Criteria.FrontFacing;
            var query = new MediaDeviceQuery(criterion);
            if (query.currentDevice != null)
            {
                cameraIsActive = true;
                display = query.currentDevice as ICameraDevice;
                display.previewResolution = (1280, 720);
                try
                {
                    var previewTexture = await display.StartRunning();
                    rawImage.texture = previewTexture;
                }
                catch (System.Exception ex)
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
            device.previewResolution = (1280, 720);
            var previewTexture = await device.StartRunning();
            rawImage.texture = previewTexture;
            var recorder = new MP4Recorder(previewTexture.width, previewTexture.height, 15, adevice.sampleRate, adevice.channelCount, 2250000, 3);
            var clock = new RealtimeClock();

            adevice.StartRunning((sampleBuffer, timestamp) =>
            {
                recorder.CommitSamples(sampleBuffer, clock.timestamp);
            });
            while (recording)
            {
                recorder.CommitFrame(previewTexture.GetPixels32(), clock.timestamp);
                await Task.Delay(20);
            }
            if (adevice.running)
            {
                adevice.StopRunning();
            }
            if (device.running)
            {
                device.StopRunning();
            }
            var path = await recorder.FinishWriting();
            System.IO.File.Move(path, FilePath);
            FileSaved(FilePath);
        }
    }
}