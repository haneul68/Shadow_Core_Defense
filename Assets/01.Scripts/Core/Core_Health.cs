using System;
using UnityEngine;

public class Core_Health : MonoBehaviour, IDamageable
{
    [Header("Core Gauge")]
    [SerializeField] private int max_Gauge = 10;
    private int current_Gauge = 0;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    public int Current_Gauge => current_Gauge;
    public int Max_Gauge => max_Gauge;

    public event Action<int, int> On_Gauge_Changed;

    public bool is_Dead { get; set; }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        is_Dead = false;

        Invoke_Gauge_Event();
        Set_Active_Anim(false);
    }

    public void TakeDamage(double damage)
    {
        if (is_Dead) return;
        Debug.Log("Core : TakeDamage");
        current_Gauge = Mathf.Max(0, current_Gauge - 1);

        Invoke_Gauge_Event();

        if (current_Gauge <= 0)
            Die();
    }

    public void Heal(double amount, bool show_Text = false)
    {
        if (is_Dead) return;

        current_Gauge = Mathf.Min(max_Gauge, current_Gauge + 1);

        Invoke_Gauge_Event();
    }

    public void Set_Active_State(bool is_Active)
    {
        if (is_Dead) return;

        Set_Active_Anim(is_Active);
    }

    private void Set_Active_Anim(bool value)
    {
        if (animator == null) return;

        animator.SetBool(Animation_Parameter_Hash.Core_Is_Active_Hash, value);
    }

    private void Invoke_Gauge_Event()
    {
        On_Gauge_Changed?.Invoke(current_Gauge, max_Gauge);
    }

    private void Die()
    {
        if (is_Dead) return;

        is_Dead = true;

        Set_Active_Anim(false);

        InGame_State_Manager.State_Change(InGame_State.DEATH);
        Debug.Log("[Core] GAME OVER");
    }
}