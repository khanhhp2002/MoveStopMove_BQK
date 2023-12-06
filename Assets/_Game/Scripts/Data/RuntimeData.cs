public class RuntimeData : Singleton<RuntimeData>
{
    public UserData UserData;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadUserData();
    }

    /// <summary>
    /// Load user data from save file.
    /// </summary>
    private void LoadUserData()
    {
        if (SaveManager.Instance.HasData<UserData>())
        {
            UserData = SaveManager.Instance.LoadData<UserData>();
        }
        else
        {
            UserData = new UserData();
        }
    }

    /// <summary>
    /// Save user data to save file.
    /// </summary>
    public void SaveUserData()
    {
        SaveManager.Instance.SaveData(UserData);
    }

    /// <summary>
    /// Reset user data to default values.
    /// </summary>
    public void ResetUserData()
    {
        UserData = new UserData();
        SaveUserData();
    }
}
