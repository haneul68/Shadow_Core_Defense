using System;
using UnityEngine;

public class Enemy_Melee_Attack : MonoBehaviour, IAttack
{
    [Header("REF")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Enemy_Ai_Controller enemy_Ai_Controller;
    [SerializeField] private Health_Manager health_Manager;

    [Space(20)]
    [Header("Attack")]
    [SerializeField] private LayerMask target;
    [SerializeField] private float attack_Cooldown;

    private float last_Attack_Time = -999f;

    private Vector2 spawn_Point;
    private float size;

    public bool Is_Attack { get; set; }

    public event Action OnAttack;

    private GameObject current_Hit_Box_Spawn_Point;

    private void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();

        if (enemy_Ai_Controller == null)
            enemy_Ai_Controller = GetComponent<Enemy_Ai_Controller>();

        if (health_Manager == null)
            health_Manager = GetComponent<Health_Manager>();
    }

    private void OnEnable()
    {
        if (health_Manager != null)
            health_Manager.On_Died += Return_All_Attack_Objects;
    }

    private void OnDisable()
    {
        if (health_Manager != null)
            health_Manager.On_Died -= Return_All_Attack_Objects;

        Return_All_Attack_Objects();
    }

    private void Update()
    {
        if (Is_Attack == true && Time.time >= last_Attack_Time + attack_Cooldown)
        {
            Is_Attack = false;
        }
    }

    public void Try_Attack()
    {
        Debug.Log("Try_Attack");
        if (Is_Attack == true) return;

        if (Time.time < last_Attack_Time + attack_Cooldown)
            return;

        if (enemy_Ai_Controller == null)
        {
            Debug.Log("enemy_Ai_Controller == null");
            return;
        }

        Is_Attack = true;
        last_Attack_Time = Time.time;

        OnAttack?.Invoke();

        (spawn_Point, size) = Set_Hit_Box();

        // 혹시 이전 것이 남아있으면 먼저 반환
       // Return_Hit_Box_Spawn_Point();

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Hit_Box_Spawn_Point).Get(hit_Box_Spawn_Point =>
        {
            if (hit_Box_Spawn_Point == null) return;

            current_Hit_Box_Spawn_Point = hit_Box_Spawn_Point;

            hit_Box_Spawn_Point.transform.SetParent(transform);
            hit_Box_Spawn_Point.transform.position = spawn_Point;
            hit_Box_Spawn_Point.transform.localScale = Vector3.one;

            Hit_Box_Spawn_Point spawnPointComp = hit_Box_Spawn_Point.GetComponent<Hit_Box_Spawn_Point>();
            if (spawnPointComp != null)
            {
                spawnPointComp.Init(hit_Box_Spawn_Point.transform, size, 0.3f);
            }
        });
    }

    public void Spawn_Hit_Box()
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

            hit_Box_C.Init_Circle(enemy.Final_ATK, gameObject, size, target);
            hit_Box_C.transform.position = spawn_Point;
            hit_Box_C.transform.localScale = Vector3.one;
        });
    }

    public void Spawn_Hit_Box_With_Effect()
    {
        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Blue_Slime_Attack_Effect).Get(effect =>
        {
            effect.transform.position = spawn_Point;
            effect.GetComponent<Effect_Return_Delay>().Init(Pool_ID.Blue_Slime_Attack_Effect, 4);
        });

        Spawn_Hit_Box();
    }

    private (Vector2 spawn_Point, float size) Set_Hit_Box()
    {
        Vector2 spawn_Point;
        float size;

        if (enemy.Enemy_Type == Enemy_Type.Ranged_Attacker)
        {
            spawn_Point = Base_Manager.Character_Mng.current_Character.transform.position;
            size = enemy.Attack_Distance / 4.8f;
        }
        else
        {
            spawn_Point = transform.position;
            size = enemy.Attack_Distance;
        }

        return (spawn_Point, size);
    }

    private void Return_All_Attack_Objects()
    {
        Is_Attack = false;
        Return_Hit_Box_Spawn_Point();
    }

    private void Return_Hit_Box_Spawn_Point()
    {
        if (current_Hit_Box_Spawn_Point == null) return;

        Hit_Box_Spawn_Point point = current_Hit_Box_Spawn_Point.GetComponent<Hit_Box_Spawn_Point>();

        if (point != null)
        {
            point.Force_Return();
        }

        current_Hit_Box_Spawn_Point = null;
    }
}