using UnityEngine;
public class Dir_Anim_Controller : MonoBehaviour
{
    [Header("REF")]
    [SerializeField] 
    private Player_Input_Manager player_Input_Manager;
    [SerializeField] 
    private Player_Move player_Move;
    [SerializeField] 
    private Animator animator;
    [SerializeField] 
    private Melee_Attack melee_Attack;
    [SerializeField] 
    private Health_Manager health_Manager;

    private float input_Dead_Zone = 0.1f;
    public Vector2 Current_Input_Dir { get; private set; }

    public bool Is_Moving { get; private set; }

    private void Start()
    {
        health_Manager.On_Died += Handle_Die;
        health_Manager.On_Hit += Handle_Hit;
    }

    void Update()
    {
        if (player_Input_Manager == null || animator == null || player_Move == null || melee_Attack == null)
        {
            Debug.Log("(player_Input_Manager == null || animator == null || player_Move == null || melee_Attack == null");
            return;
        }

        Vector2 raw_Input = player_Input_Manager.Move_Vector_2D;

        Current_Input_Dir = raw_Input;

        Is_Moving = raw_Input.sqrMagnitude >= input_Dead_Zone * input_Dead_Zone;

        Apply_Move_Animation(Is_Moving);

        if (player_Input_Manager != null && player_Input_Manager.Attack_Triggered && !player_Input_Manager.Dash_Triggered && melee_Attack.Is_Attack == false)
        {
            Handle_Attack();
        }
    }
    #region Move
    private void Apply_Move_Animation(bool is_Moving)
    {
        Vector2 dir = player_Move.Last_Facing_Dir;

        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);
        animator.SetBool(Animation_Parameter_Hash.is_Moving_Hash, is_Moving);
    }
    #endregion

    #region Attack

    private void Handle_Attack() 
    {
        Vector2 dir = player_Move.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.attack_Hash);
    }

    #endregion

    #region Hit & Die

    private void Handle_Hit()
    {
        Debug.Log("Handle_Hit");
        Vector2 dir = player_Move.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Hit_Hash);
    }

    private void Handle_Die()
    {
        Debug.Log("Handle_Die");
        Vector2 dir = player_Move.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Death_Hash);
    }
    #endregion
}
