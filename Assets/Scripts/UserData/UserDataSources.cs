using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public interface IUserDataSource
{
    void SaveUserModel(UserModel _user);
    UserModel LoadUserModel();
}

public class FileUserDataSource : IUserDataSource
{
    private string path;

    public FileUserDataSource(string _fileName)
    {
        path = Path.Combine(Application.persistentDataPath, _fileName);
    }

    public UserModel LoadUserModel()
    {
        UserModel result = null;
        if ( System.IO.File.Exists(path) )
        {
            string rawFileData = System.IO.File.ReadAllText(path);
            result = JsonConvert.DeserializeObject<UserModel>(rawFileData);
        }

        return result;
    }

    public void SaveUserModel(UserModel _user)
    {
        JsonSerializer serializer = new JsonSerializer();

        using (StreamWriter sw = new StreamWriter(path))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, _user);
        }

    }
}