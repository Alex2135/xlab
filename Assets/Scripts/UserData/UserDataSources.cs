using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public interface IUserDataSource
{
    void SaveUserModel(UserData _user);
    UserData LoadUserModel();
}

public class FileUserDataSource : IUserDataSource
{
    private string path;

    public FileUserDataSource(string _fileName)
    {
        path = Path.Combine(Application.persistentDataPath, _fileName);
    }

    public UserData LoadUserModel()
    {
        UserData result = null;
        if ( System.IO.File.Exists(path) )
        {
            string rawFileData = System.IO.File.ReadAllText(path);
            result = JsonConvert.DeserializeObject<UserData>(rawFileData);
        }

        return result;
    }

    public void SaveUserModel(UserData _user)
    {
        JsonSerializer serializer = new JsonSerializer();

        using (StreamWriter sw = new StreamWriter(path))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, _user);
        }

    }
}