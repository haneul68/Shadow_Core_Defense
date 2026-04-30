using System;
using UnityEngine;

[Serializable]
public class Boss_Skill_Teleport_Data
{
    public string skill_Name = "Boss_Skill_Teleport";
    public float cast_Time = 1f;
    public bool is_Attack_Skill = false;
}

[Serializable]
public class Boss_Skill_Spawn_Slime_Data
{
    public string skill_Name = "Boss_Skill_Spawn_Slime";
    public float cast_Time = 1f;
    public bool is_Attack_Skill = true;

    public int magma_Count = 2;
    public int green_Count = 3;
    public int blue_Count = 1;
}

[Serializable]
public class Boss_Skill_Spawn_Burst_Data
{
    public string skill_Name = "Boss_Skill_Spawn_Burst";
    public float cast_Time = 0.3f;
    public bool is_Attack_Skill = true;

    public Pool_ID spawn_Pool_ID = Pool_ID.Boss_Boom_SKill_Effect;

    public float warning_Size = 3f;

    public int total_Spawn_Count = 10;
    public float burst_Interval = 0.4f;
    public int min_Spawn_Per_Burst = 1;
    public int max_Spawn_Per_Burst = 3;

    public LayerMask target_Layer;
}
