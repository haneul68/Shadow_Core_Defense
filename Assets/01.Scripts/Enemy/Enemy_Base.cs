using System;
using UnityEngine;

public class Enemy_Base : MonoBehaviour, IHealth_Source, IMinimap_Target
{
    [SerializeField] private string enemy_Name;
    [SerializeField] private Pool_ID pool_ID;

    public RectTransform minimap_Icon;
    public RectTransform Get_Minimap_Icon() => minimap_Icon;

    #region Base Stats (타이틀/로비에서 기본값)
    [SerializeField] private double base_ATK;
    private double base_Max_HP;
    private float base_Move_Speed;
    private float attack_Distance;
    private Enemy_Type enemy_Type;
    #endregion

    #region Runtime Modifiers (버프/디버프 전용, 인게임)
    [SerializeField] private double bonus_ATK;
    [SerializeField] private double bonus_Max_HP;

    private float bonus_Move_Speed;
    #endregion

    #region Final Stats (실제 인게임에서 사용)
    public double Final_ATK => base_ATK + bonus_ATK;
    public double Final_Max_HP => base_Max_HP + bonus_Max_HP;
    public float Final_Move_Speed => base_Move_Speed + bonus_Move_Speed;
    #endregion

    #region Current Stats
    private double current_HP;

    public double Current_HP
    {
        get => current_HP;
        set => current_HP = Math.Clamp(value, 0, Final_Max_HP);
    }
    #endregion

    #region Buff / Debuff 적용 함수
    public void Add_ATK_Buff(double amount) => bonus_ATK += amount;
    public void Remove_ATK_Buff(double amount) => bonus_ATK -= amount;

    public void Add_HP_Buff(double amount)
    {
        bonus_Max_HP += amount;
        Current_HP = Math.Clamp(Current_HP, 0, Final_Max_HP);
    }
    public void Remove_HP_Buff(double amount)
    {
        bonus_Max_HP -= amount;
        Current_HP = Math.Clamp(Current_HP, 0, Final_Max_HP);
    }
  
    public void Add_Speed_Buff(float amount) => bonus_Move_Speed += amount;
    public void Remove_Speed_Buff(float amount) => bonus_Move_Speed -= amount;
    #endregion

    #region Base Stat Properties (읽기/쓰기)
    public double Base_ATK
    {
        get => base_ATK;
        set => base_ATK = Math.Max(0, value);
    }

    public double Base_Max_HP
    {
        get => base_Max_HP;
        set => base_Max_HP = Math.Max(0, value);
    }

    public float Base_Move_Speed
    {
        get => base_Move_Speed;
        set => base_Move_Speed = Mathf.Max(0, value);
    }

    public Enemy_Type Enemy_Type => enemy_Type;

    public Pool_ID Enemy_Pool_ID => pool_ID;

    public float Attack_Distance => attack_Distance;

    #endregion

    public virtual void Init()
    {
        base_ATK = Base_Manager.Enemy_Mng.Get_ATK(enemy_Name);
        base_Max_HP = Base_Manager.Enemy_Mng.Get_Max_HP(enemy_Name);
        base_Move_Speed = Base_Manager.Enemy_Mng.Get_Move_Speed(enemy_Name);

        current_HP = base_Max_HP;
        
        attack_Distance = Base_Manager.Enemy_Mng.Get_Enemy_Attack_Distance(enemy_Name);
        enemy_Type = Base_Manager.Enemy_Mng.Get_Enemy_Type(enemy_Name); 

        //Debug.Log($"Enemy Initialized: ATK={base_ATK}, MaxHP={base_Max_HP}, CurrentHP={current_HP}, MoveSpeed={base_Move_Speed}");
    }

    protected void Re_Set_Bonus_Stats()
    {
        bonus_ATK = 0;
        bonus_Max_HP = 0;
        bonus_Move_Speed = 0;
    }
    public Transform Get_Transform() => transform;

}
