using System;
using System.Collections;
using System.Collections.Generic;
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

    public ReadyRecordState(VideoRecorderUIController _context) : base(_context) { }

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
        context.IsRecordSave = false;
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
    private bool recording = false;

    public override void Render()
    { 
        context.recordButton.SetActive(true);
        context.playButton.SetActive(false);
        context.deleteButton.SetActive(false);
        context.sendButton.SetActive(false);
        var buttonImg = context.recordButton.GetComponent<Image>();
        LoadedImage.SetTextureToImage(ref buttonImg, context.stopRecordButtonState);
    }

    public override async void ProcessData()
    {
        recording = true;
        var acriterion = MediaDeviceQuery.Criteria.AudioDevice;
        var aquery = new MediaDeviceQuery(acriterion);
        var adevice = aquery.currentDevice as AudioDevice;

        var criterion = MediaDeviceQuery.Criteria.FrontFacing;
        var query = new MediaDeviceQuery(criterion);
        var device = query.currentDevice as ICameraDevice;
        device.previewResolution = context.Resolution;

        var previewTexture = await device.StartRunning();
        context.rawImage.texture = previewTexture;
        var recorder = new MP4Recorder(previewTexture.width, previewTexture.height, 15, adevice.sampleRate, adevice.channelCount, 2_250_000, 3);
        var clock = new RealtimeClock();

        adevice.StartRunning(
            (sampleBuffer, timestamp) =>
            recorder.CommitSamples(sampleBuffer, clock.timestamp)
        );
        while (recording)
        {
            recorder.CommitFrame(previewTexture.GetPixels32(), clock.timestamp);
            await Task.Delay(20);
        }
        if (adevice.running) adevice.StopRunning();
        if (device.running) device.StopRunning();

        await recorder.FinishWriting().ContinueWith(
            (_path) => context.SaveFile(_path.Result)
        );
    }

    public override void Dispose()
    {
        recording = false;
    }
}



public class PlayRecord : ARecorderState
{
    public PlayRecord(VideoRecorderUIController _context) : base(_context) { }

    public override void Render()
    {
        context.playButton.SetActive(true);
    }

    public override void ProcessData()
    {
        context.playButton.SetActive(false);
        var fitter = context.videoPlayer.GetComponent<AspectRatioFitter>();
        fitter.aspectRatio = context.rawImage.GetComponent<AspectRatioFitter>().aspectRatio;

        context.StartCoroutine(ShowVideoFromFile());
    }

    IEnumerator ShowVideoFromFile()
    {
        while (context.IsRecordSave == false)
            yield return null;
        Debug.Log("Display video from file");
        context.videoPlayer.url = context.FilePath;
        context.rawImage.texture = context.videoPlayer.targetTexture;
        context.videoPlayer.Play();
    }

    public override void Dispose()
    {
        context.videoPlayer.Stop();
    }
}



public class DeleteRecord : ARecorderState
{
    public DeleteRecord(VideoRecorderUIController _context) : base(_context)
    {
    }

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