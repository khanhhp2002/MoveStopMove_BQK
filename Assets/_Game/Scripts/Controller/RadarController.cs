using System;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    [SerializeField] private SphereCollider _sphereCollider;
    private Action<CharacterBase> _onEnemyEnterCallBack;
    private Action<CharacterBase> _onEnemyExitCallBack;

    public SphereCollider SphereCollider => _sphereCollider;
    public void OnEnemyEnterCallBack(Action<CharacterBase> callBack)
    {
        _onEnemyEnterCallBack += callBack;
    }

    public void OnEnemyExitCallBack(Action<CharacterBase> callBack)
    {
        _onEnemyExitCallBack += callBack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Character)
        {
            _onEnemyEnterCallBack?.Invoke(other.GetComponent<CharacterBase>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (byte)LayerType.Character)
        {
            _onEnemyExitCallBack?.Invoke(other.GetComponent<CharacterBase>());
        }
    }

}
