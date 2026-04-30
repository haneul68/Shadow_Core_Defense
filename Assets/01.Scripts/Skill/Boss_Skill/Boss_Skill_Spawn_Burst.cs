using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Skill_Spawn_Burst : Skill_Definition
{
    [Header("Spawn Target")]
    public Pool_ID spawn_Pool_ID;

    [Header("Warning Effect")]
    public float warning_Size = 3f;

    [Header("Burst Setting")]
    public int total_Spawn_Count = 10;
    public float burst_Interval = 0.4f;
    public int min_Spawn_Per_Burst = 1;
    public int max_Spawn_Per_Burst = 3;

    [Header("Hit Box")]
    public LayerMask target_Layer;


    public override Skill_Runtime Create_Runtime()
    {
        return new Boss_Skill_Spawn_Runtime(this);
    }
}

public class Boss_Skill_Spawn_Runtime : Skill_Runtime
{
    private readonly Boss_Skill_Spawn_Burst skillData;

    public Boss_Skill_Spawn_Runtime(Skill_Definition definition) : base(definition)
    {
        skillData = definition as Boss_Skill_Spawn_Burst;
    }

    protected override void On_Execute(GameObject owner)
    {
    }

    public override IEnumerator Execute_Coroutine(GameObject owner)
    {
        if (owner == null)
            yield break;

        if (skillData == null)
            yield break;

        int remain_Count = skillData.total_Spawn_Count;

        while (remain_Count > 0)
        {
            int burst_Count = Random.Range(skillData.min_Spawn_Per_Burst, skillData.max_Spawn_Per_Burst + 1);
            burst_Count = Mathf.Min(burst_Count, remain_Count);

            List<Vector2> spawn_Positions = new List<Vector2>();

            for (int i = 0; i < burst_Count; i++)
            {
                Vector2 pos;

                if (i == 0)
                {
                    Transform player = Base_Manager.Character_Mng.current_Character.transform;
                    pos = player.position;
                }
                else
                {
                    pos = Boss_Skill_Manager.Instance.Get_Random_Position();
                }

                int retry = 0;
                while (retry < 10)
                {
                    bool is_Too_Close = false;

                    for (int j = 0; j < spawn_Positions.Count; j++)
                    {
                        if (Vector2.Distance(spawn_Positions[j], pos) < 0.7f)
                        {
                            is_Too_Close = true;
                            break;
                        }
                    }

                    if (!is_Too_Close)
                        break;

                    pos = Boss_Skill_Manager.Instance.Get_Random_Position();
                    retry++;
                }

                spawn_Positions.Add(pos);
            }

            if (skillData.is_Attack_Skill)
            {
                Invoke_Skill_Execute();
            }

            for (int i = 0; i < spawn_Positions.Count; i++)
            {
                Vector2 spawnPos = spawn_Positions[i];

                Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Hit_Box_Spawn_Point).Get(warning_Obj =>
                {
                    warning_Obj.transform.position = spawnPos;
                    warning_Obj.transform.SetParent(null);

                    Hit_Box_Spawn_Point warning = warning_Obj.GetComponent<Hit_Box_Spawn_Point>();
                    if (warning != null)
                    {
                        warning.Init(warning_Obj.transform, skillData.warning_Size, skillData.cast_Time, false);
                    }
                });
            }

            yield return new WaitForSeconds(skillData.cast_Time);

            for (int i = 0; i < spawn_Positions.Count; i++)
            {
                Vector2 spawn_Pos = spawn_Positions[i];

                Spawn_Burst_Effect(spawn_Pos);
                Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Boss_Skill_Bump);
                Spawn_Circle_Hit_Box(spawn_Pos, Boss_Skill_Factory.Instance.Boss);
            }

            remain_Count -= burst_Count;

            if (remain_Count > 0)
            {
                yield return new WaitForSeconds(skillData.burst_Interval);
            }
        }
    }

    private void Spawn_Burst_Effect(Vector2 spawn_Pos)
    {
        Base_Manager.Pool_Mng.Pooling_OBJ(skillData.spawn_Pool_ID).Get(effect =>
        {
            effect.transform.position = spawn_Pos;
            effect.transform.SetParent(null);
            effect.SetActive(true);

            Effect_Return_Delay returnDelay = effect.GetComponent<Effect_Return_Delay>();
            if (returnDelay != null)
            {
                returnDelay.Init(skillData.spawn_Pool_ID, 4f);
            }
        });
    }
    private void Spawn_Circle_Hit_Box(Vector2 spawn_Pos, Boss boss)
    {
        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Melee_Hit_Box).Get(hit_Box =>
        {
            if (hit_Box == null)
            {
                Debug.Log("hit_Box == null");
                return;
            }

            Melee_Hit_Box hit_Box_C = hit_Box.GetComponent<Melee_Hit_Box>();
            if (hit_Box_C == null)
            {
                Debug.Log("hit_Box_C == null");
                return;
            }

            hit_Box.transform.position = spawn_Pos;
            hit_Box.transform.localScale = Vector3.one;
            hit_Box.transform.SetParent(null);

            hit_Box_C.Init_Circle(boss.Final_ATK, boss.gameObject, skillData.warning_Size, skillData.target_Layer);
        });
    }
}