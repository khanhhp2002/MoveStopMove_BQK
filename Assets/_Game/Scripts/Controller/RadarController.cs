using System;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    private Action<CharacterBase> _onEnemyEnterCallBack;
    private Action<CharacterBase> _onEnemyExitCallBack;
    private Action<Vector3> _onWallDetectedCallBack;

    public SphereCollider SphereCollider => _sphereCollider;

    /// <summary>
    /// Called when radar detects an enemy.
    /// </summary>
    /// <param name="callBack"></param>
    public void OnEnemyEnterCallBack(Action<CharacterBase> callBack)
    {
        _onEnemyEnterCallBack += callBack;
    }

    /// <summary>
    /// Called when radar loses an enemy.
    /// </summary>
    /// <param name="callBack"></param>
    public void OnEnemyExitCallBack(Action<CharacterBase> callBack)
    {
        _onEnemyExitCallBack += callBack;
    }

    /// <summary>
    /// Called when radar detects a wall.
    /// </summary>
    /// <param name="callBack"></param>
    public void OnWallDetectedCallBack(Action<Vector3> callBack)
    {
        _onWallDetectedCallBack += callBack;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Character)
        {
            _onEnemyEnterCallBack?.Invoke(other.GetComponent<CharacterBase>());
        }
        if (other.gameObject.layer == (byte)LayerType.Wall)
        {
            _onWallDetectedCallBack?.Invoke(other.ClosestPoint(transform.position));
        }
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Character)
        {
            _onEnemyExitCallBack?.Invoke(other.GetComponent<CharacterBase>());
        }
    }
}
