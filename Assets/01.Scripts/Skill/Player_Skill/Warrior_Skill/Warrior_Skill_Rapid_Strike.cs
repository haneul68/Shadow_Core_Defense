using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior_Skill_Rapid_Strike : Player_Skill_Base
{
    [Header("Rapid Strike")]
    [SerializeField] private float radius = 3f;
    [SerializeField] private int hit_Count = 10;
    [SerializeField] private float hit_Interval = 0.08f;

    [Header("Damage")]
    [SerializeField] private float damage_Per_Hit_Percent = 30f;

    [Header("Target")]
    [SerializeField] private LayerMask enemy_Layer;

    [Header("Effect")]
    [SerializeField] private bool use_Cast_Effect = true;
    [SerializeField] private Pool_ID cast_Effect_Pool_ID;

    [SerializeField] private bool use_Hit_Effect = true;
    [SerializeField] private Pool_ID hit_Effect_Pool_ID;

    [SerializeField] private Vector3 cast_Effect_Offset = Vector3.zero;
    [SerializeField] private Vector3 hit_Effect_Offset = Vector3.zero;

    protected override object[] Get_Description_Values()
    {
        return new object[]
        {
            hit_Count,
            damage_Per_Hit_Percent.ToString("F0"),
            radius.ToString("F1")
        };
    }

    protected override IEnumerator Execute(Player_Skill_Manager owner)
    {
        if (owner == null)
            yield break;

        Transform ownerTransform = owner.transform;
        if (ownerTransform == null)
            yield break;

        Spawn_Cast_Effect(owner);

        double damagePerHit = Get_Damage_Per_Hit(owner);

        for (int i = 0; i < hit_Count; i++)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(ownerTransform.position, radius, enemy_Layer);
            HashSet<Health_Manager> hitTargets = new HashSet<Health_Manager>();

            for (int j = 0; j < hits.Length; j++)
            {
                Collider2D col = hits[j];
                if (col == null) continue;

                Health_Manager health = col.GetComponent<Health_Manager>();
                if (health == null)
                    health = col.GetComponentInParent<Health_Manager>();

                if (health == null) continue;
                if (health.is_Dead) continue;

                hitTargets.Add(health);
            }

            foreach (Health_Manager target in hitTargets)
            {
                if (target == null) continue;
                if (target.is_Dead) continue;

                target.TakeDamage(damagePerHit);
                Spawn_Hit_Effect(owner, target.transform.position);
            }

            yield return new WaitForSeconds(hit_Interval);
        }
    }

    private double Get_Damage_Per_Hit(Player_Skill_Manager owner)
    {
        if (owner == null) return 0;

        Character character = owner.GetComponent<Character>();
        if (character == null)
        {
            Debug.Log("character == null");
            return 0;
        }

        double percent = damage_Per_Hit_Percent / 100.0;
        double final_Damage = character.Final_ATK * percent;

        return final_Damage;
    }

    private void Spawn_Cast_Effect(Player_Skill_Manager owner)
    {
        if (!use_Cast_Effect) return;

        Base_Manager.Pool_Mng.Pooling_OBJ(cast_Effect_Pool_ID).Get(effect =>
        {
            effect.transform.position = owner.transform.position + cast_Effect_Offset;
            effect.transform.rotation = Quaternion.identity;

            owner.Register_Spawned_Object(effect, cast_Effect_Pool_ID);
            effect.GetComponent<Effect_Return_Delay>().Init(cast_Effect_Pool_ID, 3);
        });
    }

    private void Spawn_Hit_Effect(Player_Skill_Manager owner, Vector3 hitPosition)
    {
        if (!use_Hit_Effect) return;

        Base_Manager.Pool_Mng.Pooling_OBJ(hit_Effect_Pool_ID).Get(effect =>
        {
            effect.transform.position = hitPosition + hit_Effect_Offset;
            effect.transform.rotation = Quaternion.identity;

            owner.Register_Spawned_Object(effect, hit_Effect_Pool_ID);
            effect.GetComponent<Effect_Return_Delay>().Init(hit_Effect_Pool_ID, 2);
        });
    }

    public override void Force_Stop(Player_Skill_Manager owner)
    {
        StopAllCoroutines();
    }
}