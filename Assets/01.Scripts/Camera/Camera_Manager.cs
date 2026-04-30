using Unity.Cinemachine;
using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    public static Camera_Manager Instance;

    [SerializeField]
    private CinemachineCamera cinemachine_Camera;

    private void Awake()
    {
        Instance = this;
    }

    public void Set_Target(Transform target) 
    {
        if (cinemachine_Camera == null || target == null)
        {
            Debug.Log("cinemachine_Camera == null || target == null");
        }

        cinemachine_Camera.Follow = target;
    }
}
