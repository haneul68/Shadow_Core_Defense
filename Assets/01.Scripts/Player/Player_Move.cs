using UnityEngine;

public class Player_Move : MonoBehaviour, IDirection_Provider
{
    [SerializeField] Player player;
    [SerializeField] Player_Input_Manager player_Input_Manager;
    [SerializeField] Health_Manager health_Manager;
    [SerializeField] Stamina_Manager stamina_Manager;

    private Rigidbody2D rb;

    private bool is_Dash;

    private float dash_Time;
    private float max_Dash_Time = 0.3f;

    private Vector2 dash_Dir;

    Vector2 input_Direction;

    [SerializeField]
    DIRECTION current_Direction;
    public DIRECTION Current_Direction => current_Direction;

    public Vector2 Last_Facing_Dir { get;  set; } = Vector2.down;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        current_Direction = DIRECTION.Right;
    }

    void Update()
    {
        if (health_Manager.is_Dead == true)
        {
            Stop_Player();
            return;
        }

        input_Direction = player_Input_Manager != null ? player_Input_Manager.Move_Vector_2D : Vector2.zero;

        if (player_Input_Manager != null && player_Input_Manager.Dash_Triggered && !is_Dash)
        {
            Start_Dash();
        }

        Update_Direction();
    }
    private void FixedUpdate()
    {
        if (health_Manager.is_Dead == true)
        {
            Stop_Player();
            return;
        }

        if (is_Dash)
        {
            Player_Dash();
        }
        else
        {
            Vector2 move_Dir = input_Direction.normalized;

            rb.MovePosition(rb.position + move_Dir * Time.fixedDeltaTime * player.Final_Move_Speed);
        }
    }

    private void Init()
    {
        if (player == null)
        {
            player = GetComponent<Player>();
        }

        if (player_Input_Manager == null)
        {
            player_Input_Manager = GetComponent<Player_Input_Manager>();
        }
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }

    #region Player_Default_Control
    private void Update_Direction()
    {
        const float dead_Zone_Value = 0.1f;

        if (input_Direction.sqrMagnitude < 0.01f) return;

        float x = input_Direction.x;
        float y = input_Direction.y;

        if (x > dead_Zone_Value)
        {
            if (y > dead_Zone_Value)
            {
                current_Direction = DIRECTION.RightUp;
            }
            else if (y < -1 * dead_Zone_Value)
            {
                current_Direction = DIRECTION.RightDown;
            }
            else
            {
                current_Direction = DIRECTION.Right;
            }
        }
        else if (x < -1 * dead_Zone_Value)
        {
            if (y > dead_Zone_Value)
            {
                current_Direction = DIRECTION.LeftUp;
            }
            else if (y < -1 * dead_Zone_Value)
            {
                current_Direction = DIRECTION.LeftDown;
            }
            else
            {
                current_Direction = DIRECTION.Left;
            }
        }
        else
        {
            if (y > dead_Zone_Value)
            {
                current_Direction = DIRECTION.Up;
            }
            else if (y < -1 * dead_Zone_Value)
            {
                current_Direction = DIRECTION.Down;
            }
        }
        Last_Facing_Dir = Direction8.ToVector2(current_Direction);
    }

    private void Stop_Player() 
    {
        input_Direction = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        is_Dash = false;    
    }
    #endregion

    #region Player Dash
    private void Start_Dash()
    {
        dash_Dir = input_Direction.normalized;

        if (dash_Dir == Vector2.zero)
        {
            Debug.Log("방향값 없음");
            return; 
        }

        if (stamina_Manager.Use(10) == false) 
        {
            Debug.Log("스테미너 부족");
            return;
        }

        dash_Time = 0f;
        is_Dash = true;
    }
    private void Player_Dash()
    {
        dash_Time += Time.fixedDeltaTime;

        rb.linearVelocity = dash_Dir * (player.Final_Move_Speed * 3f);
        if (dash_Time >= max_Dash_Time)
        {
            dash_Time = 0f;
            is_Dash = false;
            rb.linearVelocity = Vector2.zero;
        }
    }
    #endregion

    public Vector2 Get_Direction()
    {
        return player_Input_Manager.Move_Vector_2D;
    }
}
