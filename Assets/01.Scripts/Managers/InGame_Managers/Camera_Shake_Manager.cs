using Unity.Cinemachine;
using UnityEngine;

public class Camera_Shake_Manager : MonoBehaviour
{
    public static Camera_Shake_Manager Instance { get; private set; }

    [Header("Impulse Source")]
    [SerializeField] 
    private CinemachineImpulseSource impulse_Source;

    [Space(20)]
    [Header("Rate Limit")]
    [SerializeField] 
    private float min_Shake_Interval = 0.03f;

    [Space(20)]
    [Header("Preset Force")]
    [SerializeField] 
    private float defualt_Force = 0.6f;

    [SerializeField]
    private Camera_Shake_Level shake_Level = Camera_Shake_Level.Strong;

    [Space(20)]
    [Header("2D Direction")]
    [SerializeField] 
    private Vector3 default_Velocity = new Vector3(0f, -1f, 0f);

    private float last_Shake_Time = -999f;

    public Camera_Shake_Level Shake_Level => shake_Level;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (impulse_Source == null)
            impulse_Source = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake()
    {
        if (impulse_Source == null) return;

        float multiplier = Get_Shake_Multiplier();
        if (multiplier <= 0f) return;

        float final_Force = defualt_Force * multiplier;

        if (final_Force <= 0f) return;

        if (Time.time < last_Shake_Time + min_Shake_Interval)
            return;

        last_Shake_Time = Time.time;

        impulse_Source.GenerateImpulse(final_Force);
    }
    public void Set_Shake_Level(Camera_Shake_Level level)
    {
        shake_Level = level;
    }
    private float Get_Shake_Multiplier()
    {
        switch (shake_Level)
        {
            case Camera_Shake_Level.Off:
                return 0f;

            case Camera_Shake_Level.Weak:
                return 0.5f;

            case Camera_Shake_Level.Strong:
                return 1f;
        }

        return 1f;
    }
}