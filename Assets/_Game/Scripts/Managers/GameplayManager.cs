using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : Singleton<GameplayManager>
{
    [SerializeField] private Material[] _pants;
    [SerializeField] private CharacterBase _player;
    [SerializeField] private VirtualCameraController _virualCameraController;

    /// <summary>
    /// Returns the player.
    /// </summary>
    public CharacterBase Player => _player;

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _virualCameraController.gameObject.SetActive(!_virualCameraController.gameObject.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// Get a random pant's skin material.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Material GetPantByIndex(int index = -1)
    {
        if (index < 0 || index >= _pants.Length)
        {
            return _pants[Random.Range(0, _pants.Length)];
        }
        else
        {
            return _pants[index];
        }
    }
}
