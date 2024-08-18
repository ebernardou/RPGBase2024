using Cinemachine;
using UnityEngine;

public class CinemachineSwitcher : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainCamera;

    [SerializeField]
    private CinemachineVirtualCamera npcCamera;

    private bool mainCameraIsActive = true;

    public void SwitchState()
    {
        if (mainCameraIsActive)
        {
            mainCamera.Priority = 0;
            npcCamera.Priority = 1;
            mainCameraIsActive = false;
        }
        else
        {
            mainCamera.Priority = 1;
            npcCamera.Priority = 0;
            mainCameraIsActive = true;
        }
    }

}
