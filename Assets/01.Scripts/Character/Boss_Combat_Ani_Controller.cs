using UnityEngine;

public class Boss_Combat_Ani_Controller : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Health_Manager health_Manager;

    private IDirection_Provider direction_Povider;

    private bool is_Attack;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (health_Manager == null)
            health_Manager = GetComponent<Health_Manager>();

        if (direction_Povider == null)
            direction_Povider = GetComponentInParent<IDirection_Provider>();
    }

    private void OnEnable()
    {
        health_Manager.On_Died += Handle_Die;
        health_Manager.On_Hit += Handle_Hit;
    }

    private void OnDisable()
    {
        health_Manager.On_Died -= Handle_Die;
        health_Manager.On_Hit -= Handle_Hit;
    }
    #region Attack

    public void Handle_Attack(bool is_Attack_Skill)
    {
        if (is_Attack_Skill == false) return;

        if (direction_Povider == null)
        {
            Debug.LogError("direction_Povider == null");
            return;
        }

        if (animator == null)
        {
            Debug.LogError("animator == null");
            return;
        }

        Vector2 dir = direction_Povider.Last_Facing_Dir;

        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.attack_Hash);
    }

    #endregion

    #region Hit & Die

    private void Handle_Hit()
    {
        Vector2 dir = direction_Povider.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Hit_Hash);
    }

    private void Handle_Die()
    {
        Vector2 dir = direction_Povider.Last_Facing_Dir;
        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);

        animator.SetTrigger(Animation_Parameter_Hash.Death_Hash);
    }
    #endregion
}
