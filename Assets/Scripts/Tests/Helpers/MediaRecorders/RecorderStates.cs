using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using NatSuite.Devices;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;

public abstract class ARecorderState : IDisposable
{
    protected VideoRecorderUIController context;
    public ARecorderState(VideoRecorderUIController _context) => context = _context;
    public abstract void Render();
    public abstract void ProcessData();
    public abstract void Dispose();
}



public class ReadyRecordState : ARecorderState
{
    private ICameraDevice display;

    public ReadyRecordState(VideoRecorderUIController _context) : base(_context) 
    {
        var bgColor = context.background.color;
        context.background.color = new Color(bgColor.r, bgColor.g, bgColor.b, 0.1f);

        context.IsRecordClicked = false; 
        context.FilePath = $"{Guid.NewGuid()}.mp4";
        context.rawImage.gameObject.SetActive(true);
        var buttonImg = context.recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, context.recordButtonState);
    }

    private async void DisplayPreview()
    {
        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        if (query.currentDevice != null)
        {
            display = query.currentDevice as ICameraDevice;
            display.previewResolution = context.Resolution;
            var aspectFitter = context.rawImage.GetComponent<AspectRatioFitter>();

            try
            {
                var previewTexture = await display.StartRunning();
                context.rawImage.texture = previewTexture;
                var tex = context.rawImage.mainTexture;
                if (tex.width > tex.height)
                    aspectFitter.aspectRatio = (float)tex.width / tex.height;
                else
                    aspectFitter.aspectRatio = (float)tex.height / tex.width;
            }
            catch (Exception ex)
            {
                context.errorText.gameObject.SetActive(true);
                Debug.Log($"{ex.Message}");
            }
        }
        else
        {
            display.StopRunning();
        }
    }

    public override void Render()
    {
        context.recordButton.SetActive(true);
        context.playButton.SetActive(false);
        context.deleteButton.SetActive(false);
        context.sendButton.SetActive(false);
        context.rawImage.gameObject.SetActive(true);
        var buttonImg = context.recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, context.recordButtonState);
        DisplayPreview();
    }

    public override void ProcessData()
    {

    }

    public override void Dispose()
    {
        display.StopRunning();
    }
}



public class RecordState : ARecorderState
{
    public RecordState(VideoRecorderUIController _context) : base(_context) { }
    private bool isRecording;

    public override void Render()
    {
        isRecording = false;
        context.recordButton.SetActive(true);
        context.playButton.SetActive(false);
        context.deleteButton.SetActive(false);
        context.sendButton.SetActive(false);
        var buttonImg = context.recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, context.stopRecordButtonState);
    }

    public override async void ProcessData()
    {
        isRecording = true;

        var acriterion = MediaDeviceQuery.Criteria.AudioDevice;
        var aquery = new MediaDeviceQuery(acriterion);
        var adevice = aquery.currentDevice as AudioDevice;

        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        var device = query.currentDevice as ICameraDevice;
        device.previewResolution = context.Resolution;

        context.StartCoroutine(WaitUntilRecordFinish());
        Debug.Log("Before device running");
        //if (device != null && device.running)
        //    device.StopRunning();
        var previewTexture = await device.StartRunning(); // Не стартует при повторном запуске
        Debug.Log("After device running");
        context.rawImage.texture = previewTexture;
        var recorder = new MP4Recorder(previewTexture.width, previewTexture.height, 15, adevice.sampleRate, adevice.channelCount, 2_250_000, 3);
        var clock = new RealtimeClock();
        adevice.StartRunning(
            (sampleBuffer, timestamp) => recorder.CommitSamples(sampleBuffer, clock.timestamp)
        );
        Debug.Log($"Thread ID before clicked: {Thread.CurrentThread.ManagedThreadId}");
        while (context.IsRecordClicked)
        {
            recorder.CommitFrame(previewTexture.GetPixels32(), clock.timestamp);
            await Task.Delay(20);
        }
        if (adevice.running) adevice.StopRunning();
        if (device.running) device.StopRunning();

        await recorder.FinishWriting().ContinueWith(
            (_path) => {
                context.SaveFile(_path.Result);
                isRecording = false;
            }
        );
    }

    IEnumerator WaitUntilRecordFinish()
    {
        while (isRecording)
            yield return new WaitForSeconds(0.1f);
        context.ChangeState(new PlayRecord(context));
    }

    public override void Dispose()
    {
        isRecording = false;
    }
}



public class PlayRecord : ARecorderState
{
    private ICameraDevice display;

    public PlayRecord(VideoRecorderUIController _context) : base(_context) { }

    public override void Render()
    {
        Debug.Log($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        context.playButton.SetActive(true);
        context.recordButton.SetActive(false);
        context.deleteButton.SetActive(true);
        DisplayPreview();
    }

    private async void DisplayPreview()
    {
        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        if (query.currentDevice != null)
        {
            display = query.currentDevice as ICameraDevice;
            display.previewResolution = context.Resolution;
            var aspectFitter = context.rawImage.GetComponent<AspectRatioFitter>();

            try
            {
                var previewTexture = await display.StartRunning();
                context.rawImage.texture = previewTexture;
                var tex = context.rawImage.mainTexture;
                if (tex.width > tex.height)
                    aspectFitter.aspectRatio = (float)tex.width / tex.height;
                else
                    aspectFitter.aspectRatio = (float)tex.height / tex.width;
            }
            catch (Exception ex)
            {
                context.errorText.gameObject.SetActive(true);
                Debug.Log($"{ex.Message}");
            }
        }
        else
        {
            if (display.running)
                display.StopRunning();
        }
    }

    public override void ProcessData()
    {
        display.StopRunning();
        context.playButton.SetActive(false);
        var fitter = context.videoPlayer.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = context.rawImage.GetComponent<AspectRatioFitter>().aspectRatio;

        Debug.Log("Display video from file");
        context.videoPlayer.url = context.FilePath;
        context.rawImage.texture = context.videoPlayer.targetTexture;
        context.videoPlayer.loopPointReached +=
            source => context.ChangeState(new PlayRecord(context));
        context.videoPlayer.Play();
    }

    public override void Dispose()
    {
        context.videoPlayer.Stop();
    }
}



public class DeleteRecord : ARecorderState
{
    public DeleteRecord(VideoRecorderUIController _context) : base(_context) { }

    public override void Dispose()
    {
        
    }

    public override void ProcessData()
    {
        if (System.IO.File.Exists(context.FilePath))
        {
            System.IO.File.Delete(context.FilePath);
        }
    }

    public override void Render()
    {
        context.recordButton.SetActive(true);
        context.playButton.SetActive(false);
        context.deleteButton.SetActive(false);
        context.sendButton.SetActive(false);
    }
}



public class SendRecord : ARecorderState
{
    public SendRecord(VideoRecorderUIController _context) : base(_context)
    {
    }

    public override void Dispose()
    {
        
    }

    public override void ProcessData()
    {
        
    }

    public override void Render()
    {
        
    }
}