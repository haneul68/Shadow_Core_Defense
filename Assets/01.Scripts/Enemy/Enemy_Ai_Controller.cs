using UnityEngine;

public class Enemy_Ai_Controller : MonoBehaviour, IDirection_Provider
{
    [Header("REF")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Enemy_Melee_Attack enemy_Melee_Attack;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Health_Manager health_manager;

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Enemy_State enemy_State;

    [SerializeField] private LayerMask target_Layer;

    public Vector2 Last_Facing_Dir { get; set; } = Vector2.down;

    private void Awake()
    {
        if (enemy == null) 
        {
            enemy = GetComponent<Enemy>();
        }

        if (enemy_Melee_Attack == null) 
        {
            enemy_Melee_Attack = GetComponent<Enemy_Melee_Attack>();
        }

        if (health_manager == null)
        {
            health_manager = GetComponent<Health_Manager>();
        }
    }

    private void OnEnable()
    {
        enemy_State = Enemy_State.Idle;
        health_manager.On_Died += OnDeath;
    }

    private void OnDisable()
    {
        health_manager.On_Died -= OnDeath;

    }

    private void Update()
    {
        if (Round_Manager.Instance.Game_Ended) return;

        if (health_manager.is_Dead == true)
        {
            rb.linearVelocity = Vector2.zero;
            enemy_State = Enemy_State.Death;
            return;
        }
        if (target == null) 
        {
            Find_Target();
            return;
        }

        Evaluate_Transition();

        if (enemy_State == Enemy_State.Attack)
        {
            if (enemy_Melee_Attack.Is_Attack == false) 
            {
                Attack();
            }
        }
    }

    private void FixedUpdate()
    {
        if (Round_Manager.Instance.Game_Ended) return;
        if (target == null) return;

        if (enemy_State == Enemy_State.Chase)
        {
            Chase();
        }
    }

    private void Find_Target() 
    {
        if (enemy.Enemy_Type == Enemy_Type.Core_Attacker)
        {
            GameObject core = GameObject.FindGameObjectWithTag("Core");
            if (core != null)
            {
                target = core.transform;
            }
        }
        else if (enemy.Enemy_Type == Enemy_Type.Melee_Attacker || enemy.Enemy_Type == Enemy_Type.Ranged_Attacker) 
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void Chase() 
    {
        Vector2 dir = Get_Direction();

        rb.MovePosition(rb.position + dir * enemy.Final_Move_Speed * Time.fixedDeltaTime);
    }

    private void Attack() 
    {
        if (enemy_Melee_Attack == null) 
        {
            Debug.Log("enemy_Melee_Attack == null");
            return;
        }
        enemy_Melee_Attack.Try_Attack();
    }

    private void OnDeath() 
    {
        if(health_manager == null) return;

        Base_Manager.Instance.Return_Pool_Obj_Delay(enemy.Enemy_Pool_ID, this.gameObject, 1f);
    }

    private void Evaluate_Transition()
    {
        if (target == null) 
        {
            enemy_State = Enemy_State.Idle;
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(
            transform.position,
            enemy.Attack_Distance,
            target_Layer
        );


        if (hit != null && hit.transform == target)
        {
            if (enemy_Melee_Attack.Is_Attack == false)
            {
                
                enemy_State = Enemy_State.Attack;
            }
            else 
            {
                enemy_State = Enemy_State.Idle;
            }
            
        }
        else
        {
            if (enemy_Melee_Attack.Is_Attack == false)
            {
                enemy_State = Enemy_State.Chase;
            }   
        }
    }
    private void OnDrawGizmos()
    {
        if (enemy_State == Enemy_State.Attack)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, enemy.Attack_Distance);
    }

    public Vector2 Get_Direction()
    {
        if (target == null) return Vector2.zero;

        if (enemy_State != Enemy_State.Chase) 
        {
            return Vector2.zero;
        }

        Vector2 dir = (target.position - transform.position).normalized;

        if (dir.sqrMagnitude > 0.001f) 
        {
            Last_Facing_Dir = dir;
        }

        return dir; 
    }
}
