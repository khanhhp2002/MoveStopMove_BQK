using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _menuCamera;
    [SerializeField] private CinemachineVirtualCamera _gameplayCamera;
    [SerializeField] private float _cameraDefaultDistance;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        GameplayManager.Instance.OnGameStatePrepare += Init;
        GameplayManager.Instance.OnGameStatePrepare += MenuCameraSetting;
        GameplayManager.Instance.OnGameStatePlaying += PlayingCameraSetting;
    }

    /// <summary>
    /// Init is called when the game state is preparing.
    /// </summary>
    private void Init()
    {
        _gameplayCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = _cameraDefaultDistance;
    }

    /// <summary>
    /// OnGameStatePrepare is called when the game state is preparing.
    /// </summary>
    private void MenuCameraSetting()
    {
        _menuCamera.Priority = 1;
        _gameplayCamera.Priority = 0;
    }

    /// <summary>
    /// OnGameStatePlaying is called when the game state is playing.
    /// </summary>
    private void PlayingCameraSetting()
    {
        _menuCamera.Priority = 0;
        _gameplayCamera.Priority = 1;
    }

    /// <summary>
    /// Zoom out the gameplay camera.
    /// </summary>
    /// <param name="value"></param>
    public void ZoomOutGamePlayCamera(float value)
    {
        _gameplayCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = _cameraDefaultDistance * value * 1.2f;
    }
}
