using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

class DownloadStrategy : IImagesDownloader
{
    private IImagesDownloader imageDownloader;
    public event Action<object> onLoadImagesBegin;
    public event Action<List<LoadedImage>> onLoadImageEnd;
    public List<LoadedImage> downloadedImages { get; private set; }

    public void SetImageDownloader(IImagesDownloader _imDldr)
    {
        imageDownloader = _imDldr;
    }

    public async Task<List<LoadedImage>> DownloadQuestImagesAsync(object obj)
    {
        onLoadImagesBegin?.Invoke(obj);
        var test = (Test)obj ?? throw new ArgumentNullException("Test object is null");
        var result = new List<LoadedImage>(); // List of question and answers images
        var tasks = new List<Task>();

        for (int i = 0; i < test.quests.Count; i++)
        {
            var quest = test.quests[i];
            if (quest.isLinksExist())
            {
                var taskDownloadQuestImgs = imageDownloader.DownloadQuestImagesAsync(quest).ContinueWith(
                    async (taskWithQuestImgs, idx) =>
                    {
                        var imgs = await taskWithQuestImgs;
                        Thread.Sleep(15);
                        imgs.ForEach(x => x._name += $"_{idx}");
                        result.AddRange(imgs);
                    }
                , i
                , TaskContinuationOptions.OnlyOnRanToCompletion);
                tasks.Add(await taskDownloadQuestImgs);
            }
        }
        await Task.WhenAll(tasks.ToArray());

        downloadedImages = result;
        onLoadImageEnd?.Invoke(result);
        return result;
    }
}

/*
 * Необходимо параллельно загружать изображения из всех вопросов
 * теста. Изображения вопросов и ответов. Процесс загрузки должен
 * проходить асинхронно, а в это время пользователь должен видеть
 * Loader. В конце загрузки всех изображений из тестов изображения
 * первого вопроса должны будут показаться пользователю, а Loader
 * должен исчезнуть.
 * 
 * Задачи:
 * 1. Параллельная загрузка изображений вопросов и их ответов.
 * 2. Обеспечить асинхронность параллельной загрузки изображений
 *    к работающему приложению 
 * 3. Передача сигнала работающему приложению о загрузке всех
 *    вопросов (или первого вопроса).
 * 
 */

public class LoadedImage
{
    public Texture2D _image;
    public string _name;

    public LoadedImage(Texture2D _t, string _n) { _image = _t; _name = _n; }

    public static void SetTextureToImage(ref Image _img, Texture2D _tex)
    {
        _img.sprite = Sprite.Create(_tex,
                                    new Rect(0, 0, _tex.width, _tex.height),
                                    new Vector2(0.5f, 0.5f)
                                    );
    }
}

internal class QuestionDownloader: IImagesDownloader
{
    private IImageRequest imageRequester;

    public QuestionDownloader(IImageRequest _imageDownloader)
    {
        imageRequester = _imageDownloader;
    }

    public async Task<List<LoadedImage>> DownloadQuestImagesAsync(object obj)
    {
        Question quest = (Question)obj ?? throw new ArgumentNullException("DownloadQuestImagesAsync: quest object is null");
        var result = new List<LoadedImage>();
        var tasks = new List<Task<LoadedImage>>();

        // Fetching quest image
        if (quest.file != null &&
            quest.file.isLinksExist())
        {
            var t = imageRequester.FetchImageAsync(quest.file.link, "quest");
            if (t != null) tasks.Add(t);
        }

        // Fetching answers images
        for (int i = 0; i < quest.answers.Length; i++)
        {
            var ansFile = quest.answers[i].file;
            if (ansFile != null &&
                ansFile.isLinksExist())
            {
                var t = imageRequester.FetchImageAsync(ansFile.link, $"answer_{i}");
                if (t != null) tasks.Add(t);
            }
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks.ToArray());
            foreach (var t in tasks) result.Add(t.Result);
        }
        else
        {
            throw new Exception("No tasks for fetching quest images!");
        }
        // Waiting until images was downloaded
        return result;
    }
}

class NetImageRequester : IImageRequest
{
    public async Task<LoadedImage> FetchImageAsync(string _url, string _name)
    {
        UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url);
        try
        {
            await uwr.SendWebRequest();
            Texture2D result = DownloadHandlerTexture.GetContent(uwr);
            return new LoadedImage(result, _name);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            uwr.Dispose();
        }
        return null;
    }
}

interface IImageRequest
{
    Task<LoadedImage> FetchImageAsync(string _url, string _name);
}

interface IImagesDownloader
{
    Task<List<LoadedImage>> DownloadQuestImagesAsync(object obj);
}