using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    /// <summary>
    /// Save data to file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);
        //PlayerPrefs.SetString(typeof(T).ToString(), json);
        File.WriteAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json", json);
    }

    /// <summary>
    /// Load data from file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadData<T>()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
        //string json = PlayerPrefs.GetString(typeof(T).ToString());
        return JsonUtility.FromJson<T>(json);
    }

    /// <summary>
    /// Delete data from file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void DeleteData<T>()
    {
        //PlayerPrefs.DeleteKey(typeof(T).ToString());
        File.Delete(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }

    /// <summary>
    /// Check if data exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool HasData<T>()
    {
        //return PlayerPrefs.HasKey(typeof(T).ToString());
        return File.Exists(Application.persistentDataPath + "/" + typeof(T).ToString() + ".json");
    }
}
