using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using UnityEngine.Networking;

class DownloadStrategy : IImageDownloader
{
    private IImageDownloader imageDownloader;
    public event Action<object> onLoadImageBegin;
    public event Action<List<NetImage>> onLoadImageEnd;

    public void SetImageDownloader(IImageDownloader _imDldr)
    {
        imageDownloader = _imDldr;
    }

    public async Task<List<NetImage>> DownloadImagesAsync(object obj)
    {
        onLoadImageBegin?.Invoke(obj);
        Test test = (Test)obj ?? throw new ArgumentNullException("Test object is null");
        // List of question and answers images
        List<NetImage> result = new List<NetImage>();
        int i = 0;
        foreach (var quest in test.quests)
        {
            if (quest.isLinksExist())
            {
                Debug.Log($"Download images {i} quest");
                var questionImages = await imageDownloader.DownloadImagesAsync(quest);
                questionImages.ForEach(x => x._name += $"_{i}");
                result.AddRange(questionImages);
            }
            i++;
        }

        onLoadImageEnd?.Invoke(result);
        return result;
    }
}

/*
 * 
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

class NetImage
{
    public Texture2D _image;
    public string _name;

    public NetImage(Texture2D _t, string _n) { _image = _t; _name = _n; }

    public static void SetTextureToImage(ref Image _img, Texture2D _tex)
    {
        _img.sprite = Sprite.Create(_tex,
                                    new Rect(0, 0, _tex.width, _tex.height),
                                    new Vector2(0.5f, 0.5f)
                                    );
    }
}

internal class QuestionDownloader: IImageDownloader
{
    async Task<NetImage> FetchImageAsync(string _url, string _name)
    {
        Texture2D result = new Texture2D(1000, 1000);
        var www = await new WWW(_url);
        www.LoadImageIntoTexture(result);
        return new NetImage(result, _name);
    }

    public async Task<List<NetImage>> DownloadImagesAsync(object obj)
    {
        Question quest = (Question)obj ?? throw new ArgumentNullException("DownloadImagesAsync: quest object is null");
        var result = new List<NetImage>();
        var tasks = new List<Task<NetImage>>();
        if (quest.file != null &&
            quest.file.isLinksExist())
        {
            tasks.Add(FetchImageAsync(quest.file.link, "quest"));
        }

        for (int i = 0; i < quest.answers.Length; i++)
        {
            var ansFile = quest.answers[i].file;
            if (ansFile != null && 
                ansFile.isLinksExist())
            {
                Debug.Log($"Answer №{i} image link:\n{ansFile.link}");
                var t = FetchImageAsync(ansFile.link, $"answer_{i}");
                tasks.Add(t);
            }
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks.ToArray());
            foreach (var t in tasks) result.Add(await t);
        }

        // Waiting until images was downloaded
        return result;
    }
}

interface IImageDownloader
{
    Task<List<NetImage>> DownloadImagesAsync(object obj);
}