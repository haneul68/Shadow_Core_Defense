using UnityEngine;

public class Combat_Ani_Controller : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Health_Manager health_Manager;

    private IDirection_Provider direction_Povider;
    private IAttack attacker;

    private void Awake()
    {
        if (attacker == null)
        {
            attacker = GetComponent<IAttack>();
        }

        if (direction_Povider == null)
        {
            direction_Povider = GetComponent<IDirection_Provider>();
        }
    }

    private void OnEnable()
    {
        attacker.OnAttack += Handle_Attack;
        health_Manager.On_Died += Handle_Die;
        health_Manager.On_Hit += Handle_Hit;
    }

    private void OnDisable()
    {
        attacker.OnAttack -= Handle_Attack;
        health_Manager.On_Died -= Handle_Die;
        health_Manager.On_Hit -= Handle_Hit;
    }
    #region Attack

    private void Handle_Attack()
    {
        Debug.Log("Handle_Attack");
        Vector2 dir = direction_Povider.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.attack_Hash);
    }

    #endregion

    #region Hit & Die

    private void Handle_Hit()
    {
        Debug.Log("Handle_Hit");
        Vector2 dir = direction_Povider.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Hit_Hash);
    }

    private void Handle_Die()
    {
        Debug.Log("Handle_Die");
        Vector2 dir = direction_Povider.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Death_Hash);
    }
    #endregion
}

