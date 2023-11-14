using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public void SaveData<T>(T data)
    {
        string json = JsonUtility.ToJson(data);

        PlayerPrefs.SetString(typeof(T).ToString(), json);
    }

    public T LoadData<T>()
    {
        string json = PlayerPrefs.GetString(typeof(T).ToString());

        return JsonUtility.FromJson<T>(json);
    }

    public void DeleteData<T>()
    {
        PlayerPrefs.DeleteKey(typeof(T).ToString());
    }
}
