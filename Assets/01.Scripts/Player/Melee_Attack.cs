using System;
using UnityEngine;

public class Melee_Attack : MonoBehaviour, IAttack
{
    [Header("REF")]
    [SerializeField] private Player_Move player_Move;
    [SerializeField] private Character character;
    [SerializeField] private Player_Input_Manager player_Input_Manager;
    [SerializeField] private LayerMask target;

    [Space(20)]
    [Header("Value")]
    [SerializeField]
    private Transform hit_Box_Position;
    [SerializeField]
    private float attack_Cooldown = 0.3f;
    [SerializeField] 
    private float spawn_Distance = 0.8f;
    [SerializeField]
    private SFX_Type Attack_Sound;
    private float last_Attack_Time = -999f;

    public event Action OnAttack;

    public bool Is_Attack { get; set; }

    private void Awake()
    {
        if (player_Move == null) 
        {
            player_Move = GetComponent<Player_Move>();  
        }
        if (character == null) 
        {
            character = GetComponent<Character>();
        }
        if (player_Input_Manager == null) 
        {
            player_Input_Manager = GetComponent<Player_Input_Manager>();
        }
    }

    private void Update()
    {
        if (player_Input_Manager.Attack_Triggered && !Is_Attack)
        {
            Try_Attack();
        }

        if (Is_Attack == true && Time.time >= last_Attack_Time + attack_Cooldown) 
        {
            Is_Attack = false;
        }
    }

    public void Try_Attack() 
    {
        if (player_Move == null || player_Input_Manager == null || !player_Input_Manager.Attack_Triggered || player_Input_Manager.Dash_Triggered == true)
        {
            Debug.Log("player_Move == null || input_Reader == null");
            return;
        }
        Debug.Log($"{character.Final_ATK}");

        if (Time.time < last_Attack_Time + attack_Cooldown)
            return;

        Base_Manager.Sound_Mng.Play_SFX(Attack_Sound);

        Is_Attack = true;
        OnAttack?.Invoke();
        last_Attack_Time = Time.time;

        Spawn_Hit_Box();
    }
    public void Spawn_Hit_Box() 
    {
        Vector2 attack_Dir = player_Move.Last_Facing_Dir;

        Vector3 spawn_Position = hit_Box_Position.position + (Vector3)(attack_Dir * spawn_Distance);

        Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Melee_Hit_Box).Get(hit_Box =>
        {
            if (hit_Box == null)
            {
                Debug.LogError("hit_Box null");
                return;
            }

            var hit_Box_C = hit_Box.GetComponent<Melee_Hit_Box>();

            if (hit_Box_C == null)
            {
                Debug.LogError("Melee_Hit_Box 컴포넌트 없음");
                return;
            }

            hit_Box_C.Init_Box(character.Final_ATK, attack_Dir, gameObject, target);

            Vector3 parent_Scale = transform.lossyScale;

            hit_Box.transform.SetParent(transform, false);
            hit_Box.transform.position = spawn_Position;
            hit_Box.transform.localScale = new Vector3(1f / parent_Scale.x,1f / parent_Scale.y,1f / parent_Scale.z);
        }); 
    }
}
