using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Player_Input_Manager : MonoBehaviour
{
    #region Variable
    private PlayerInput player_Input;

    private InputAction move_Action;
    private InputAction dash_Action;
    private InputAction attack_Action;

    private InputAction use_Item_1_Action;
    private InputAction use_Item_2_Action;
    private InputAction use_Item_3_Action;
    private InputAction use_Item_4_Action;
    private InputAction use_Item_5_Action;

    private InputAction skill_Q_Action;
    private InputAction skill_W_Action;
    private InputAction skill_E_Action;

    public bool is_Moving = false;
    #endregion

    #region Property
    public Vector2 Move_Vector_2D { get; private set; }
    public bool Dash_Triggered { get; private set; }
    public bool Attack_Triggered { get; private set; }

    public bool Use_Item_1_Triggered { get; private set; }
    public bool Use_Item_2_Triggered { get; private set; }
    public bool Use_Item_3_Triggered { get; private set; }
    public bool Use_Item_4_Triggered { get; private set; }
    public bool Use_Item_5_Triggered { get; private set; }

    public bool Skill_Q_Triggered {get; private set; }
    public bool Skill_W_Triggered {get; private set; }
    public bool Skill_E_Triggered { get; private set; }

    public bool Setting_Triggered { get; private set; }
    #endregion

    private void Awake()
    {
        if (player_Input == null)
        {
            player_Input = GetComponent<PlayerInput>();
            Debug.Log("Set_player_Input");
        }
        Resolve_Action();
    }

    void Update()
    {
        Move_Vector_2D = move_Action != null ? move_Action.ReadValue<Vector2>() : Vector2.zero;

        Dash_Triggered = dash_Action != null && dash_Action.triggered;

        Attack_Triggered = attack_Action != null && attack_Action.triggered;

        Use_Item_1_Triggered = use_Item_1_Action != null && use_Item_1_Action.triggered;
        Use_Item_2_Triggered = use_Item_2_Action != null && use_Item_2_Action.triggered;
        Use_Item_3_Triggered = use_Item_3_Action != null && use_Item_3_Action.triggered;
        Use_Item_4_Triggered = use_Item_4_Action != null && use_Item_4_Action.triggered;
        Use_Item_5_Triggered = use_Item_5_Action != null && use_Item_5_Action.triggered;

        Skill_Q_Triggered = skill_Q_Action != null && skill_Q_Action.triggered;
        Skill_W_Triggered = skill_W_Action != null && skill_W_Action.triggered;
        Skill_E_Triggered = skill_E_Action != null && skill_E_Action.triggered;

        Move_Check();
    }

    private void Move_Check() 
    {
        if (Move_Vector_2D == Vector2.zero)
        {
            is_Moving = false;
        }
        else
        {
            is_Moving = true;
        }
    }

    private void Resolve_Action()
    {
        if (player_Input == null || player_Input.actions == null)
        {
            Debug.Log("player_Input == null || player_Input.actions == null");
            return;
        }

        move_Action = Get_Action("Move");
        dash_Action = Get_Action("Dash");
        attack_Action = Get_Action("Attack");

        use_Item_1_Action = Get_Action("Use_Item_1");
        use_Item_2_Action = Get_Action("Use_Item_2");
        use_Item_3_Action = Get_Action("Use_Item_3");
        use_Item_4_Action = Get_Action("Use_Item_4");
        use_Item_5_Action = Get_Action("Use_Item_5");

        skill_Q_Action = Get_Action("Skill_Q");
        skill_W_Action = Get_Action("Skill_W");
        skill_E_Action = Get_Action("Skill_E");
    }

    private InputAction Get_Action(string action_Name)
    {
        if (string.IsNullOrEmpty(action_Name))
        {
            Debug.LogError("string.IsNullOrEmpty(action_Name)");
            return null;
        }

        InputAction action = player_Input.actions.FindAction(action_Name, false);

        if (action == null)
        {
            Debug.LogError("action == null");
        }
        return action;
    }
}
