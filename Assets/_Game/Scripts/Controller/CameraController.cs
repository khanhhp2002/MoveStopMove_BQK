using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool _isBlocked;
    private Action _onCameraUnblock;
    private RaycastHit[] _results = new RaycastHit[2];

    /// <summary>
    /// LateUpdate is called every frame
    /// </summary>
    private void LateUpdate()
    {
        if (GameplayManager.Instance.GameState != GameState.Playing) return;
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
        if (Physics.RaycastNonAlloc(ray, _results) > 0)
        {
            // If the hit object is not the player, then it is an obstacle
            foreach (RaycastHit hit in _results)
            {
                Debug.Log(hit.transform.gameObject.layer);
                if (hit.transform.gameObject.layer == (int)LayerType.Obstacle)
                {
                    _isBlocked = true;
                    ObstacleController obstacleController = hit.transform.GetComponent<ObstacleController>();
                    obstacleController.OnBlockCamera();
                    _onCameraUnblock += obstacleController.OnUnblockCamera;
                    break;
                }
                else
                {
                    _isBlocked = false;
                }
            }

            if (!_isBlocked)
            {
                _onCameraUnblock?.Invoke();
                _onCameraUnblock = null;
            }
        }

    }
}
