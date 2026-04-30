using UnityEngine;

public class Boss_Skill_Manager : MonoBehaviour
{
    public static Boss_Skill_Manager Instance;

    [Space(20)]
    [Header("Spawn_Center")]
    [SerializeField] private Transform spawn_Center_Obj;

    [Space(20)]
    [Header("Spawn_Radius")]
    [SerializeField] private float min_Radius;
    [SerializeField] private float max_Radius;

    private void Awake()
    {
        Instance = this;
    }

    #region SPAWN_POINT
    public Vector2 Get_Random_Position()
    {
        if (spawn_Center_Obj == null)
        {
            Debug.LogError("spawn_Center_Obj == null");
            return Vector2.zero;
        }

        if (min_Radius >= max_Radius)
        {
            Debug.LogError("min_Radius >= max_Radius");
            return spawn_Center_Obj.position;
        }

        int safety = 0;
        const int max_Try = 100;

        while (safety < max_Try)
        {
            safety++;

            float x = UnityEngine.Random.Range(-max_Radius, max_Radius);
            float y = UnityEngine.Random.Range(-max_Radius, max_Radius);

            if (Mathf.Abs(x) < min_Radius && Mathf.Abs(y) < min_Radius)
                continue;

            return (Vector2)spawn_Center_Obj.position + new Vector2(x, y);
        }

        return spawn_Center_Obj.position;
    }

    private void OnDrawGizmosSelected()
    {
        if (spawn_Center_Obj == null) return;

        Vector3 center = spawn_Center_Obj.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, Vector2.one * (min_Radius * 2));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, Vector2.one * (max_Radius * 2));
    }
    #endregion
}
