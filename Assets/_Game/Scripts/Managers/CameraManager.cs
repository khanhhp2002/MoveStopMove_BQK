using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _menuCamera;
    [SerializeField] private CinemachineVirtualCamera _gameplayCamera;

    /// <summary>
    /// OnGameStatePrepare is called when the game state is preparing.
    /// </summary>
    public void OnGameStatePrepare()
    {
        _menuCamera.Priority = 1;
        _gameplayCamera.Priority = 0;
    }

    /// <summary>
    /// OnGameStatePlaying is called when the game state is playing.
    /// </summary>
    public void OnGameStatePlaying()
    {
        _menuCamera.Priority = 0;
        _gameplayCamera.Priority = 1;
    }
}
