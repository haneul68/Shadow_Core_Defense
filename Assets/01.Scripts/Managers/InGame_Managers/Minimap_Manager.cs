using System.Collections.Generic;
using UnityEngine;

public class Minimap_Manager : MonoBehaviour
{
    public static Minimap_Manager Instance;

    [Header("Map Bound")]
    [SerializeField] private float min_X;
    [SerializeField] private float max_X;
    [SerializeField] private float min_Y;
    [SerializeField] private float max_Y;

    [Header("UI")]
    [SerializeField] private RectTransform minimapRect;

    private List<IMinimap_Target> targets = new List<IMinimap_Target>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        targets.Clear();
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    public void Register(IMinimap_Target target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void Unregister(IMinimap_Target target)
    {
        targets.Remove(target);
    }

    private void Update()
    {
        foreach (var t in targets)
        {
            if (t == null) continue;

          RectTransform icon = t.Get_Minimap_Icon();

            if (icon == null)
                continue;

            Vector3 pos = t.Get_Transform().position;

            float x = (pos.x - min_X) / (max_X - min_X);
            float y = (pos.y - min_Y) / (max_Y - min_Y);

            Vector2 uiPos = new Vector2(x * minimapRect.sizeDelta.x,y * minimapRect.sizeDelta.y);

            icon.anchoredPosition = uiPos;
        }
    }
}