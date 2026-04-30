using System.Collections.Generic;
using UnityEngine;

public enum HitBoxType
{
    Box,   
    Circle  
}

public class Melee_Hit_Box : MonoBehaviour
{
    [Header("Life_Time")]
    [SerializeField] private float life_Time = 0.1f;

    [Header("Target_Layer")]
    [SerializeField] private LayerMask target_Layer;

    [Header("Hit_Box_Size")]
    [Tooltip("Horizontal")]
    [SerializeField] private Vector2 horizontal_Size = new Vector2(1f, 2f);
    [Tooltip("Vertical")]
    [SerializeField] private Vector2 vertical_Size = new Vector2(2f, 1f);

    private double damage;
    private GameObject owner;
    private Vector2 attack_Direction;

    private HashSet<Collider2D> hit_Targets = new HashSet<Collider2D>();
    private BoxCollider2D box_Collider_2D;
    private CircleCollider2D circle_Collider_2D;

    private void Awake()
    {
        box_Collider_2D = GetComponent<BoxCollider2D>();
        circle_Collider_2D = GetComponent<CircleCollider2D>();

        if (box_Collider_2D != null) box_Collider_2D.isTrigger = true;
        if (circle_Collider_2D != null) circle_Collider_2D.isTrigger = true;
    }
    private void OnDisable()
    {
        hit_Targets.Clear(); 
    }

    public void Init_Box(double damage, Vector2 attack_Direction, GameObject owner, LayerMask target_Layer)
    {
        hit_Targets.Clear();

        this.damage = damage;
        this.owner = owner;
        this.target_Layer = target_Layer;
        this.attack_Direction = attack_Direction;
        box_Collider_2D.enabled = true;
        circle_Collider_2D.enabled = false;

        Set_Dir_Hit_Box_Size();

        Base_Manager.Instance.Return_Pool_Obj_Delay(Pool_ID.Melee_Hit_Box, gameObject, life_Time);
    }

    public void Init_Circle(double damage, GameObject owner, float range, LayerMask target_Layer)
    {
        hit_Targets.Clear();

        this.damage = damage;
        this.owner = owner;
        this.target_Layer = target_Layer;
        box_Collider_2D.enabled = false;
        circle_Collider_2D.enabled = true;

        circle_Collider_2D.radius = range;

        Base_Manager.Instance.Return_Pool_Obj_Delay(Pool_ID.Melee_Hit_Box, gameObject, life_Time);
    }

    private void Set_Dir_Hit_Box_Size()
    {
        if (Mathf.Abs(attack_Direction.x) > Mathf.Abs(attack_Direction.y))
        {
            box_Collider_2D.size = horizontal_Size;
        }
        else 
        {
            box_Collider_2D.size = vertical_Size;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"OnTriggerEnter2D : 충돌 오브젝트 이름: {collision.name}");
        if (collision.gameObject == owner)
        {
            Debug.Log("collision.gameObject == owner");
            return;
        }
        if (((1 << collision.gameObject.layer) & target_Layer) == 0)
        {
            Debug.Log("collision.gameObject.layer & target_Layer) == 0");
            Debug.Log($"충돌 오브젝트 이름: {collision.name}");
            Debug.Log($"[HitBox] targetLayer: {target_Layer.value}");
            Debug.Log($"[HitBox] collisionLayer: {collision.gameObject.layer}");
            return;
        }
        if (hit_Targets.Contains(collision))
        {
            Debug.Log($"hit_Targets : 충돌 오브젝트 이름: {collision.name}");
            return;
        }
        hit_Targets.Add(collision);
        IDamageable hp = collision.GetComponent<IDamageable>();
        if (hp != null && !hp.is_Dead)
        {
            hp.TakeDamage(damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (circle_Collider_2D != null && circle_Collider_2D.enabled)
        {
            Gizmos.DrawWireSphere(transform.position, circle_Collider_2D.radius);
        }
        else if (box_Collider_2D != null && box_Collider_2D.enabled)
        {
            Gizmos.DrawWireCube(transform.position, box_Collider_2D.size);
        }
    }
}
