using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    public void OnBlockCamera()
    {
        _meshRenderer.material = GameplayManager.Instance._obstacleMaterials[0];
    }

    public void OnUnblockCamera()
    {
        _meshRenderer.material = GameplayManager.Instance._obstacleMaterials[1];
    }
}
