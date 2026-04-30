using UnityEngine;

public class Boss_Direction_Ani_Controller : MonoBehaviour
{
    [Header("REF")]
    [SerializeField]
    private Animator animator;

    private IDirection_Provider direction_Povider;
    private IAttack attacker;

    private Vector2 last_Dir;

    private void Awake()
    {
        if (direction_Povider == null)
            direction_Povider = GetComponent<IDirection_Provider>();

        if (attacker == null)
            attacker = GetComponent<IAttack>();
    }

    private void OnDisable()
    {
        last_Dir = new Vector2(999f,999f);
    }

    private void Update()
    {
        if (direction_Povider == null || animator == null)
            return;

        if (attacker != null && attacker.Is_Attack)
            return;

        Update_Animation();
    }

    private void Update_Animation()
    {
        Vector2 dir = direction_Povider.Last_Facing_Dir;

        if (dir == last_Dir) return;

        last_Dir = dir;

        animator.SetFloat(Animation_Parameter_Hash.move_X_Hash, dir.x);
        animator.SetFloat(Animation_Parameter_Hash.move_Y_Hash, dir.y);
    }
}