using System.Collections.Generic;
using UnityEngine;

public class Skill_Tornado : MonoBehaviour
{
    [Header("REF")]
    [SerializeField] private CircleCollider2D damage_Trigger;
        

    private Player_Skill_Manager owner;
    private LayerMask enemy_Layer;
    private Pool_ID pool_Id;

    private double base_Atk;
    private float damage_Per_Second_Percent;
    private float duration;
    private float move_Speed;
    private float search_Radius;
    private float damage_Tick_Interval;

    private float life_Timer;
    private float damage_Tick_Timer;

    private Transform current_Target;

    private readonly List<Health_Manager> targets_In_Range = new List<Health_Manager>();

    private void Awake()
    {
        if (damage_Trigger == null)
            damage_Trigger = GetComponent<CircleCollider2D>();

        if (damage_Trigger != null)
            damage_Trigger.isTrigger = true;
    }

    public void Init(
        Player_Skill_Manager owner,
        double base_Atk,
        float damage_Per_Second_Percent,
        float duration,
        float move_Speed,
        float search_Radius,
        LayerMask enemy_Layer,float damage_Tick_Interval,
        Pool_ID pool_Id)
    {
        this.owner = owner;
        this.base_Atk = base_Atk;
        this.damage_Per_Second_Percent = damage_Per_Second_Percent;
        this.duration = duration;
        this.move_Speed = move_Speed;
        this.search_Radius = search_Radius;
        this.enemy_Layer = enemy_Layer;
        this.pool_Id = pool_Id;
        this.damage_Tick_Interval = damage_Tick_Interval;

        life_Timer = 0f;
        damage_Tick_Timer = 0f;
        current_Target = null;

        targets_In_Range.Clear();
        enabled = true;
    }

    private void Update()
    {
        life_Timer += Time.deltaTime;

        if (life_Timer >= duration)
        {
            Return_To_Pool();
            return;
        }

        Update_Target();
        Move_To_Target();
        Damage_Tick();
    }

    private void Update_Target()
    {
        if (current_Target != null)
        {
            Health_Manager current_Health = current_Target.GetComponent<Health_Manager>();
            if (current_Health == null)
                current_Health = current_Target.GetComponentInParent<Health_Manager>();

            if (current_Health != null && !current_Health.is_Dead)
                return;
        }

        current_Target = Find_Nearest_Target();
    }

    private Transform Find_Nearest_Target()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, search_Radius, enemy_Layer);

        float min_Distance = float.MaxValue;
        Transform nearest_Target = null;

        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D col = hits[i];
            if (col == null) continue;

            Health_Manager health = col.GetComponent<Health_Manager>();
            if (health == null)
                health = col.GetComponentInParent<Health_Manager>();

            if (health == null) continue;
            if (health.is_Dead) continue;

            float distance = Vector2.Distance(transform.position, health.transform.position);
            if (distance < min_Distance)
            {
                min_Distance = distance;
                nearest_Target = health.transform;
            }
        }

        return nearest_Target;
    }

    private void Move_To_Target()
    {
        if (current_Target == null)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            current_Target.position,
            move_Speed * Time.deltaTime
        );
    }

    private void Damage_Tick()
    {
        damage_Tick_Timer += Time.deltaTime;

        if (damage_Tick_Timer < damage_Tick_Interval)
            return;

        damage_Tick_Timer = 0f;

        double damage = base_Atk * (damage_Per_Second_Percent / 100f);

        for (int i = targets_In_Range.Count - 1; i >= 0; i--)
        {
            Health_Manager target = targets_In_Range[i];

            if (target == null)
            {
                targets_In_Range.RemoveAt(i);
                continue;
            }

            if (target.is_Dead)
            {
                targets_In_Range.RemoveAt(i);
                continue;
            }

            target.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Is_In_LayerMask(collision.gameObject.layer, enemy_Layer))
            return;

        Health_Manager health = collision.GetComponent<Health_Manager>();
        if (health == null)
            health = collision.GetComponentInParent<Health_Manager>();

        if (health == null) return;
        if (health.is_Dead) return;
        if (targets_In_Range.Contains(health)) return;

        targets_In_Range.Add(health);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Health_Manager health = collision.GetComponent<Health_Manager>();
        if (health == null)
            health = collision.GetComponentInParent<Health_Manager>();

        if (health == null) return;

        targets_In_Range.Remove(health);
    }

    private bool Is_In_LayerMask(int layer, LayerMask layer_Mask)
    {
        return ((1 << layer) & layer_Mask) != 0;
    }

    private void Return_To_Pool()
    {
        targets_In_Range.Clear();
        current_Target = null;
        owner = null;
        enabled = false;

        Base_Manager.Pool_Mng.pool_Dictionary[pool_Id].Return(gameObject);
    }

    private void OnDisable()
    {
        targets_In_Range.Clear();
        current_Target = null;
        life_Timer = 0f;
        damage_Tick_Timer = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, search_Radius);
    }
}