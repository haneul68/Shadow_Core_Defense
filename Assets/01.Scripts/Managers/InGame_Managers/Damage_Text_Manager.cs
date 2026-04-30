using System.Collections.Generic;
using UnityEngine;

public class Damage_Text_Manager : MonoBehaviour
{
    public static Damage_Text_Manager Instance;

    [SerializeField]
    private Transform damage_Text_Canvas;

    private readonly Dictionary<Transform, Damage_Stack_Info> stack_Infos = new Dictionary<Transform, Damage_Stack_Info>();

    [SerializeField] 
    private float stack_Reset_Time = 0.25f;
    [SerializeField]
    private float stack_Offset_Y = 22f;

    private const int max_Text_Count = 15;

    private void Awake()
    {
        Instance = this;
    }

    public void Show_Damage(Transform target, double damage, Color color)
    {
        if (target == null) return;
        if (damage_Text_Canvas == null) return;

        float offset = Get_Stack_Offset(target);

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Damage_Text).Get(obj =>
        {
            Damage_Text damage_Text = obj.GetComponent<Damage_Text>();
            if (damage_Text == null) return;

            damage_Text.Init(((int)damage).ToString(), target, damage_Text_Canvas, color, offset);
        });
    }

    private float Get_Stack_Offset(Transform target)
    {
        if (!stack_Infos.TryGetValue(target, out Damage_Stack_Info info))
        {
            info = new Damage_Stack_Info();
            stack_Infos[target] = info;
        }

        if (Time.time - info.lastTime > stack_Reset_Time)
        {
            info.stackCount = 0;
        }

        int index = info.stackCount % max_Text_Count;
        float offset = index * stack_Offset_Y;

        info.stackCount++;
        info.lastTime = Time.time;

        return offset;
    }
}

public class Damage_Stack_Info
{
    public int stackCount;
    public float lastTime;
}