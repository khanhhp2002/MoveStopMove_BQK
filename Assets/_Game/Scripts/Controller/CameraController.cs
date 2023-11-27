using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _isBlocked;
    private Action _onCameraUnblock;
    private RaycastHit[] _results = new RaycastHit[2];
    private LayerMask _layerMask;
    private CharacterBase _player;

    /// <summary>
    /// Start is called before the first frame update.
    /// </summary>
    private void Start()
    {
        _layerMask = LayerMask.GetMask(LayerType.Obstacle.ToString(), LayerType.Character.ToString());
        _player = GameplayManager.Instance.Player;
    }
    /// <summary>
    /// LateUpdate is called every frame
    /// </summary>
    private void LateUpdate()
    {
        if (GameplayManager.Instance.GameState != GameState.Playing) return;

        RaycastHit hit2;
        if (Physics.Raycast(transform.position, _player.transform.position - transform.position, out hit2, 50f, _layerMask))
        {
            if (hit2.transform.gameObject.layer == (int)LayerType.Obstacle)
            {
                _isBlocked = true;
                ObstacleController obstacleController = hit2.transform.GetComponent<ObstacleController>();
                obstacleController.OnBlockCamera();
                _onCameraUnblock += obstacleController.OnUnblockCamera;
            }
            else if (_isBlocked && hit2.transform.gameObject.layer == (int)LayerType.Character)
            {
                _isBlocked = false;
                _onCameraUnblock?.Invoke();
                _onCameraUnblock = null;
            }
        }

    }
}
