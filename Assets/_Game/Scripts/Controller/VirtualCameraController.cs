using Cinemachine;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour
{
    [Header("Virtual Camera Components"), Space(5f)]
    [SerializeField] protected CinemachineVirtualCamera cinemachineVirtualCamera;

    [Header("Virtual Camera Stats"), Space(5f)]
    [SerializeField] protected bool invert;
    [SerializeField] protected float cameraRotateSpeed;

    private CinemachineOrbitalTransposer cinemachineOrbitalTransposer;
    private bool _isDoubleCheck = false;

    /// <summary>
    /// OnEnable is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        _isDoubleCheck = false;
        Invoke(nameof(Init), .1f);
    }

    /// <summary>
    /// Try to get the CinemachineOrbitalTransposer component from the CinemachineVirtualCamera.
    /// </summary>
    private void Init()
    {
        if (cinemachineOrbitalTransposer is null)
        {
            cinemachineOrbitalTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            Invoke(nameof(Init), .1f);
            return;
        }

        cinemachineOrbitalTransposer.m_XAxis.m_InputAxisValue = cameraRotateSpeed;
        cinemachineOrbitalTransposer.m_XAxis.m_InvertInput = invert;

        if (!_isDoubleCheck)
        {
            _isDoubleCheck = true;
            Invoke(nameof(DoubleCheck), .1f);
        }
    }

    /// <summary>
    /// Double check if the CinemachineOrbitalTransposer component is set correctly.
    /// </summary>
    private void DoubleCheck()
    {
        if (cinemachineOrbitalTransposer.m_XAxis.m_InputAxisValue != cameraRotateSpeed)
        {
            Init();
        }
    }

    /// <summary>
    /// Set the priority of the CinemachineVirtualCamera.
    /// </summary>
    /// <param name="value"></param>
    public void ChangePriority(int value)
    {
        cinemachineVirtualCamera.Priority = value;
        OnEnable();
    }

    /// <summary>
    /// Change the rotation speed of the CinemachineOrbitalTransposer.
    /// </summary>
    /// <param name="value"></param>
    public void ChangeCameraRotateSpeed(float value)
    {
        cameraRotateSpeed = value;
        OnEnable();
    }
}
