using UnityEngine;

public class UI_Core_Gauge : MonoBehaviour
{
    [Header("Gauge Images")]
    [SerializeField] 
    private GameObject[] gauge_Image;
    [SerializeField]
    private Core_Health core;

    private void Awake()
    {
        if (core != null)
            core.On_Gauge_Changed += Update_Gauge;
    }

    private void OnDisable()
    {
        if (core != null)
            core.On_Gauge_Changed -= Update_Gauge;
    }

    private void Update_Gauge(int current, int max)
    {
        if (gauge_Image == null || gauge_Image.Length == 0) return;
        float percent = (float)current / max;

        int index = Mathf.RoundToInt((gauge_Image.Length - 1) * percent);
        index = Mathf.Clamp(index, 0, gauge_Image.Length - 1);
            
        for (int i = 0; i < gauge_Image.Length; i++)
        {
            if (gauge_Image[i] == null) continue;
            gauge_Image[i].SetActive(i == index);
        }
    }
}