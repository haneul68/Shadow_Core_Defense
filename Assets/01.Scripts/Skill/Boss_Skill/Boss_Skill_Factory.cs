using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_Factory : MonoBehaviour
{
    [SerializeField]
    private Boss boss;

    public static Boss_Skill_Factory Instance { get; private set; }

    [Header("Teleport")]
    [SerializeField] 
    private Boss_Skill_Teleport_Data teleport_Data = new Boss_Skill_Teleport_Data();

    [Header("Spawn Slime")]
    [SerializeField] private 
        Boss_Skill_Spawn_Slime_Data spawn_Slime_Data = new Boss_Skill_Spawn_Slime_Data();

    [Header("Spawn Burst")]
    [SerializeField] 
    private Boss_Skill_Spawn_Burst_Data spawn_Burst_Data = new Boss_Skill_Spawn_Burst_Data();

    private Dictionary<Boss_Skill_Type, Skill_Definition> boss_Skills_Cache = new Dictionary<Boss_Skill_Type, Skill_Definition>();

    private readonly List<GameObject> active_Burst_Warnings = new List<GameObject>();
    private readonly List<GameObject> active_Burst_Effects = new List<GameObject>();
    private readonly List<GameObject> active_Burst_HitBoxes = new List<GameObject>();

    public Boss Boss => boss;    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (boss == null)
            boss = GetComponent<Boss>();

        Instance = this;
        Init();
    }

    public void Init()
    {
        boss_Skills_Cache.Clear();

        boss_Skills_Cache[Boss_Skill_Type.Teleport] = new Boss_Skill_Teleport()
        {
            skill_Name = teleport_Data.skill_Name,
            cast_Time = teleport_Data.cast_Time,
            is_Attack_Skill = teleport_Data.is_Attack_Skill
        };

        boss_Skills_Cache[Boss_Skill_Type.Spawn_Slime] = new Boss_Skill_Spawn_Slime()
        {
            skill_Name = spawn_Slime_Data.skill_Name,
            cast_Time = spawn_Slime_Data.cast_Time,
            is_Attack_Skill = spawn_Slime_Data.is_Attack_Skill,
            magma_Count = spawn_Slime_Data.magma_Count,
            green_Count = spawn_Slime_Data.green_Count,
            blue_Count = spawn_Slime_Data.blue_Count
        };

        boss_Skills_Cache[Boss_Skill_Type.Spawn_Burst] = new Boss_Skill_Spawn_Burst()
        {
            skill_Name = spawn_Burst_Data.skill_Name,
            cast_Time = spawn_Burst_Data.cast_Time,
            is_Attack_Skill = spawn_Burst_Data.is_Attack_Skill,
            spawn_Pool_ID = spawn_Burst_Data.spawn_Pool_ID,
            warning_Size = spawn_Burst_Data.warning_Size,
            total_Spawn_Count = spawn_Burst_Data.total_Spawn_Count,
            burst_Interval = spawn_Burst_Data.burst_Interval,
            min_Spawn_Per_Burst = spawn_Burst_Data.min_Spawn_Per_Burst,
            max_Spawn_Per_Burst = spawn_Burst_Data.max_Spawn_Per_Burst,

            target_Layer = spawn_Burst_Data.target_Layer
        };
    }

    public Skill_Runtime Create_Boss_Skill(Boss_Skill_Type boss_Skill_Type)
    {
        if (!boss_Skills_Cache.TryGetValue(boss_Skill_Type, out Skill_Definition definition))
        {
            Debug.LogError($"보스 스킬 캐시에 없음 : {boss_Skill_Type}");
            return null;
        }

        return definition.Create_Runtime();
    }

}