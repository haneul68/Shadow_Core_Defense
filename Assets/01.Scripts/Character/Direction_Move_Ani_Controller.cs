using UnityEngine;

public class Direction_Move_Ani_Controller : MonoBehaviour
{
    [Header("REF")]
    [SerializeField] 
    private Animator animator;

    private IDirection_Provider direction_Povider;
    private IAttack attacker;


    private float input_Dead_Zone = 0.1f;

    public Vector2 Current_Input_Dir { get; private set; }

    public bool Is_Moving { get; private set; }

    private void Awake()
    {
        if (direction_Povider == null) 
        {
            direction_Povider = GetComponent<IDirection_Provider>();
        }
        if (attacker == null)
        {
            attacker = GetComponent<IAttack>();
        }
    }

    void Update()
    {
        if (direction_Povider == null || animator == null || attacker == null)
        {
            Debug.Log("direction_Povider == null || animator == null || attacker == null");
            return;
        }

        if (attacker.Is_Attack == true) 
        {
            return;
        }

        Vector2 raw_Input = direction_Povider.Get_Direction();

        Current_Input_Dir = raw_Input;

        Is_Moving = raw_Input.sqrMagnitude >= input_Dead_Zone * input_Dead_Zone;

        Apply_Move_Animation(Is_Moving);
    }
    #region Move
    private void Apply_Move_Animation(bool is_Moving)
    {

        Vector2 dir = direction_Povider.Last_Facing_Dir;

        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);
        animator.SetBool(Animation_Parameter_Hash.is_Moving_Hash, is_Moving);
    }
    #endregion
}
